using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Bichebot.Modules.React;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Modules.Bank
{
    public class BankModule : BaseModule
    {
        private readonly IBankCore bank;
        
        private readonly BankModuleSettings settings;
        
        public BankModule(IBotCore core, IBankCore bank, BankModuleSettings settings) : base(core)
        {
            this.settings = settings;
            this.bank = bank;
        }

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (FromAdmin(message))
            {
                
            }

            var recipients = message.MentionedUsers.Where(u => u.Id != message.Author.Id).ToArray();
            if (Regex.IsMatch(message.Content, @"лови \d+$") && recipients.Length > 0)
            {
                if (recipients.Length != 1)
                {
                    await message.Channel.SendMessageAsync($"Чел, переводи по одному {Core.ToEmojiString("kripotabamboe")}")
                        .ConfigureAwait(false); 
                }
                
                var words = message.Content.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var numbers = words.Where(w => int.TryParse(w, out var _)).Select(int.Parse).ToArray();

                if (numbers.Length != 1)
                {
                    await message.Channel.SendMessageAsync($"Чё {Core.ToEmojiString("valera")} ?")
                        .ConfigureAwait(false);
                }
                else
                {
                    var transaction = await bank.TryTransactAsync(message.Author.Id, recipients.First().Id, numbers.First())
                        .ConfigureAwait(false);

                    if (transaction.IsSuccess)
                    {
                        var t = transaction.Value;
                        await message.Channel.SendMessageAsync($"{message.Author.Username}: {t.FromBalance} -> {t.Value} -> {t.ToBalance} :{recipients.First().Username}")
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync($"Проблемка: {transaction.Error}")
                            .ConfigureAwait(false);
                    }
                }
                
            }

            switch (message.Content)
            {
                case "бичекоины":
                    await ShowAllBalanceAsync(message);
                    break;
                case "мои коины":
                case "/bank-balance":
                    await ShowBalanceAsync(message);
                    break;
            }
        }

        private async Task ShowAllBalanceAsync(SocketMessage message)
        {
            var balances = await bank.GetAllBalanceAsync().ConfigureAwait(false);
            await message.Channel
                .SendMessageAsync(
                    $"Бичекоины:\n{string.Join("\n", balances.Select(t => $"{t.username}: {t.balance}"))}")
                .ConfigureAwait(false);
        }
        
        private async Task ShowHelpAsync(SocketMessage message)
        {
            await message.Channel
                .SendMessageAsync("/bank-help, /bank-balance")
                .ConfigureAwait(false);
        }

        private async Task ShowBalanceAsync(SocketMessage message)
        {
            var balance = await bank.GetBalanceAsync(message.Author.Id).ConfigureAwait(false);
            await message.Channel
                .SendMessageAsync($"{message.Author.Username}: {balance}.0")
                .ConfigureAwait(false);
        }

        private bool FromAdmin(IMessage message) => settings.Admins.Contains(message.Author.Id);
    }
}