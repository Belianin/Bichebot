using System;
using Bichebot.Core.Modules;
using Bichebot.Core.Pipeline;

namespace Bichebot.Core
{
    public interface IBotConfigurationBuilder
    {
        IBotConfigurationBuilder Use<TModule>() where TModule : class, IBotModule;

        IBotConfigurationBuilder Use<TModule, TSettings>(TSettings settings) where TModule : class, IBotModule where TSettings : class;

        IBotConfigurationBuilder Use<TModule, TSettings>(Func<IServiceProvider, TSettings> settings) where TModule : class, IBotModule where TSettings : class;

        IBotConfigurationBuilder Use<TModule, TSettings>(Func<IBotCore, TSettings> settings) where TModule : class, IBotModule where TSettings : class;

        IBotConfigurationBuilder ConfigurePipeline(Action<IPipelineBuilder> configure);

        Bot Build();
    }
}