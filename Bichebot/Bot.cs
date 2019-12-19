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
        public Bot(BotConfig config)
        {
            this.config = config;
            
            discordClient = new DiscordSocketClient();
            discordClient.ReactionAdded += HandleReactionAsync;
            discordClient.MessageReceived += HandleMessage;
        }

        public async Task RunAsync(CancellationToken token)
        {
            await discordClient.LoginAsync(TokenType.Bot, config.Token)
                .ConfigureAwait(false);
            await discordClient.StartAsync().ConfigureAwait(false);

            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
        }

        private async Task HandleMessage(SocketMessage message)
        {
            Console.WriteLine(message);
            if (message.Content.Contains("/t4"))
            {
                var rates = GetEmoteReactionsRating(GetMessages(TimeSpan.FromDays(3), message.Channel))
                    .OrderByDescending(r => r.Value)
                    .Take(10);
                
                var response = "Величайшие смайлы:\n" + string.Join("\n", rates.Select(e => $"{ToEmojiString(e.Key)}: {e.Value}"));

                await message.Channel.SendMessageAsync(response).ConfigureAwait(false);
                
            }
            else if (message.Content.Contains("/t5"))
            {
                var rates = GetEmoteUsingsRating(GetMessages(TimeSpan.FromDays(3), message.Channel))
                    .OrderByDescending(r => r.Value)
                    .Take(10);
                
                var response = "Величайшие смайлы:\n" + string.Join("\n", rates.Select(e => $"{ToEmojiString(e.Key)}: {e.Value}"));

                await message.Channel.SendMessageAsync(response).ConfigureAwait(false);
                
            }
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

                enumerator = channel.GetMessagesAsync(lastId, Direction.Before).Flatten().GetEnumerator();
            } while (!isEmpty && timestamp.Add(period) < DateTime.UtcNow);
        }

        private static Dictionary<string, int> GetEmoteUsingsRating(IEnumerable<IUserMessage> messages)
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

            return result;
        }

        private static Dictionary<string, int> GetEmoteReactionsRating(IEnumerable<IUserMessage> messages)
        {
            var result = new Dictionary<string, int>();
            
            foreach (var reaction in messages.SelectMany(m => m.Reactions))
            {
                if (!result.ContainsKey(reaction.Key.Name))
                    result[reaction.Key.Name] = 0;

                result[reaction.Key.Name] += reaction.Value.ReactionCount;
            }

            return result;
        }

        private async Task HandleReactionAsync(
            Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            var message = await channel.GetMessageAsync(reaction.MessageId).ConfigureAwait(false);
            
            if (message is RestUserMessage userMessage)
            {
                await SendToBestChannelAsync(userMessage).ConfigureAwait(false);
            }
        }

        private async Task SendToBestChannelAsync(IUserMessage userMessage)
        {
            if (alreadyBest.Contains(userMessage.Id) ||
                !userMessage.Reactions.Values.Any(r => r.ReactionCount >= 4) ||
                userMessage.Channel.Id == config.BestChannelId)
                return;
            
            alreadyBest.Add(userMessage.Id);
            await Guild.GetTextChannel(config.BestChannelId).SendMessageAsync(userMessage.Content)
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