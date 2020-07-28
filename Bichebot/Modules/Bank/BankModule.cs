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

        protected override Task HandleMessageAsync(SocketMessage message)
        {
            if (FromAdmin(message))
            {
                
            }

            switch (message.Content)
            {
                case "мои коины":
                case "/bank-balance":
                    return ShowBalanceAsync(message);
            }
            
            return Task.CompletedTask;
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