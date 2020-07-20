using System;
using System.Collections.Generic;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.React;
using Bichebot.Modules.React.Triggers;
using Bichebot.Modules.React.Triggers.Domain;
using Bichebot.Repositories;
using Discord.WebSocket;

namespace Bichebot
{
    public static class BotFactory
    {
        public static IBotFactory Instance => new DefaultBotFactory();
    }
}