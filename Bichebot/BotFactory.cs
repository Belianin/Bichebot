using System.Collections.Generic;
using System.IO;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Audio;
using Bichebot.Modules.Bank;
using Bichebot.Modules.Best;
using Bichebot.Modules.Greeter;
using Bichebot.Modules.MemeGenerator;
using Bichebot.Modules.Moderate;
using Bichebot.Modules.React;
using Bichebot.Modules.React.Triggers;
using Bichebot.Modules.React.Triggers.Domain;
using Bichebot.Modules.Statistics;
using Bichebot.Modules.Supreme;
using Bichebot.Repositories;
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
                new ReactModule(core, new ReactModuleSettings
                {
                    Triggers = new IReactionTrigger[]
                    {
                        new ContentTrigger(core),
                        new HelloTrigger(core),
                        new LeagueOfLegendsTrigger(core),
                        new LikeTrigger(core),
                        new QuestionTrigger(core),
                        new RareTrigger(core),
                        new RudeTrigger(core), 
                    }
                }),
                new ModerateModule(core),
                new SupremeModule(core),
                new AudioModule(core, audio),
                new BankModule(core, bankCore, new BankModuleSettings(new List<ulong> {272446177163608066})),
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
                }),
                new MemeGeneratorModule(core, new MemeGeneratorModuleSettings{MemePhrases = File.ReadAllLines("Resources/memes.txt")})
            };

            return new Bot(core, modules, settings.Token);
        }
    }
}