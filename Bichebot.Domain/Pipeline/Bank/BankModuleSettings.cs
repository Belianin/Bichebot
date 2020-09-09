using System.Collections.Generic;

namespace Bichebot.Domain.Pipeline.Bank
{
    public class BankModuleSettings
    {
        public ICollection<ulong> Admins { get; set; }
    }
}