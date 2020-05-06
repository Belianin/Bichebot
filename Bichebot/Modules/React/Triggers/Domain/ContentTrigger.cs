using Bichebot.Core;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.React.Triggers.Domain
{
    public class ContentTrigger : IReactionTrigger
    {
        private readonly IBotCore core;

        public ContentTrigger(IBotCore core)
        {
            this.core = core;
        }

        public bool IsNeedReaction(IMessage message)
        {
            if (message.Attachments.Count > 0 && message.Content.Length >= 100)
                return true;

            return core.Random.Roll(message.Content.Length, 100);
        }

        public ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply(core.Random.Choose(core.Guild.Emotes));
        }
    }
}