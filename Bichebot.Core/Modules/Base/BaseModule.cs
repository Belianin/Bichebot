using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Core.Modules.Base
{
    public abstract class BaseModule : IBotModule
    {
        protected readonly IBotCore Core;

        private Task work;
        private CancellationTokenSource cts;

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

            cts = new CancellationTokenSource();
            work = Task.Run(async () =>
            {
                try
                {
                    await DoWorkAsync(cts.Token).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    await Core.Guild.GetTextChannel(553146693743280128)
                        .SendMessageAsync(e.Message)
                        .ConfigureAwait(false);
                }
            }, cts.Token);
        }

        public virtual void Stop()
        {
            if (!IsRunning)
                return;

            cts.Cancel();
            Core.Client.MessageReceived -= HandleMessageAsync;
            Core.Client.ReactionAdded -= HandleReactionAsync;
            IsRunning = false;
        }

        protected virtual async Task DoWorkAsync(CancellationToken token) {}

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