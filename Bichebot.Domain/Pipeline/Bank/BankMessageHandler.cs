using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Core.Pipeline;
using Bichebot.Core.Users;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Domain.Pipeline.Bank
{
    public class BankMessageHandler : IMessageHandler
    {
        private readonly IBotCore core;

        private readonly BankModuleSettings settings;

        public BankMessageHandler(IBotCore core, BankModuleSettings settings)
        {
            this.settings = settings;
            this.core = core;
        }

        public async Task<bool> HandleAsync(SocketMessage message)
        {
            if (FromAdmin(message))
                if (Regex.IsMatch(message.Content, @"дать -?\d+$"))
                {
                    var recipients2 = message.MentionedUsers.ToArray();
                    if (recipients2.Length != 1)
                        await message.Channel
                            .SendMessageAsync($"Чел, переводи по одному {core.ToEmojiString("kripotabamboe")}")
                            .ConfigureAwait(false);

                    var words = message.Content.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var numbers = words.Where(w => int.TryParse(w, out _)).Select(int.Parse).ToArray();

                    if (numbers.Length != 1)
                    {
                        await message.Channel.SendMessageAsync($"Чё {core.ToEmojiString("valera")} ?")
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        var transaction = core.Bank.Add(recipients2.First().Id, numbers.First());

                        if (transaction.IsSuccess)
                        {
                            var t = transaction.Value;
                            await message.Channel.SendMessageAsync($"{recipients2.First().Username}: {t}")
                                .ConfigureAwait(false);
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync($"Проблемка: {transaction.Error}")
                                .ConfigureAwait(false);
                        }
                    }
                }

            var recipients = message.MentionedUsers.Where(u => u.Id != message.Author.Id).ToArray();
            if (Regex.IsMatch(message.Content, @"лови \d+$") && recipients.Length > 0)
            {
                if (recipients.Length != 1)
                    await message.Channel
                        .SendMessageAsync($"Чел, переводи по одному {core.ToEmojiString("kripotabamboe")}")
                        .ConfigureAwait(false);

                var words = message.Content.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var numbers = words.Where(w => int.TryParse(w, out _)).Select(int.Parse).ToArray();

                if (numbers.Length != 1)
                {
                    await message.Channel.SendMessageAsync($"Чё {core.ToEmojiString("valera")} ?")
                        .ConfigureAwait(false);
                }
                else
                {
                    var transaction = core.Bank.TryTransact(message.Author.Id, recipients.First().Id, numbers.First());

                    if (transaction.IsSuccess)
                    {
                        var t = transaction.Value;
                        await message.Channel
                            .SendMessageAsync(
                                $"{message.Author.Username}: {t.FromBalance} -> {t.Value} -> {t.ToBalance} :{recipients.First().Username}")
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

            return false;
        }

        private async Task ShowAllBalanceAsync(SocketMessage message)
        {
            var balances = core.Bank.GetAllBalances();
            if (balances.IsFail)
                await message.Channel.SendMessageAsync("Не смогли достать всех пользователей")
                    .ConfigureAwait(false);
            else
                await message.Channel
                    .SendMessageAsync(
                        $"Бичекоины:\n{string.Join("\n", balances.Value.Select(t => $"{t.Name}: {t.Bichecoins}"))}")
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
            var balance = core.Bank.GetBalance(message.Author.Id);
            if (balance.IsFail)
                await message.Channel.SendMessageAsync($"Не получилось узнать баланс: {balance.Error}")
                    .ConfigureAwait(false);
            else
                await message.Channel
                    .SendMessageAsync($"{message.Author.Username}: {balance.Value}.0")
                    .ConfigureAwait(false);
        }

        private bool FromAdmin(IMessage message)
        {
            return settings.Admins.Contains(message.Author.Id);
        }
    }
}