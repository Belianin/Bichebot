using System.Collections.Generic;

namespace Bichebot.Domain.Modules.Best
{
    public class BestModuleSettings
    {
        public ulong BestChannelId { get; set; }

        public int ReactionCountToBeBest { get; set; } = 3;

        public int Reward { get; set; } = 100;

        public HashSet<ulong> SourceChannelIds { get; set; }
    }
}