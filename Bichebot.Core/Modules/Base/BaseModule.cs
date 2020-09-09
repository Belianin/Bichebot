using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Core.Modules.Base
{
    public abstract class BaseModule : IBotModule
    {
        protected readonly IBotCore Core;

        protected BaseModule(IBotCore core)
        {
            Core = core;
        }

        public bool IsRunning { get; private set; }

        public virtual void Run()
        {
            if (IsRunning)
                return;

            Core.Client.MessageReceived += HandleMessageAsync;
            Core.Client.ReactionAdded += HandleReactionAsync;
            IsRunning = true;
        }

        public virtual void Stop()
        {
            if (!IsRunning)
                return;

            Core.Client.MessageReceived -= HandleMessageAsync;
            Core.Client.ReactionAdded -= HandleReactionAsync;
            IsRunning = false;
        }

        protected virtual async Task HandleMessageAsync(SocketMessage message)
        {
        }

        protected virtual async Task HandleReactionAsync(
            Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel channel,
            SocketReaction reaction)
        {
        }
    }
}