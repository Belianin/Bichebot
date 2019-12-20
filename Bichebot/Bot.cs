using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Bichebot
{
    public class Bot
    {
        private SocketGuild Guild => discordClient.Guilds.First(g => g.Id == config.GuildId);

        private readonly BotConfig config;
        
        private readonly DiscordSocketClient discordClient;
        
        private readonly HashSet<ulong> alreadyBest = new HashSet<ulong>();
        
        private readonly Random rnd = new Random();
        
        public Bot(BotConfig config)
        {
            this.config = config;
            
            discordClient = new DiscordSocketClient();
            discordClient.ReactionAdded += HandleReactionAsync;
            discordClient.MessageReceived += HandleMessageAsync;
        }


        public async Task RunAsync(CancellationToken token)
        {
            Console.WriteLine("Starting");
            await discordClient.LoginAsync(TokenType.Bot, config.Token)
                .ConfigureAwait(false);
            await discordClient.StartAsync().ConfigureAwait(false);
            Console.WriteLine("Started");

            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
        }

        private async Task HandleMessageAsync(SocketMessage message)
        {
            Console.WriteLine(message.Content); //  /t4 7
            var args = Regex.Split(message.Content, @"\s+");
            if (args[0] == "/t4")
            {
                var days = 3;
                if (args.Length > 1)
                    int.TryParse(args[1], out days);
                
                var rates = GetEmoteReactionsRating(GetMessages(TimeSpan.FromDays(days), message.Channel))
                    .OrderByDescending(r => r.Count)
                    .Take(10);
                
                var response = $"Величайшие смайлы:\n{JoinEmoteStatistics(rates)}";

                await message.Channel.SendMessageAsync(response).ConfigureAwait(false);
                
            }
            else if (args[0] == "/t5")
            {
                var days = 3;
                if (args.Length > 1)
                    int.TryParse(args[1], out days);
                
                var rates = GetEmoteUsingsRating(GetMessages(TimeSpan.FromDays(days), message.Channel))
                    .OrderByDescending(r => r.Count)
                    .Take(10);
                
                var response = $"Величайшие смайлы:\n{JoinEmoteStatistics(rates)}";

                await message.Channel.SendMessageAsync(response).ConfigureAwait(false);
                
            }
            else if (message.Content.Contains("лол"))
            {
                if (rnd.Next(0, 100) > 50)
                {
                    await message.Channel.SendMessageAsync(
                            $"{ToEmojiString("dobrobamboe")} может лучше в {ToEmojiString("supremebamboe")}?")
                        .ConfigureAwait(false);
                }
                else if (rnd.Next(0, 100) > 50)
                {
                    await message.Channel.SendMessageAsync(
                            $"{ToEmojiString("dobrobamboe")} ты хотел сказать {ToEmojiString("supremebamboe")}?")
                        .ConfigureAwait(false);
                }
                else
                {
                    await message.Channel.SendMessageAsync(
                            $"{ToEmojiString("valera")} ну лан")
                        .ConfigureAwait(false);
                }
            }
            else
            {
                if (rnd.Next(0, 1000) == 999)
                {
                    await message.Channel.SendMessageAsync("Скатился...").ConfigureAwait(false);
                }
            }
        }

        private string JoinEmoteStatistics(IEnumerable<Statistic> statistics)
        {
            return string.Join("\n", statistics.Select(e => $"{ToEmojiString(e.Value)}: {e.Count}"));
        }

        private static IEnumerable<IUserMessage> GetMessages(TimeSpan period, IMessageChannel channel)
        {
            var timestamp = DateTime.UtcNow;
            ulong lastId = 0;
            
            var messages = channel.GetMessagesAsync().Flatten();
            var enumerator = messages.GetEnumerator();
            var isEmpty = true;
            do
            {
                while (enumerator.MoveNext().Result)
                {
                    isEmpty = false;
                    lastId = enumerator.Current.Id;
                    if (enumerator.Current.Timestamp.UtcDateTime < timestamp)
                        timestamp = enumerator.Current.Timestamp.UtcDateTime;
                    if (enumerator.Current is IUserMessage userMessage)
                        yield return userMessage;
                }

                Console.WriteLine(timestamp);

                enumerator = channel.GetMessagesAsync(lastId, Direction.Before).Flatten().GetEnumerator();
            } while (!isEmpty && timestamp.Add(period) > DateTime.UtcNow);
        }

        private static IEnumerable<Statistic> GetEmoteUsingsRating(IEnumerable<IUserMessage> messages)
        {
            var result = new Dictionary<string, int>();
            var regex = new Regex(@":(.*?):");

            var emotes = messages
                .Where(m => !m.Author.IsBot)
                .SelectMany(m => regex.Matches(m.Content));
            
            foreach (var emote in emotes)
            {
                var value = emote.ToString().Substring(1, emote.ToString().Length - 2);
                if (!result.ContainsKey(value))
                    result[value] = 0;

                result[value] += 1;
            }

            return result.Select(v => new Statistic
            {
                Count = v.Value,
                Value = v.Key
            });
        }

        private static IEnumerable<Statistic> GetEmoteReactionsRating(IEnumerable<IUserMessage> messages)
        {
            var result = new Dictionary<string, int>();
            
            foreach (var reaction in messages.SelectMany(m => m.Reactions))
            {
                if (!result.ContainsKey(reaction.Key.Name))
                    result[reaction.Key.Name] = 0;

                result[reaction.Key.Name] += reaction.Value.ReactionCount;
            }

            return result.Select(v => new Statistic
            {
                Count = v.Value,
                Value = v.Key
            });
        }

        private async Task HandleReactionAsync(
            Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            Console.WriteLine("Reaction");
            var message = await channel.GetMessageAsync(reaction.MessageId).ConfigureAwait(false);
            
            if (message is RestUserMessage userMessage)
            {
                await SendToBestChannelAsync(userMessage).ConfigureAwait(false);
            }
        }

        private async Task SendToBestChannelAsync(IUserMessage userMessage)
        {
            if (alreadyBest.Contains(userMessage.Id) ||
                !userMessage.Reactions.Values.Any(r => r.ReactionCount >= config.ReactionCountToBeBest) ||
                userMessage.Channel.Id == config.BestChannelId)
                return;
            
            alreadyBest.Add(userMessage.Id);

            var emotes = string.Join("", userMessage
                .Reactions
                .OrderByDescending(r => r.Value.ReactionCount)
                .SelectMany(e => Enumerable.Repeat(ToEmojiString(e.Key.Name), e.Value.ReactionCount)));

            var embed = new EmbedBuilder()
                .WithAuthor(userMessage.Author)
                .WithTitle(userMessage.Content)
                .WithFooter("#бичехосты-лучшее")
                .WithDescription(emotes)
                .WithTimestamp(userMessage.Timestamp);

            if (userMessage.Attachments.Count > 0)
                embed.WithImageUrl(userMessage.Attachments.First().Url);

            await Guild.GetTextChannel(config.BestChannelId).SendMessageAsync(embed: embed.Build())
                .ConfigureAwait(false);
        }

        private string ToEmojiString(string text)
        {
            var emote = Guild.Emotes.FirstOrDefault(e => e.Name == text);
            if (emote == null)
                return text;
            
            return $"<:{emote.Name}:{emote.Id}>";
        }
    }
}