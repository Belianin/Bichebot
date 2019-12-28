using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Best;
using Bichebot.Modules.Moderate;
using Bichebot.Modules.React;
using Bichebot.Modules.Statistics;
using Bichebot.Modules.Supreme;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Bichebot
{
    public class Bot
    {
        private readonly IBotCore core;

        private readonly AudioSpeaker audio;

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

        private async Task HandleMessageAsync(SocketMessage message)
        {
            Console.WriteLine(message.Content);
            var msg = new Message
            {
                DiscordMessage = message,
                Tts = message.Content.Contains("tts")
            };

            if (message.Content == "go")
                audio.Connect(message.Author);
            else if (message.Content == "ilidan")
                await audio.SendMessageAsync("ilidan.mp3").ConfigureAwait(false);
        }
    }
}