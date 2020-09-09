using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core.Modules;
using Bichebot.Core.Pipeline;
using Discord;
using Discord.WebSocket;
using Nexus.Logging;

namespace Bichebot.Core
{
    public class Bot
    {
        private readonly IBotCore core;
        private readonly ICollection<IBotModule> modules;
        private readonly IMessagePipeline pipeline;

        internal Bot(IBotCore core, ICollection<IBotModule> modules, IMessagePipeline pipeline)
        {
            this.core = core;
            this.modules = modules;
            this.pipeline = pipeline;
        }

        public async Task RunAsync(string token, CancellationToken cancellationToken)
        {
            core.Log.Info("Starting");

            core.Client.LoginAsync(TokenType.Bot, token).Wait(cancellationToken);
            core.Client.StartAsync().Wait(cancellationToken);

            core.Client.MessageReceived += HandleMessage;

            foreach (var module in modules)
                module.Run();

            core.Log.Info("Started");

            await Task.Delay(-1, cancellationToken).ConfigureAwait(false);
            
            core.Log.Info("Stopping");
        }

        private Task HandleMessage(SocketMessage message)
        {
            return pipeline.HandleAsync(message);
        }
    }
}