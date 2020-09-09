using System.Threading.Tasks;
using Discord.WebSocket;

namespace Bichebot.Core.Modules.Base
{
    public abstract class BaseMessageModule : IBotModule
    {
        protected readonly IBotCore Core;
        protected abstract Task HandleMessageAsync(SocketMessage message);
        protected BaseMessageModule(IBotCore core)
        {
            Core = core;
        }

        public bool IsRunning { get; private set; }
        
        public void Run()
        {
            if (IsRunning)
                return;
            
            Core.Client.MessageReceived += HandleMessageAsync;
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;
            
            Core.Client.MessageReceived -= HandleMessageAsync;
            IsRunning = false;
        }
    }
}