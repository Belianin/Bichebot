using System.Collections.Generic;

namespace Bichebot.Modules.FOnlineGame
{
    public class Character
    {
        public int Level { get; set; }
        
        public int Experience { get; set; }
        
        public Characteristics Characteristics { get; set; }
        
        public List<Item> Inventory { get; set; }
    }
}