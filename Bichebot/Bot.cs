using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules;
using Discord;
using Nexus.Logging;

namespace Bichebot
{
    public class Bot
    {
        private readonly IBotCore core;
        private readonly ICollection<IBotModule> modules;
        private readonly ILog log;
        private readonly string token;

        internal Bot(IBotCore core, ICollection<IBotModule> modules, string token, ILog log)
        {
            this.core = core;
            this.modules = modules;
            this.token = token;
            this.log = log;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            log.Info("Starting");

            core.Client.LoginAsync(TokenType.Bot, token).Wait(cancellationToken);
            core.Client.StartAsync().Wait(cancellationToken);

            foreach (var module in modules)
                module.Run();

            log.Info("Started");

            await Task.Delay(-1, cancellationToken).ConfigureAwait(false);
            
            log.Info("Stopping");
        }
    }
}