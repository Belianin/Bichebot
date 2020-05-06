using Bichebot.Core;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.React.Triggers.Domain
{
    public class LeagueOfLegendsTrigger : ReactionTrigger
    {
        private readonly IBotCore core;
        
        public LeagueOfLegendsTrigger(IBotCore core) : base(
            new []
            {
                new [] {"лол"}
            })
        {
            this.core = core;
        }

        public override ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply(core.Random.Choose(new []
            {
                $"{message.Author.Username}, {core.ToEmojiString("dobrobamboe")} может лучше в {core.ToEmojiString("supremebamboe")}?",
                $"{message.Author.Username}, {core.ToEmojiString("dobrobamboe")} ты хотел сказать {core.ToEmojiString("supremebamboe")}",
                $"{core.ToEmojiString("valera")} ну лан",
                $"{message.Author.Username}, а я за блицкранка куушкой не попадаю {core.ToEmojiString("liquidbamboe")}А?{core.ToEmojiString("liquidbamboe")}А?",
            }));
        }
    }
}