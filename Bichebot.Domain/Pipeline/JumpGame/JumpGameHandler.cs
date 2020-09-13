using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Core.Pipeline;
using Discord.WebSocket;

namespace Bichebot.Domain.Pipeline.JumpGame
{
    public class JumpGameHandler : IMessageHandler
    {
        private readonly JumpGame jumpGame;
        private readonly WithermansSettings settings;

        public JumpGameHandler(IBotCore core, WithermansSettings settings)
        {
            this.settings = settings;
            jumpGame = new JumpGame(core, settings.JumpGame.ChannelId);
        }

        public async Task<bool> HandleAsync(SocketMessage message)
        {
            if (message.Channel.Id == settings.JumpGame.ChannelId &&
                message.Author.Id == settings.Admin &&
                message.Content == "/start-jump-game")
            {
                Task.Run(() => jumpGame.Run());
                return true;
            }

            return false;
        }
    }
}