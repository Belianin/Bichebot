using System.Collections.Generic;

namespace Bichebot.Domain.Pipeline.Bank
{
    public class BankSettings
    {
        public ICollection<ulong> Admins { get; set; }
    }
}