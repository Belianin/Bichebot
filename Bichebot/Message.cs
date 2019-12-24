using Discord.WebSocket;

namespace Bichebot
{
    public class Message
    {
        public SocketMessage DiscordMessage { get; set; }

        public bool Tts { get; set; } = false;
    }
}