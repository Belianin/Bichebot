using System;
using System.Collections.Generic;
using System.Linq;
using Bichebot.Core.Banking;
using Discord;
using Discord.WebSocket;
using Nexus.Logging;

namespace Bichebot.Core
{
    public class BotCore : IBotCore
    {
        private readonly ulong guildId;

        public BotCore(ulong guildId, DiscordSocketClient discordClient, IBankCore bank, ILog log)
        {
            this.guildId = guildId;
            Client = discordClient;
            Bank = bank;
            Log = log;
        }

        public IBankCore Bank { get; }
        public ILog Log { get; }
        public DiscordSocketClient Client { get; }

        public SocketGuild Guild => Client.Guilds.First(g => g.Id == guildId);

        public Random Random { get; } = new Random();

        public string ToEmojiString(string text)
        {
            var emote = Guild.Emotes.FirstOrDefault(e => e.Name == text);
            if (emote == null)
                return $":{text}:";

            return emote.Animated ? $"<a:{emote.Name}:{emote.Id}>" : $"<:{emote.Name}:{emote.Id}>";
        }

        public IEnumerable<IUserMessage> GetMessages(IMessageChannel channel, TimeSpan period)
        {
            var timestamp = DateTime.UtcNow;
            ulong lastId = 0;

            var messages = channel.GetMessagesAsync().Flatten();
            var enumerator = messages.GetAsyncEnumerator();
            var isEmpty = true;
            do
            {
                while (enumerator.MoveNextAsync().Result)
                {
                    isEmpty = false;
                    lastId = enumerator.Current.Id;
                    if (enumerator.Current.Timestamp.UtcDateTime < timestamp)
                        timestamp = enumerator.Current.Timestamp.UtcDateTime;
                    if (enumerator.Current is IUserMessage userMessage)
                        yield return userMessage;
                }

                Console.WriteLine(timestamp);

                enumerator = channel.GetMessagesAsync(lastId, Direction.Before).Flatten().GetAsyncEnumerator();
            } while (!isEmpty && timestamp.Add(period) > DateTime.UtcNow);
        }
    }
}