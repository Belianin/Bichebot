using Bichebot.Core;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.React.Triggers.Domain
{
    public class RareTrigger : IReactionTrigger
    {
        private readonly IBotCore core;

        public RareTrigger(IBotCore core)
        {
            this.core = core;
        }

        public bool IsNeedReaction(IMessage message)
        {
            return core.Random.Roll(1000);
        }

        public ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply("Скатился...");
        }
    }
}