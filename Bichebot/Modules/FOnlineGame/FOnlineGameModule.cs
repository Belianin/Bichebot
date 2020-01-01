using Bichebot.Core;
using Bichebot.Modules.Base;

namespace Bichebot.Modules.FOnlineGame
{
    public class FOnlineGameModule : StatefulBaseModule<Character>
    {
        public FOnlineGameModule(IBotCore core) : base(core, () => new Character())
        {
            
        }
    }
}