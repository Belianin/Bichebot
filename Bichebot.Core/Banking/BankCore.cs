using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core.Repositories;
using Bichebot.Core.Utilities;

namespace Bichebot.Core.Banking
{
    public class BankCore : IBankCore
    {
        private readonly IRepository<ulong, Bicheman> repository;

        public BankCore(IRepository<ulong, Bicheman> repository)
        {
            this.repository = repository;
        }

        public async Task<(string username, int balance)[]> GetAllBalanceAsync()
        {
            var all = await repository.GetAllAsync().ConfigureAwait(false);

            return all.Values.Select(b => (b.Name, b.Bichecoins)).ToArray();
        }

        public async Task<int> GetBalanceAsync(ulong id)
        {
            var user = await GetUser(id).ConfigureAwait(false);
            
            return user.Bichecoins;
        }

        public async Task<Result<int>> SetBalanceAsync(ulong id, int value)
        {
            if (value < 0)
                return "Negative value";

            var user = await GetUser(id).ConfigureAwait(false);
            user.Bichecoins = value;

            await repository.CreateOrUpdateAsync(user.Id, user).ConfigureAwait(false);
            
            return value;
        }

        public async Task<Result<int>> AddAsync(ulong id, int value)
        {
            var user = await GetUser(id).ConfigureAwait(false);
            var balance = user.Bichecoins + value;
            if (balance < 0)
                balance = 0;

            user.Bichecoins = balance;

            await repository.CreateOrUpdateAsync(user.Id, user).ConfigureAwait(false);

            return balance;
        }

        // В теории конечно потокобезопастности бы
        public async Task<Result<Transaction>> TryTransactAsync(ulong @from, ulong to, int value)
        {
            if (value < 1)
                return "Must be a positive number";
            var fromUser = await GetUser(from).ConfigureAwait(false);
            if (fromUser.Bichecoins < value)
                return "Not enough balance";
            
            var fromBalance = await AddAsync(@from, -value).ConfigureAwait(false);
            var toBalance = await AddAsync(to, value).ConfigureAwait(false);

            return new Transaction
            {
                Form = from,
                To = to,
                Value = value,
                FromBalance = fromBalance.Value,
                ToBalance = toBalance.Value
            };
        }

        private Task<Bicheman> GetUser(ulong id)
        {
            return repository.GetOrCreateAsync(id, new Bicheman(id, "Name"));
        }
    }

    public class Transaction
    {
        public ulong Form { get; set; }
        public ulong To { get; set; }
        public int Value { get; set; }
        public int FromBalance { get; set; }
        public int ToBalance { get; set; }
    }
}