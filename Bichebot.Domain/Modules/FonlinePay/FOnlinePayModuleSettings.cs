using System.Collections.Generic;

namespace Bichebot.Domain.Modules.FonlinePay
{
    public class FOnlinePayModuleSettings
    {
        public Dictionary<string, ulong> Color { get; set; }
        public PriceList PriceList { get; set; }
        public ulong RewardChannelId { get; set; }
    }
}