using System.Linq;
using Discord.WebSocket;

namespace Bichebot
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
    }
}