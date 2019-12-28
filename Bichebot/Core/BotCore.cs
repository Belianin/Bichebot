using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Core
{
    public class BotCore : IBotCore
    {
        private readonly ulong guildId;

        private readonly DiscordSocketClient discordClient;

        public BotCore(ulong guildId, DiscordSocketClient discordClient)
        {
            this.guildId = guildId;
            this.discordClient = discordClient;
        }

        public SocketGuild Guild => discordClient.Guilds.First(g => g.Id == guildId);

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
    }
}