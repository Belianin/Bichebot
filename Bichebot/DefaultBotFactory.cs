using System.IO;
using Bichebot.Modules.Audio;
using Bichebot.Modules.Bank;
using Bichebot.Modules.Best;
using Bichebot.Modules.FOnlineStatistics;
using Bichebot.Modules.Greeter;
using Bichebot.Modules.MemeGenerator;
using Bichebot.Modules.Moderate;
using Bichebot.Modules.Statistics;
using Bichebot.Modules.Supreme;
using Bichebot.Modules.Withermans;
using Nexus.Logging.Console;

namespace Bichebot
{
    internal class DefaultBotFactory : IBotFactory
    {
        public Bot Create(BotSettings settings)
        {
            return new BotConfigurationBuilder(settings, new ColourConsoleLog())
                .Use<MemeGeneratorModule, MemeGeneratorModuleSettings>(new MemeGeneratorModuleSettings
                    {MemePhrases = File.ReadAllLines("Resources/memes.txt")})
                .Use<StatisticsModule>()
                .UseReactModule()
                .Use<AudioModule>()
                .Use<SupremeModule>()
                .Use<ModerateModule>()
                .Use<BankModule, BankModuleSettings>(settings.BankModule)
                .Use<BestModule, BestModuleSettings>(settings.BestModule)
                .Use<GreeterModule, GreeterModuleSettings>(settings.GreeterModule)
                .Use<WithermansModule, WithermansSettings>(settings.WithermansModule)
                .Use<FOnlineStatisticsModule, FonlineStatisticsModuleSettings>(settings.FonlineStatisticsModule)
                .Build();
        }
    }
}