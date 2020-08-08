using System;
using System.Linq;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Audio;
using Bichebot.Repositories;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Logging;

namespace Bichebot
{
    public class BotConfigurationBuilder : IBotConfigurationBuilder
    {
        private readonly IServiceCollection serviceCollection = new ServiceCollection();
        private readonly BotSettings botSettings;
        private readonly ILog log;

        public BotConfigurationBuilder(BotSettings botSettings, ILog log)
        {
            this.botSettings = botSettings;
            this.log = log;

            serviceCollection.AddSingleton<ILog>(log);
            serviceCollection.AddSingleton<DiscordSocketClient>();
            serviceCollection.AddSingleton<IBotCore>(sp => 
                new BotCore(botSettings.GuildId, sp.GetService<DiscordSocketClient>()));
            serviceCollection.AddSingleton<IBankCore>(new BankCore(
                new FileRepository<ulong, Bicheman>("Bichemans", ulong.Parse)));
            serviceCollection.AddSingleton<AudioSpeaker>();
        }

        public IBotConfigurationBuilder Use<TModule>() where TModule : class, IBotModule
        {
            serviceCollection.AddSingleton<IBotModule, TModule>();

            return this;
        }

        public IBotConfigurationBuilder Use<TModule, TSettings>(TSettings settings) where TModule : class, IBotModule where TSettings : class
        {
            serviceCollection.AddSingleton<TSettings>(settings);

            return Use<TModule>();
        }

        public IBotConfigurationBuilder Use<TModule, TSettings>(Func<IServiceProvider, TSettings> settings) where TModule : class, IBotModule where TSettings : class
        {
            serviceCollection.AddSingleton<TSettings>(settings);

            return Use<TModule>();
        }

        public IBotConfigurationBuilder Use<TModule, TSettings>(Func<IBotCore, TSettings> settings) where TModule : class, IBotModule where TSettings : class
        {
            serviceCollection.AddSingleton<TSettings>(sp => settings(sp.GetRequiredService<IBotCore>()));

            return Use<TModule>();
        }

        public Bot Build()
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var core = serviceProvider.GetRequiredService<IBotCore>();

            var a = serviceProvider.GetServices<IBotModule>().ToArray();
            
            return new Bot(
                serviceProvider.GetRequiredService<IBotCore>(),
                serviceProvider.GetServices<IBotModule>().ToArray(),
                botSettings.Token,
                log);
        }
    }

    public interface IBotConfigurationBuilder
    {
        IBotConfigurationBuilder Use<TModule>() where TModule : class, IBotModule;

        IBotConfigurationBuilder Use<TModule, TSettings>(TSettings settings) where TModule : class, IBotModule where TSettings : class;

        IBotConfigurationBuilder Use<TModule, TSettings>(Func<IServiceProvider, TSettings> settings) where TModule : class, IBotModule where TSettings : class;

        IBotConfigurationBuilder Use<TModule, TSettings>(Func<IBotCore, TSettings> settings) where TModule : class, IBotModule where TSettings : class;

        Bot Build();
    }
}