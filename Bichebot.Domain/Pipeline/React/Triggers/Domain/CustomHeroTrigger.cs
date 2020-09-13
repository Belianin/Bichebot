using System.Collections.Generic;
using Bichebot.Core;
using Bichebot.Core.Utilities;
using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers.Domain
{
    public class CustomHeroTrigger : ReactionTrigger
    {
        // todo вынести смайлы в константы
        private readonly IBotCore core;
        
        public CustomHeroTrigger(IBotCore core) : base(new []
        {
            new [] {"кх", "прогре", "грей", "грею", "грев"}
        }, 2)
        {
            this.core = core;
        }

        public override ReactionReply GetReply(IMessage message)
        {
            var replies = new[]
            {
                $"Запускаем токсик спайдера, работяги {core.ToEmojiString("mujikbamboe")}",
                $"Лучше бы в фолач... {core.ToEmojiString("slivbamboe")}",
                $"Главное, я прошу, НАЖМИ W {core.ToEmojiString("nesspride3")}{core.ToEmojiString("nesspride3")}",
                $"ХЭКА ХАААВНООО {core.ToEmojiString("badoobamboe")}",
                $"Я бы тож посмотрел, но только если Несс будет {core.ToEmojiString("oldbamboe")}",
                $"Желаю вам приятной игры и балдежных скиллов, друзья {core.ToEmojiString("dobrobamboe")}"
            };

            var text = core.Random.Choose(replies);
            return new ReactionReply(text);
        }
    }
}