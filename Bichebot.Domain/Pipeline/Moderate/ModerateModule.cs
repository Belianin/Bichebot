using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core.Pipeline;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Domain.Pipeline.Moderate
{
    public class ModerateModule : IMessageHandler
    {
        public async Task<bool> HandleAsync(SocketMessage message)
        {
            if (message.Content.Contains("бот") && message.Content.Contains("удали"))
            {
                await DeletePreviousMessageAsync(message).ConfigureAwait(false);
                return true;
            }

            return false;
        }

        private static async Task DeletePreviousMessageAsync(SocketMessage message)
        {
            var messages = await message.Channel.GetMessagesAsync(message, Direction.Before, 1).FlattenAsync()
                .ConfigureAwait(false);

            var messageToDelete = messages.FirstOrDefault();
            
            if (messageToDelete is IUserMessage userMessage && !userMessage.Content.Contains("~~"))
            {
                await userMessage.DeleteAsync().ConfigureAwait(false);
                await message.Channel.SendMessageAsync($"||~~{userMessage.Content}~~||")
                    .ConfigureAwait(false);
            }
        }
    }
}