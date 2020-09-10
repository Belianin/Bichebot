using System;
using System.Linq;
using Bichebot.Core.Audio;
using Bichebot.Core.Banking;
using Bichebot.Core.Modules;
using Bichebot.Core.Pipeline;
using Bichebot.Core.Users;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Logging;

namespace Bichebot.Core
{
    public class BotConfigurationBuilder : IBotConfigurationBuilder
    {
        private readonly IServiceCollection services = new ServiceCollection();

        public BotConfigurationBuilder(ulong guildId, ILog log)
        {
            var bank = new BankCore(new FileUserRepository("Bichemans"));
            var core = new BotCore(guildId, new DiscordSocketClient(), bank, log);

            services.AddSingleton<IBotCore>(core);
            services.AddSingleton<ILog>(log); // временно?
            services.AddSingleton<AudioSpeaker>();
        }

        public IBotConfigurationBuilder Use<TModule>() where TModule : class, IBotModule
        {
            services.AddSingleton<IBotModule, TModule>();

            return this;
        }

        public IBotConfigurationBuilder Use<TModule, TSettings>(TSettings settings)
            where TModule : class, IBotModule where TSettings : class
        {
            services.AddSingleton(settings);

            return Use<TModule>();
        }

        public IBotConfigurationBuilder Use<TModule, TSettings>(Func<IServiceProvider, TSettings> settings)
            where TModule : class, IBotModule where TSettings : class
        {
            services.AddSingleton(settings);

            return Use<TModule>();
        }

        public IBotConfigurationBuilder Use<TModule, TSettings>(Func<IBotCore, TSettings> settings)
            where TModule : class, IBotModule where TSettings : class
        {
            services.AddSingleton(sp => settings(sp.GetRequiredService<IBotCore>()));

            return Use<TModule>();
        }

        public IBotConfigurationBuilder ConfigurePipeline(Action<IPipelineBuilder> configure)
        {
            services.AddSingleton(x =>
            {
                var builder = new PipelineBuilder(x.GetRequiredService<IBotCore>());

                configure(builder);

                return builder.Build();
            });

            return this;
        }

        public Bot Build()
        {
            var serviceProvider = services.BuildServiceProvider();

            return new Bot(
                serviceProvider.GetRequiredService<IBotCore>(),
                serviceProvider.GetServices<IBotModule>().ToArray(),
                serviceProvider.GetService<IMessagePipeline>());
        }
    }
}