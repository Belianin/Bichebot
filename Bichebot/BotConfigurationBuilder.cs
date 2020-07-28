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
            serviceCollection.AddSingleton<IBankCore>(
                new BankCore(new BichemansRepository()));
            serviceCollection.AddSingleton<AudioSpeaker>();
        }

        public IBotConfigurationBuilder Use<TModule>() where TModule : IBotModule
        {
            serviceCollection.Add(new ServiceDescriptor(typeof(IBotModule), typeof(TModule)));

            return this;
        }

        public IBotConfigurationBuilder Use<TModule>(object settings) where TModule : IBotModule
        {
            serviceCollection.AddSingleton(settings);

            return Use<TModule>();
        }

        public IBotConfigurationBuilder Use<TModule>(Func<IServiceProvider, object> settings) where TModule : IBotModule
        {
            serviceCollection.AddSingleton(settings);

            return Use<TModule>();
        }

        public IBotConfigurationBuilder Use<TModule>(Func<IBotCore, object> settings) where TModule : IBotModule
        {
            serviceCollection.AddSingleton(sp => settings(sp.GetRequiredService<IBotCore>()));

            return Use<TModule>();
        }

        public Bot Build()
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            return new Bot(
                serviceProvider.GetRequiredService<IBotCore>(),
                serviceProvider.GetServices<IBotModule>().ToArray(),
                botSettings.Token,
                log);
        }
    }

    public interface IBotConfigurationBuilder
    {
        IBotConfigurationBuilder Use<TModule>() where TModule : IBotModule;

        IBotConfigurationBuilder Use<TModule>(object settings) where TModule : IBotModule;

        IBotConfigurationBuilder Use<TModule>(Func<IServiceProvider, object> settings) where TModule : IBotModule;

        IBotConfigurationBuilder Use<TModule>(Func<IBotCore, object> settings) where TModule : IBotModule;

        Bot Build();
    }
}