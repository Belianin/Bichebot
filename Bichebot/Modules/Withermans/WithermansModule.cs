using System.Threading.Tasks;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Discord.WebSocket;

namespace Bichebot.Modules.Withermans
{
    public class WithermansModule : BaseModule
    {
        private readonly JumpGame jumpGame;
        private readonly WithermansSettings settings;
        public WithermansModule(IBotCore core, IBankCore bank, WithermansSettings settings) : base(core)
        {
            this.settings = settings;
            jumpGame = new JumpGame(core, bank, settings.JumpGame.ChannelId);
        }

        protected override Task HandleMessageAsync(SocketMessage message)
        {
            if (message.Channel.Id == settings.JumpGame.ChannelId &&
                message.Author.Id == settings.Admin &&
                message.Content == "/start-jump-game")
                return jumpGame.Run();
            return Task.CompletedTask;
        }
    }
}