using System.Collections.Generic;
using System.Linq;
using Bichebot.Core.Utilities;
using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers
{
    public abstract class ReactionTrigger : IReactionTrigger
    {
        private readonly ICollection<ICollection<string>> conditions;

        protected ReactionTrigger(ICollection<ICollection<string>> conditions)
        {
            this.conditions = conditions
                .Select(c => c.Select(x => x.ToLower()).ToArray())
                .ToArray();
        }
        
        public virtual bool IsNeedReaction(IMessage message)
        {
            var content = message.Content.ToLower();
            
            return conditions.All(c => content.ContainsAny(c));
        }

        public abstract ReactionReply GetReply(IMessage message);
    }
}