using Discord;

namespace Bichebot.Modules.React.Triggers
{
    public interface IReactionTrigger
    {
        bool IsNeedReaction(IMessage message);
        ReactionReply GetReply(IMessage message);
    }
}