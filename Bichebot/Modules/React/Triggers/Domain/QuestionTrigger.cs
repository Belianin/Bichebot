using Bichebot.Core;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.React.Triggers.Domain
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