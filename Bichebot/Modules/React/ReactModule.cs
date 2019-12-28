using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Bichebot.Utilities;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Modules.React
{
    public class ReactModule : BaseModule
    {
        public ReactModule(IBotCore core) : base(core) {}

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (!(message is IUserMessage userMessage))
                return;

            if (IsDeservingLike(message, out var reaction))
                await userMessage.AddReactionAsync(Core.Guild.Emotes.First(n => n.Name == reaction))
                    .ConfigureAwait(false);

            if (Core.Random.Roll(10))
                await userMessage.AddReactionAsync(Core.Random.Choose(Core.Guild.Emotes))
                    .ConfigureAwait(false);

            if (message.Content.Contains("лол"))
            {
                await message.Channel.SendMessageAsync(Core.Random.Choose(new []
                    {
                        $"{message.Author.Username} {Core.ToEmojiString("dobrobamboe")} может лучше в {Core.ToEmojiString("supremebamboe")}?",
                        $"{message.Author.Username} {Core.ToEmojiString("dobrobamboe")} ты хотел сказать {Core.ToEmojiString("supremebamboe")}",
                        $"{Core.ToEmojiString("valera")} ну лан",
                        $"{message.Author.Username} а я за блицкранка куушкой не попадаю {Core.ToEmojiString("liquidbamboe")}А?{Core.ToEmojiString("liquidbamboe")}А?",
                    }))
                    .ConfigureAwait(false);
            }
            else if (IsBotHello(message))
            {
                await message.Channel.SendMessageAsync(Core.Random.Choose(new []
                    {
                        $"{message.Author.Username} Здарова Бро {Core.ToEmojiString("dobrobamboe")}",
                        $"Привет {message.Author.Username}",
                        $"{message.Author.Username}, я приветсвую тебя",
                        $"{message.Author.Username} Добро пожаловать в Бухту Бичехостов",
                        $"{message.Author.Username} Как дела ?",
                        $"{message.Author.Username} Добро пожаловать. Добро пожаловать в Бухту Бичехостов. Сами вы её выбрали или её выбрали за вас, это лучшее место из оставшихся."
                    }))
                    .ConfigureAwait(false);
            }
            else if (message.Content.Contains("бот лох"))
            {
                await message.Channel.SendMessageAsync(Core.Random.Choose(new[]
                    {
                        $"{message.Author.Username} Удали",
                        $"{message.Author.Username} Ты в муте"
                    }))
                    .ConfigureAwait(false);
            }
            else if (Core.Random.Roll(1000))
                await message.Channel.SendMessageAsync("Скатился...").ConfigureAwait(false);
        }
        
        private bool IsDeservingLike(IMessage message, out string emoji)
        {
            var lower = message.Content.ToLower();
            var goodWords = new[] {"красавчик", "молор", "найс", "дядя", "мужик", "васян", "гунирал", "ля", "какой"};
            var emojies = new []
            {
                "valera", "oldbamboe", "kadikbamboe", "dobrobamboe", "lejatbabmoe"
            };
            if (lower.Contains("бот") && !lower.Contains("не") && lower.ContainsAny(goodWords))
            {
                emoji = Core.Random.Choose(emojies);
                return true;
            }

            emoji = null;
            return false;
        }
        
        private static bool IsBotHello(IMessage message)
        {
            var lower = message.Content.ToLower();
            return lower.ContainsAny(new [] {"бот ", "бот,", " бот"}) &&
                   lower.ContainsAny(new[] {"привет", "здаров", " хай ", "доброе утро", "добрый вечер", "добрый день"});
        }
    }
}