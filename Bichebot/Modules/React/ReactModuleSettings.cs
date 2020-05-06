using System.Collections.Generic;
using Bichebot.Modules.React.Triggers;

namespace Bichebot.Modules.React
{
    public class ReactModuleSettings
    {
        public HashSet<ulong> ReactChannels { get; set; } = new HashSet<ulong>
        {
            553146693743280128, 676118662205276163, 656922777344802882
        };
        
        public ICollection<IReactionTrigger> Triggers { get; set; }
    }
}