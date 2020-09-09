using System;
using System.Collections.Generic;
using Bichebot.Core.Banking;
using Discord;
using Discord.WebSocket;
using Nexus.Logging;

namespace Bichebot.Core
{
    public interface IBotCore
    {
        IBankCore Bank { get; }

        ILog Log { get; }

        DiscordSocketClient Client { get; }

        SocketGuild Guild { get; }

        Random Random { get; }

        string ToEmojiString(string text);

        IEnumerable<IUserMessage> GetMessages(IMessageChannel channel, TimeSpan period);
    }
}