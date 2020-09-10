using Bichebot.Core;
using Bichebot.Domain.Modules;
using Bichebot.Domain.Modules.Best;
using Bichebot.Domain.Modules.FOnlineStatistics;
using Bichebot.Domain.Pipeline.Bank;
using Bichebot.Domain.Pipeline.JumpGame;
using Bichebot.Domain.Pipeline.MemeGenerator;
using Bichebot.Domain.Pipeline.Moderate;
using Bichebot.Domain.Pipeline.React;
using Bichebot.Domain.Pipeline.React.Triggers;
using Bichebot.Domain.Pipeline.React.Triggers.Domain;
using Bichebot.Domain.Pipeline.Statistics;
using Bichebot.Domain.Pipeline.Supreme;
using Bichebot.Settings;
using Nexus.Logging.Console;

namespace Bichebot
{
    internal class BotFactory : IBotFactory
    {
        public static IBotFactory Instance => new BotFactory();

        public Bot Create(BotSettings settings)
        {
            return new BotConfigurationBuilder(settings.GuildId, new ColourConsoleLog())
                .Use<TipModule>()
                .Use<BestModule, BestModuleSettings>(settings.BestModule)
                .Use<FOnlineStatisticsModule, FonlineStatisticsModuleSettings>(settings.FonlineStatisticsModule)
                .ConfigurePipeline(x => x
                    .Use<MemeGeneratorMessageHandler, MemeGeneratorSettings>(settings.MemeGenerator)
                    .Use<JumpGameMessageHandler, WithermansSettings>(settings.WithermansModule)
                    .Use<BankMessageHandler, BankModuleSettings>(settings.BankModule)
                    .Use<ModerateModule>()
                    .Use<StatisticsModule>()
                    .Use<SupremeHandler>()
                    .Use<ReactMessageHandler, ReactSettings>(core => new ReactSettings
                    {
                        Triggers = new IReactionTrigger[]
                        {
                            new ContentTrigger(core),
                            new HelloTrigger(core),
                            new LeagueOfLegendsTrigger(core),
                            new LikeTrigger(core),
                            new QuestionTrigger(core),
                            new RareTrigger(core),
                            new RudeTrigger(core)
                        }
                    }))
                .Build();
        }
    }
}