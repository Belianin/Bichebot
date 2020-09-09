using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers
{
    public interface IReactionTrigger
    {
        bool IsNeedReaction(IMessage message);
        ReactionReply GetReply(IMessage message);
    }
}