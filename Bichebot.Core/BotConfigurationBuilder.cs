using System;
using System.Linq;
using Bichebot.Core.Audio;
using Bichebot.Core.Banking;
using Bichebot.Core.Modules;
using Bichebot.Core.Pipeline;
using Bichebot.Core.Repositories;
using Bichebot.Core.Users;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Logging;

namespace Bichebot.Core
{
    public class BotConfigurationBuilder : IBotConfigurationBuilder
    {
        private readonly IServiceCollection serviceCollection = new ServiceCollection();

        public BotConfigurationBuilder(ulong guildId, ILog log)
        {
            var bank = new BankCore(new FileUserRepository("Bichemans"));
            var core = new BotCore(guildId, new DiscordSocketClient(), bank, log);
            
            serviceCollection.AddSingleton<IBotCore>(core);
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

        public IBotConfigurationBuilder ConfigurePipeline(Action<IPipelineBuilder> configure)
        {
            serviceCollection.AddSingleton<IMessagePipeline>(x =>
            {
                var builder = new PipelineBuilder(x.GetRequiredService<IBotCore>());

                configure(builder);

                return builder.Build();
            });

            return this;
        }
        
        public Bot Build()
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            return new Bot(
                serviceProvider.GetRequiredService<IBotCore>(),
                serviceProvider.GetServices<IBotModule>().ToArray(),
                serviceProvider.GetService<IMessagePipeline>());
        }
    }
}