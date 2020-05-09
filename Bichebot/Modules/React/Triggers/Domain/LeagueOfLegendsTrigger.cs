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
                new [] {"лол", "lol", "лол,", "lol,", "лол.", "lol."}
            })
        {
            this.core = core;
        }

        public override ReactionReply GetReply(IMessage message)
        {
            return new ReactionReply(core.Random.Choose(new[]
            {
                $"{message.Author.Username}, 361-82-44 недорогой вывоз мусора. Запиши, пригодится {core.ToEmojiString("dobrobamboe")}",
                $"Я тоже искренне рад этому, {message.Author.Username}! Наконец-то люди начинают обращаться к истинному Богу!{core.ToEmojiString("nivgbamboe")}",
                $"Люблю я вонь горячей плазмы поутру!{core.ToEmojiString("deadinside")}",
                $"Похоже {message.Author.Username} вы обречены играть в доту вечно{core.ToEmojiString("oldigorbamboe")}",
                $"{core.ToEmojiString("lol")}Вероятность победить в следующей катке: 0%{core.ToEmojiString("dota")}",
                $"{core.ToEmojiString("lol")}Вероятность победить в следующей катке: 100%{core.ToEmojiString("dota")}",
                $"{core.ToEmojiString("lol")}Вероятность победить в следующей катке: 50%{core.ToEmojiString("dota")}",
                $"Кто-то считает {message.Author.Username} мусором, но для меня он сокровище{core.ToEmojiString("dobrobamboe")}",
                $"го ластецкую?{core.ToEmojiString("likebamboe")}{core.ToEmojiString("thumbsup_tone1")}",
                $"{message.Author.Username}, го ластецкую?{core.ToEmojiString("roflanbamboe")}",
                $"Че, {message.Author.Username}, го ластецкую?{core.ToEmojiString("roflanbamboe")}",
                $"{message.Author.Username}, абсолютно согласен с тобой!{core.ToEmojiString("olddobrobamboe")}",
                $"{message.Author.Username}, поддерживаю!{core.ToEmojiString("coolstory")}",
                $"Ещё одна жертва дедлайна...",
                $"ПРЕБАФКА К ПЕНСИИ))){core.ToEmojiString("alohabamboe")}",
                $"{message.Author.Username} советую играть в личке чтобы не лагало!{core.ToEmojiString("kislenkobamboe")}{core.ToEmojiString("thumbsup_tone1")}",
                $"Приветствую, {message.Author.Username}. Отчего земля у меня под ногами потеплела при твоем приближении?{core.ToEmojiString("imbabamboe")}",
                $"{message.Author.Username}, {core.ToEmojiString("dobrobamboe")} ты хотел сказать {core.ToEmojiString("supremebamboe")}",
                $"{core.ToEmojiString("valera")} ну лан",
                $"Святые небеса, ты хоть понимаешь{message.Author.Username}, что говоришь? Ты правда в это веришь?",
                $"Берешь гидравлический фазоинвертор, подключаешь его к циклическому прото-излучателю, потом врубаешь на полную...",
                $"Три икс в кубе плюс константа.. . Ну что там?",
                $"{message.Author.Username} переходим к делу только быстро",
                $"{message.Author.Username} умри, насекомое!",
                $"{message.Author.Username} Не. Хочу. Играть. В говно.",
                $"{message.Author.Username} я бы поиграл с вами, но я просто бот...",
                $"{message.Author.Username} Теперь я вижу всю иронию...",
                $"Всё очень просто. {message.Author.Username} удали Доту.",
                $"Ты спятил",
                $"Лучше бы вступил в Nexus Omega Corp. :evebamboe:",
                $"Стой, куда ты {message.Author.Username}? Ну ты и соня. Как тебя зовут?",
                $"{message.Author.Username}, а я за блицкранка куушкой не попадаю {core.ToEmojiString("liquidbamboe")}А?{core.ToEmojiString("liquidbamboe")}А?",
            }));
        }
    }
}