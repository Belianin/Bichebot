using Bichebot.Core;
using Bichebot.Core.Utilities;
using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers.Domain
{
    public class QuestionTrigger : IReactionTrigger
    {
        private readonly IBotCore core;

        public QuestionTrigger(IBotCore core)
        {
            this.core = core;
        }

        public bool IsNeedReaction(IMessage message)
        {
            return message.Content.Contains("?") && core.Random.Roll(10);
        }

        public ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply(core.GetEmote(core.Random.Choose(new[] {"thinkingbamboe", "papich"})));
        }
    }
}