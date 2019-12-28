using System.Collections.Generic;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Audio;
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
        public static Bot Create(BotSettings settings)
        {
            var discordClient = new DiscordSocketClient();
            var core = new BotCore(settings.GuildId, discordClient);
            var modules = new List<IBotModule>
            {
                new StatisticsModule(core),
                new ReactModule(core),
                new ModerateModule(core),
                new SupremeModule(core),
                new AudioModule(core),
                new BestModule(core, new BestModuleSettings
                {
                    BestChannelId = settings.BestChannelId,
                    ReactionCountToBeBest = settings.ReactionCountToBeBest
                })
            };
            
            return new Bot(core, modules, settings.Token);
        }
    }
}