using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers
{
    public static class ReactionTriggerExtensions
    {
        public static bool TryGetReaction(this IReactionTrigger trigger, IMessage message, out ReactionReply reply)
        {
            if (trigger.IsNeedReaction(message))
            {
                reply = trigger.GetReply(message);
                return true;
            }

            reply = null;
            return false;
        }
    }
}