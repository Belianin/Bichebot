using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Bichebot.Core.Pipeline
{
    public class MessagePipeline : IMessagePipeline
    {
        private readonly IReadOnlyCollection<IMessageHandler> handlers;

        public MessagePipeline(IReadOnlyCollection<IMessageHandler> handlers)
        {
            this.handlers = handlers;
        }

        public async Task HandleAsync(SocketMessage message)
        {
            foreach (var handler in handlers)
            {
                try
                {
                    var handleResult = await handler.HandleAsync(message).ConfigureAwait(false);
                    if (handleResult)
                        break;
                }
                catch (Exception e)
                {
                    await message.Channel.SendMessageAsync($"Мужики, помогите: {e.Message}").ConfigureAwait(false);
                }
            }
        }
    }
}