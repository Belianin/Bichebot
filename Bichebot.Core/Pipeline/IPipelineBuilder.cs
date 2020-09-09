using System;

namespace Bichebot.Core.Pipeline
{
    public interface IPipelineBuilder
    {
        IPipelineBuilder Use<THandler>() where THandler : class, IMessageHandler;
        
        IPipelineBuilder Use<THandler, TSettings>(TSettings settings) where THandler : class, IMessageHandler where TSettings : class;
        
        IPipelineBuilder Use<THandler, TSettings>(Func<IServiceProvider, TSettings> settings) where THandler : class, IMessageHandler where TSettings : class;

        IPipelineBuilder Use<THandler, TSettings>(Func<IBotCore, TSettings> settings) where THandler : class, IMessageHandler where TSettings : class;
    }
}