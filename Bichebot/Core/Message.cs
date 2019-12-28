using Discord.WebSocket;

namespace Bichebot.Core
{
    public class Message
    {
        public SocketMessage DiscordMessage { get; set; }

        public bool Tts { get; set; } = false;
    }
}