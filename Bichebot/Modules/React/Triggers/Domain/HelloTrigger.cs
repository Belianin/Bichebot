using Bichebot.Core;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.React.Triggers.Domain
{
    public class HelloTrigger : ReactionTrigger
    {
        private readonly IBotCore core;
        
        public HelloTrigger(IBotCore core) : base(new []
        {
            new [] {"бот ", "бот,", " бот"},
            new [] {"привет", "здаров", " хай ", "доброе утро", "добрый вечер", "добрый день"}
        })
        {
            this.core = core;
        }

        public override ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply(core.Random.Choose(
                $"{message.Author.Username}, здарова Бро {core.ToEmojiString("dobrobamboe")}",
                $"Привет {message.Author.Username}", $"{message.Author.Username}, я приветсвую тебя",
                $"{message.Author.Username}, добро пожаловать в Бухту Бичехостов",
                $"{message.Author.Username}, как дела ?",
                $"{message.Author.Username}, добро пожаловать. Добро пожаловать в Бухту Бичехостов. Сами вы её выбрали или её выбрали за вас, это лучшее место из оставшихся."));

        }
    }
}