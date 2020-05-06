using System.Collections.Generic;
using System.Linq;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.React.Triggers
{
    public abstract class ReactionTrigger : IReactionTrigger
    {
        private readonly ICollection<ICollection<string>> conditions;

        protected ReactionTrigger(ICollection<ICollection<string>> conditions)
        {
            this.conditions = conditions;
        }
        
        public virtual bool IsNeedReaction(IMessage message)
        {
            return conditions.All(c => message.Content.ContainsAny(c));
        }

        public abstract ReactionReply GetReply(IMessage message);
    }
}