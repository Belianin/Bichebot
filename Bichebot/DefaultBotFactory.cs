using System.IO;
using Bichebot.Modules.Audio;
using Bichebot.Modules.Bank;
using Bichebot.Modules.Best;
using Bichebot.Modules.Greeter;
using Bichebot.Modules.MemeGenerator;
using Bichebot.Modules.Moderate;
using Bichebot.Modules.Statistics;
using Bichebot.Modules.Supreme;

namespace Bichebot
{
    internal class DefaultBotFactory : IBotFactory
    {
        public Bot Create(BotSettings settings)
        {
            return new BotConfigurationBuilder(settings)
                .Use<MemeGeneratorModule>(new MemeGeneratorModuleSettings{MemePhrases = File.ReadAllLines("Resources/memes.txt")})
                .Use<StatisticsModule>()
                .UseReactModule()
                .Use<AudioModule>()
                .Use<SupremeModule>()
                .Use<ModerateModule>()
                .Use<BankModule>(settings.BankModule)
                .Use<BestModule>(settings.BestModule)
                .Use<GreeterModule>(settings.GreeterModule)
                .Build();
        }
    }
}