using Bichebot.Modules.Base;
using Bichebot.Utilities;

namespace Bichebot.Banking
{
    public class BankCore : IBankCore
    {
        private readonly IRepository<ulong, int> repository;

        public BankCore(IRepository<ulong, int> repository)
        {
            this.repository = repository;
        }

        public int GetBalance(ulong id)
        {
            return repository.GetOrCreateAsync(id, 0).Result;
        }

        public Result<int> SetBalance(ulong id, int value)
        {
            if (value < 0)
                return "Negative value";
            
            repository.CreateOrUpdateAsync(id, value);
            return value;
        }

        public Result<int> Add(ulong id, int value)
        {
            var balance = repository.GetOrCreateAsync(id, 0).Result + value;
            if (balance < 0)
                balance = 0;
            
            repository.CreateOrUpdateAsync(id, balance);

            return balance;
        }

        // В теории конечно потокобезопастности бы
        public Result TryTransact(ulong @from, ulong to, int value)
        {
            if (GetBalance(from) >= value)
            {
                Add(from, -value);
                Add(to, value);
                
                return Result.Ok();
            }


            return "Not enough balance";
        }
    }
}