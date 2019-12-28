using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Core
{
    public interface IBotCore
    {
        DiscordSocketClient Client { get; }
        SocketGuild Guild { get; }

        string ToEmojiString(string text);

        IEnumerable<IUserMessage> GetMessages(IMessageChannel channel, TimeSpan period);

    }
}