using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Bichebot.Modules.React.Triggers;
using Discord;
using Discord.WebSocket;
using Nexus.Logging;

namespace Bichebot.Modules.React
{
    public class ReactModule : BaseModule
    {
        private readonly ReactModuleSettings settings;
        private readonly ILog log;
        public ReactModule(ReactModuleSettings settings, IBotCore core, ILog log) : base(core)
        {
            this.settings = settings;
            this.log = log.ForContext(GetType().Name);
        }

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (!(message is IUserMessage userMessage) || !settings.ReactChannels.Contains(message.Channel.Id))
                return;
            
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
        }
    }
}