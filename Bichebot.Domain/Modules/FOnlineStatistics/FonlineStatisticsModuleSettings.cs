using System.Collections.Generic;

namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public class FonlineStatisticsModuleSettings
    {
        public ulong ChannelId { get; set; }
        
        public Dictionary<string, CharacterOwner> Color { get; set; }
        
        public PriceList PriceList { get; set; }
    }
}