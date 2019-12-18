using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Bichebot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var token = Environment.GetEnvironmentVariable("BICHEBOT_TOKEN", EnvironmentVariableTarget.User);
            string mongoPassword;
            if (token != null)
            {
                mongoPassword = Environment.GetEnvironmentVariable("MONGODB_PASSWORD", EnvironmentVariableTarget.User);
            }
            else
            {
                token = Environment.GetEnvironmentVariable("BICHEBOT_TOKEN");
                mongoPassword = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
            }
            new Bot(token, new MongoEmoteStatisticsRepository(mongoPassword))
                .RunAsync().Wait();
        }
    }

    public class BotConfig
    {
        public string Token { get; set; }
        
        public string GuildId { get; set; }
    }

    public class Bot
    {
        private DiscordSocketClient discordClient;

        private ulong serverId = 307817006021738507;

        private SocketGuild guild => discordClient.Guilds.First(g => g.Id == serverId);

        private string token = "";
        
        private ulong bestChannelId = 602475515391246358;

        private int reactionCountToBeBest = 3;
        
        private HashSet<ulong> alreadyBest = new HashSet<ulong>();

        private readonly IEmoteStatisticsRepository statistics;

        public Bot(string token, IEmoteStatisticsRepository statistics)
        {
            this.token = token;
            this.statistics = statistics;
            discordClient = new DiscordSocketClient();
            discordClient.ReactionAdded += HandleReactionAsync;
            discordClient.MessageReceived += HandleMessage;
        }

        private async Task HandleMessage(SocketMessage message)
        {
            Console.WriteLine(message);
            if (message.Content.Contains("/stat")) // Костыль message.MentionedUsers.FirstOrDefault(u => u.IsBot) != null && 
            {
                var result = await statistics.GetAllStatisticsAsync().ConfigureAwait(false);

                var response = result == null ? "Что-то не то..." : "Величайшие смайлы:\n" + string.Join("\n", result.OrderByDescending(e => e.ReactionCount).Select(e => $"{ToEmojiString(e.Name)}: {e.ReactionCount}"));

                await message.Channel.SendMessageAsync(response).ConfigureAwait(false);
            }
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
            
            await statistics.IncrementAsync(reaction.Emote.Name).ConfigureAwait(false);

            //await channel.SendMessageAsync($"Хм, \"{message.Content}\" вызывает у тебя {ToEmojiString(reaction.Emote.Name)}? Понятно...").ConfigureAwait(false);
        }

        private async Task SendToBestChannelAsync(IUserMessage userMessage)
        {
            if (!alreadyBest.Contains(userMessage.Id) && userMessage.Reactions.Values.Any(r => r.ReactionCount >= 4) && userMessage.Channel.Id != bestChannelId)
            {
                alreadyBest.Add(userMessage.Id);
                await guild.GetTextChannel(bestChannelId)
                    .SendMessageAsync(userMessage.Content).ConfigureAwait(false);
            }
        }

        public async Task RunAsync()
        {
            await discordClient.LoginAsync(TokenType.Bot, token)
                .ConfigureAwait(false);
            await discordClient.StartAsync().ConfigureAwait(false);

            while (true)
            {
                Thread.Sleep(10000);
            }
        }
        
        public string ToEmojiString(string text)
        {
            var emote = guild.Emotes.FirstOrDefault(e => e.Name == text);
            if (emote == null)
                return text;
            
            return $"<:{emote.Name}:{emote.Id}>";
        }
    }
}