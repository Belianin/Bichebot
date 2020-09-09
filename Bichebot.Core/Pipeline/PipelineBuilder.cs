using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bichebot.Core.Pipeline
{
    public class PipelineBuilder : IPipelineBuilder
    {
        private readonly IServiceCollection serviceCollection = new ServiceCollection();

        public PipelineBuilder(IBotCore core)
        {
            serviceCollection.AddSingleton<IBotCore>(core);
        }

        public IPipelineBuilder Use<THandler>() where THandler : class, IMessageHandler
        {
            serviceCollection.AddSingleton<IMessageHandler, THandler>();
            return this;
        }

        public IPipelineBuilder Use<THandler, TSettings>(TSettings settings) where THandler : class, IMessageHandler where TSettings : class
        {
            throw new NotImplementedException();
        }

        public IPipelineBuilder Use<THandler, TSettings>(Func<IServiceProvider, TSettings> settings) where THandler : class, IMessageHandler where TSettings : class
        {
            throw new NotImplementedException();
        }

        public IPipelineBuilder Use<THandler, TSettings>(Func<IBotCore, TSettings> settings) where THandler : class, IMessageHandler where TSettings : class
        {
            throw new NotImplementedException();
        }

        public IMessagePipeline Build()
        {
            throw new NotImplementedException();
        }
    }
}