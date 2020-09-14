using System.Collections.Generic;

namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public class FonlineStatisticsModuleSettings
    {
        public ulong ChannelId { get; set; }
        
        public Dictionary<string, ulong> Color { get; set; }
        
        public PriceList PriceList { get; set; }
        public ulong RewardChannelId { get; set; }
    }
}