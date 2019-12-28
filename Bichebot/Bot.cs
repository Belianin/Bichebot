using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Best;
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

        private readonly BotConfig config;

        private readonly Random rnd = new Random();

        public Bot(BotConfig config)
        {
            this.config = config;
            var discordClient = new DiscordSocketClient();
            
            core = new BotCore(config.GuildId, discordClient);
            audio = new AudioSpeaker(core);
            modules = new List<IBotModule>
            {
                new StatisticsModule(core),
                new ReactModule(core),
                new BestModule(core, new BestModuleSettings
                {
                    BestChannelId = config.BestChannelId,
                    ReactionCountToBeBest = config.ReactionCountToBeBest
                })
            };
            
            foreach (var module in modules)
                module.Run();

            discordClient.MessageReceived += HandleMessageAsync;
        }

        public async Task RunAsync(CancellationToken token)
        {
            Console.WriteLine("Starting");
            await core.Client.LoginAsync(TokenType.Bot, config.Token).ConfigureAwait(false);
            await core.Client.StartAsync().ConfigureAwait(false);
            Console.WriteLine("Started");

            while (!token.IsCancellationRequested)
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
            else if (message.Content.Contains("бот") && message.Content.Contains("удали"))
                await DeletePreviousMessageAsync(message).ConfigureAwait(false);
        }

        private static async Task DeletePreviousMessageAsync(SocketMessage message)
        {
            var messages = await message.Channel.GetMessagesAsync(message, Direction.Before, 1).FlattenAsync()
                .ConfigureAwait(false);

            var messageToDelete = messages.FirstOrDefault();
            
            if (messageToDelete is IUserMessage userMessage && !userMessage.Content.Contains("~~"))
            {
                await userMessage.DeleteAsync().ConfigureAwait(false);
                await message.Channel.SendMessageAsync($"||~~{userMessage.Content}~~||")
                    .ConfigureAwait(false);
            }
        }
    }
}