using System.Collections.Generic;
using Bichebot.Domain.Pipeline.React.Triggers;

namespace Bichebot.Domain.Pipeline.React
{
    public class ReactSettings
    {
        public HashSet<ulong> ReactChannels { get; set; } = new HashSet<ulong>
        {
            553146693743280128, 676118662205276163, 656922777344802882
        };
        
        public ICollection<IReactionTrigger> Triggers { get; set; }
    }
}