using Bichebot.Modules.React;
using Bichebot.Modules.React.Triggers;
using Bichebot.Modules.React.Triggers.Domain;

namespace Bichebot
{
    public static class BotConfigurationBuilderExtensions
    {
        public static IBotConfigurationBuilder UseReactModule(this IBotConfigurationBuilder builder)
        {
            return builder.Use<ReactModule>(core => new ReactModuleSettings()
            {
                Triggers = new IReactionTrigger[]
                {
                    new ContentTrigger(core),
                    new HelloTrigger(core),
                    new LeagueOfLegendsTrigger(core),
                    new LikeTrigger(core),
                    new QuestionTrigger(core),
                    new RareTrigger(core),
                    new RudeTrigger(core),
                }
            });
        }
    }
}