using System.Collections.Generic;

namespace Bichebot.Modules.Bank
{
    public class BankModuleSettings
    {
        public ICollection<ulong> Admins { get; }

        public BankModuleSettings(ICollection<ulong> admins = null)
        {
            Admins = admins ?? new List<ulong>();
        }
    }
}