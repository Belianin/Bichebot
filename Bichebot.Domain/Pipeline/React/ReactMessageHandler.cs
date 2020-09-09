using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core.Pipeline;
using Bichebot.Domain.Pipeline.React.Triggers;
using Discord;
using Discord.WebSocket;
using Nexus.Logging;

namespace Bichebot.Domain.Pipeline.React
{
    public class ReactMessageHandler : IMessageHandler
    {
        private readonly ReactSettings settings;
        private readonly ILog log;
        public ReactMessageHandler(ReactSettings settings, ILog log)
        {
            this.settings = settings;
            this.log = log.ForContext(GetType().Name);
        }

        public async Task<bool> HandleAsync(SocketMessage message)
        {
            if (!(message is IUserMessage userMessage) || !settings.ReactChannels.Contains(message.Channel.Id))
                return false;
            
            foreach (var trigger in settings.Triggers)
            {
                if (!trigger.TryGetReaction(message, out var reaction))
                    continue;
                log.Info($"Reacting with {reaction} by {trigger.GetType().Name} on '{message.Content ?? "attachment"}'");
                
                if (reaction.Text != null)
                    await message.Channel.SendMessageAsync(reaction.Text).ConfigureAwait(false);
                if (reaction.Emotes != null && reaction.Emotes.Count > 0)
                    await userMessage.AddReactionsAsync(reaction.Emotes.ToArray()).ConfigureAwait(false);
            }

            return true;
        }
    }
}