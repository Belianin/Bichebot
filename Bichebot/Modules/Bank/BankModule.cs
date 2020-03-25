using System;
using System.Threading.Tasks;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Modules.Base;
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

            switch (message.Content)
            {
                case "/bank-help":
                    await ShowBalanceAsync(message).ConfigureAwait(false);
                    return;
                case "/bank-balance":
                    await ShowBalanceAsync(message).ConfigureAwait(false);
                    return;
            }
        }
        
        private async Task ShowHelpAsync(SocketMessage message)
        {
            await message.Channel
                .SendMessageAsync("/bank-help, /bank-balance")
                .ConfigureAwait(false);
        }

        private async Task ShowBalanceAsync(SocketMessage message)
        {
            await message.Channel
                .SendMessageAsync($"{message.Author.Username}: {bank.GetBalance(message.Author.Id)}.0")
                .ConfigureAwait(false);
        }

        private bool FromAdmin(IMessage message) => settings.Admins.Contains(message.Author.Id);
    }
}