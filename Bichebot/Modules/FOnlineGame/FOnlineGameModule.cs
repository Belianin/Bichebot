using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Discord.WebSocket;

namespace Bichebot.Modules.FOnlineGame
{
    public class FOnlineGameModule : StatefulBaseModule<Character>
    {
        private readonly FOnlineGameModuleSettings settings;
        public FOnlineGameModule(IBotCore core, FOnlineGameModuleSettings settings)
            : base(core, () => new Character())
        {
            this.settings = settings;
        }

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (message.Channel.Id != settings.GameChannelId)
                return;
            return;
        }
    }
}