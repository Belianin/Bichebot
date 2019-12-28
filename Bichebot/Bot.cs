using System;
using System.Collections.Generic;
using System.Threading;
using Bichebot.Core;
using Bichebot.Modules;
using Discord;

namespace Bichebot
{
    public class Bot
    {
        private readonly IBotCore core;

        private readonly ICollection<IBotModule> modules;

        private readonly string token;

        internal Bot(IBotCore core, ICollection<IBotModule> modules, string token)
        {
            this.core = core;
            this.modules = modules;
            this.token = token;
        }

        public void Run(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting");
            
            core.Client.LoginAsync(TokenType.Bot, token).Wait(cancellationToken);
            core.Client.StartAsync().Wait(cancellationToken);

            foreach (var module in modules)
                module.Run();
            
            Console.WriteLine("Started");

            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
        }
    }
}