using System.Collections.Generic;

namespace Bichebot.Domain.Modules.Best
{
    public class BestModuleSettings
    {
        public ulong BestChannelId { get; set; } = 602475515391246358;

        public int ReactionCountToBeBest { get; set; } = 3;

        public int Reward { get; set; } = 100;
        
        public HashSet<ulong> SourceChannelIds { get; set; } = new HashSet<ulong>
        {
            553146693743280128, 676118662205276163, 656922777344802882
        };
    }
}