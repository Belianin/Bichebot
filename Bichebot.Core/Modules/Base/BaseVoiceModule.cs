using System.Threading.Tasks;
using Discord.WebSocket;

namespace Bichebot.Core.Modules.Base
{
    public abstract class BaseVoiceModule : IBotModule
    {
        protected readonly IBotCore Core;

        protected BaseVoiceModule(IBotCore core)
        {
            Core = core;
        }

        public bool IsRunning { get; private set; }

        public void Run()
        {
            if (IsRunning)
                return;

            Core.Client.UserVoiceStateUpdated += HandleMessageAsync;
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            Core.Client.UserVoiceStateUpdated -= HandleMessageAsync;
            IsRunning = false;
        }

        protected abstract Task HandleMessageAsync(SocketUser user, SocketVoiceState firstState,
            SocketVoiceState secondState);
    }
}