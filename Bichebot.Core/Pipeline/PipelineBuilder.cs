using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Bichebot.Core.Pipeline
{
    public class PipelineBuilder : IPipelineBuilder
    {
        private readonly IServiceCollection services = new ServiceCollection();

        public PipelineBuilder(IBotCore core)
        {
            services.AddSingleton(core);
            services.AddSingleton(core.Log);
        }

        public IPipelineBuilder Use<THandler>() where THandler : class, IMessageHandler
        {
            services.AddSingleton<IMessageHandler, THandler>();
            return this;
        }

        public IPipelineBuilder Use<THandler, TSettings>(TSettings settings) where THandler : class, IMessageHandler
            where TSettings : class
        {
            services.AddSingleton<TSettings>(settings);

            return Use<THandler>();
        }

        public IPipelineBuilder Use<THandler, TSettings>(Func<IServiceProvider, TSettings> settings)
            where THandler : class, IMessageHandler where TSettings : class
        {
            services.AddSingleton<TSettings>(settings);

            return Use<THandler>();
        }

        public IPipelineBuilder Use<THandler, TSettings>(Func<IBotCore, TSettings> settings)
            where THandler : class, IMessageHandler where TSettings : class
        {
            services.AddSingleton<TSettings>(sp => settings(sp.GetService<IBotCore>()));

            return Use<THandler>();
        }

        public IMessagePipeline Build()
        {
            var provider = services.BuildServiceProvider();
            
            return new MessagePipeline(provider.GetServices<IMessageHandler>().ToArray());
        }
    }
}