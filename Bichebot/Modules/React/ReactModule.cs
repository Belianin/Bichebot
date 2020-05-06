using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Bichebot.Modules.React.Triggers;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Modules.React
{
    public class ReactModule : BaseModule
    {
        private readonly ReactModuleSettings settings;
        public ReactModule(IBotCore core, ReactModuleSettings settings) : base(core)
        {
            this.settings = settings;
        }

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (!(message is IUserMessage userMessage) || !settings.ReactChannels.Contains(message.Channel.Id))
                return;
            
            foreach (var trigger in settings.Triggers)
            {
                if (!trigger.TryGetReaction(message, out var reaction))
                    continue;
                
                if (reaction.Text != null)
                    await message.Channel.SendMessageAsync(reaction.Text).ConfigureAwait(false);
                if (reaction.Emotes != null && reaction.Emotes.Count > 0)
                    await userMessage.AddReactionsAsync(reaction.Emotes.ToArray()).ConfigureAwait(false);
            }
        }
    }
}