using System.Threading.Tasks;
using Bichebot.Core;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Modules.Base
{
    public abstract class BaseModule : IBotModule
    {
        protected readonly IBotCore Core;

        protected BaseModule(IBotCore core)
        {
            Core = core;
        }

        protected virtual async Task HandleMessageAsync(SocketMessage message) {}

        protected virtual async Task HandleReactionAsync(
            Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel channel,
            SocketReaction reaction) {}

        public bool IsRunning { get; private set; }
        
        public void Run()
        {
            if (IsRunning)
                return;
            
            Core.Client.MessageReceived += HandleMessageAsync;
            Core.Client.ReactionAdded += HandleReactionAsync;
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;
            
            Core.Client.MessageReceived -= HandleMessageAsync;
            Core.Client.ReactionAdded -= HandleReactionAsync;
            IsRunning = false;
        }
    }
}