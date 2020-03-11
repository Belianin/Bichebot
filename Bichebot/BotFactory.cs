using System.Collections.Generic;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Audio;
using Bichebot.Modules.Best;
using Bichebot.Modules.Greeter;
using Bichebot.Modules.Moderate;
using Bichebot.Modules.React;
using Bichebot.Modules.Statistics;
using Bichebot.Modules.Supreme;
using Bichebot.Modules.Survey;
using Discord.WebSocket;

namespace Bichebot
{
    public static class BotFactory
    {
        public static Bot Create(BotSettings settings)
        {
            var discordClient = new DiscordSocketClient();
            var core = new BotCore(settings.GuildId, discordClient);
            var audio = new AudioSpeaker(core);
            var modules = new List<IBotModule>
            {
                new StatisticsModule(core),
                new ReactModule(core),
                new ModerateModule(core),
                new SupremeModule(core),
                new AudioModule(core, audio),
                new SurveyModule(core, () => new SurveyState(), Questions),
                new BestModule(core, new BestModuleSettings
                {
                    BestChannelId = settings.BestChannelId,
                    ReactionCountToBeBest = settings.ReactionCountToBeBest
                }),
                new GreeterModule(core, audio, new GreeterModuleSettings
                {
                    Greetings = new Dictionary<ulong, string>
                    {
                        {177461516646219776, "host.mp3"},
                        {177455722248798208, "qwear.mp3"},
                        {272446177163608066, "aler.mp3"},
                        {490208786036948994, "ilidan.mp3"}
                    }
                })
            };
            
            return new Bot(core, modules, settings.Token);
        }
        
        private static List<string> Questions => new List<string>
        {
            "lifer",
            "igorbamboe",
            "alohabamboe"
        };
    }
}