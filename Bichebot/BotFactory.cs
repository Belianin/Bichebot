using System.Collections.Generic;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Best;
using Bichebot.Modules.Moderate;
using Bichebot.Modules.React;
using Bichebot.Modules.Statistics;
using Bichebot.Modules.Supreme;
using Discord.WebSocket;

namespace Bichebot
{
    public static class BotFactory
    {
        public static Bot Create(BotConfig config)
        {
            var discordClient = new DiscordSocketClient();
            var core = new BotCore(config.GuildId, discordClient);
            var modules = new List<IBotModule>
            {
                new StatisticsModule(core),
                new ReactModule(core),
                new ModerateModule(core),
                new SupremeModule(core),
                new BestModule(core, new BestModuleSettings
                {
                    BestChannelId = config.BestChannelId,
                    ReactionCountToBeBest = config.ReactionCountToBeBest
                })
            };
            
            return new Bot(core, modules, config.Token);
        }
    }
}