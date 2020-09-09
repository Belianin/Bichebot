using Bichebot.Core;
using Bichebot.Core.Utilities;
using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers.Domain
{
    public class RudeTrigger : ReactionTrigger
    {
        private readonly IBotCore core;
        
        public RudeTrigger(IBotCore core) : base(new []
        {
            new [] {"бот"},
            new [] {"лох", "деб", "твар"} 
        })
        {
            this.core = core;
        }

        public override ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply(core.Random.Choose(
                $"{message.Author.Username}, удали",
                $"{message.Author.Username}, ты в муте"));
        }
    }
}