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

            if (Core.Random.IsSuccess(10))
                await userMessage.AddReactionAsync(Core.Random.GetRandomReadonly(Core.Guild.Emotes))
                    .ConfigureAwait(false);

            if (message.Content.Contains("лол"))
            {
                if (Core.Random.IsSuccess(2))
                {
                    await message.Channel.SendMessageAsync(
                            $"{Core.ToEmojiString("dobrobamboe")} может лучше в {Core.ToEmojiString("supremebamboe")}?")
                        .ConfigureAwait(false);
                }
                else if (Core.Random.IsSuccess(2))
                {
                    await message.Channel.SendMessageAsync(
                            $"{Core.ToEmojiString("dobrobamboe")} ты хотел сказать {Core.ToEmojiString("supremebamboe")}?")
                        .ConfigureAwait(false);
                }
                else
                {
                    await message.Channel.SendMessageAsync(
                            $"{Core.ToEmojiString("valera")} ну лан")
                        .ConfigureAwait(false);
                }
            }
            else if (Core.Random.IsSuccess(1000))
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
                emoji = Core.Random.GetRandom(emojies);
                return true;
            }

            emoji = null;
            return false;
        }
    }
}