using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Core.Modules.Base;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Domain.Modules
{
    public class TipModule : BaseModule
    {
        public TipModule(IBotCore core) : base(core)
        {
        }

        protected override async Task HandleReactionAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.Emote.Name == "tip")
            {
                var message = reaction.Message.IsSpecified 
                    ? reaction.Message.Value
                    : await reaction.Channel.GetMessageAsync(reaction.MessageId).ConfigureAwait(false);
                var user = reaction.User.IsSpecified
                    ? reaction.User.Value
                    : await reaction.Channel.GetUserAsync(reaction.UserId).ConfigureAwait(false);

                if (reaction.UserId == message.Author.Id)
                    return;
                
                var result = Core.Bank.TryTransact(reaction.UserId, message.Author.Id, 10);
                if (result.IsSuccess)
                {
                    var t = result.Value;
                    await message.Channel
                        .SendMessageAsync(
                            $"{user.Username} {t.FromBalance} -> 10 -> {t.ToBalance} {message.Author.Username}")
                        .ConfigureAwait(false);
                }
            }
        }
    }
}