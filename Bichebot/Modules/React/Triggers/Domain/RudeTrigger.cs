using Bichebot.Core;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.React.Triggers.Domain
{
    public class RudeTrigger : ReactionTrigger
    {
        private readonly IBotCore core;
        
        public RudeTrigger(IBotCore core) : base(new []
        {
            new [] {"бот лох"}
        })
        {
            this.core = core;
        }

        public override ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply(core.Random.Choose(new[]
            {
                $"{message.Author.Username}, удали",
                $"{message.Author.Username}, ты в муте"
            }));
        }
    }
}