using System;
using System.Collections.Generic;
using System.Linq;
using Bichebot.Core.Utilities;
using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers
{
    public abstract class ReactionTrigger : IReactionTrigger
    {
        private readonly ICollection<ICollection<string>> conditions;
        private readonly int chance;
        private readonly Random random;

        protected ReactionTrigger(ICollection<ICollection<string>> conditions, int chance = 1)
        {
            random = new Random();
            this.chance = chance;
            this.conditions = conditions
                .Select(c => c.Select(x => x.ToLower()).ToArray())
                .ToArray();
        }

        public virtual bool IsNeedReaction(IMessage message)
        {
            if (!random.Roll(chance))
                return false;
            
            var content = message.Content.ToLower();

            return conditions.All(c => content.ContainsAny(c));
        }

        public abstract ReactionReply GetReply(IMessage message);
    }
}