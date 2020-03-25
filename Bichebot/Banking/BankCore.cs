using Bichebot.Modules.Base;

namespace Bichebot.Banking
{
    public class BankCore : IBankCore<ulong>
    {
        private readonly IRepository<ulong, int> repository;
        
        public int GetBalance(ulong id)
        {
            throw new System.NotImplementedException();
        }

        public int SetBalance(ulong id, int value)
        {
            throw new System.NotImplementedException();
        }

        public int Add(ulong id, int value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryTransact(ulong @from, ulong to, int value)
        {
            throw new System.NotImplementedException();
        }
    }
}