using Bichebot.Core;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.React.Triggers.Domain
{
    public class LikeTrigger : ReactionTrigger
    {
        private readonly IBotCore core;
        public LikeTrigger(IBotCore core) : base(new []
        {
            new [] {"бот"},
            new [] {"красавчик", "молор", "найс", "дядя", "мужик", "васян", "гунирал", "ля", "какой"}
        })
        {
            this.core = core;
        }

        public override ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply(core.GetEmote(
                core.Random.Choose("valera", "oldbamboe", "kadikbamboe", "dobrobamboe", "lejatbabmoe")));
        }
    }
}