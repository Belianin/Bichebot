using Discord.WebSocket;

namespace Bichebot
{
    public interface IBotCore
    {
        SocketGuild Guild { get; }

        string ToEmojiString(string text);
    }
}