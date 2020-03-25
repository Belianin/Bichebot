using System.Collections.Generic;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Audio;
using Bichebot.Modules.Bank;
using Bichebot.Modules.Base;
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
            var bankCore = new BankCore(new CachingRepository<ulong, int>(new FakeRepository<ulong, int>()));
            var audio = new AudioSpeaker(core);
            var modules = new List<IBotModule>
            {
                new StatisticsModule(core),
                new ReactModule(core, new ReactModuleSettings()),
                new ModerateModule(core),
                new SupremeModule(core),
                new AudioModule(core, audio),
                new SurveyModule(core, () => new SurveyState(), Questions),
                new BankModule(core, bankCore, new BankModuleSettings(new List<ulong>{272446177163608066})),
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
        private static List<string> Questions3 => new List<string>
        {
            "lootecbamboe",
            "jet",
            "lifer"
        };
        private static List<string> Questions => new List<string>
        {
            "lootecbamboe",
            "jet",
            "lifer",
            "supremebamboe",
            "alohabamboe",
            "lejatbamboe",
            "roflanbamboe",
            "quasilegend",
            "badoobamboe",
            "spongebamboe",
            "hitlerbamboe",
            "dobrobamboe",
            "oldbamboe",
            "qwirbamboe",
            "kadikbamboe",
            "kripotabamboe",
            "lyabamboe",
            "liquidbamboe",
            "igorbamboe2",
            "slivbamboe",
            "vasyanbamboe",
            "newfagbamboe",
            "deadinside",
            "bombitbamboe",
            "fbamboe",
            "coolstory",
            "imbabamboe",
            "qwirchamp",
            "mujikbamboe",
            "nolifer",
            "t3",
            "valera",
            "oroobamboe",
            "lol2",
            "dota",
            "lol",
            "lejatbamboe2",
            "oldigorbamboe",
            "olddobrobamboe",
            "oldlejatbamboe",
            "nesspride3",
            "ohrenelbamboe",
            "papichbamboe",
            "thinkingbamboe",
            "hellobamboe",
            "thonkbamboe",
            "thonkbamboe2",
            "kislenkobamboe",
            "nivgbamboe",
            "likebamboe"
        };
    }
}