using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bichebot.Repositories;
using Bichebot.Utilities;

namespace Bichebot.Banking
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

            return all.Values.Select(b => (b.Id.ToString(), b.Bichecoins)).ToArray();
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
        public async Task<Result> TryTransactAsync(ulong @from, ulong to, int value)
        {
            var fromUser = await GetUser(from).ConfigureAwait(false);
            if (fromUser.Bichecoins < value)
                return "Not enough balance";
            
            await AddAsync(@from, -value).ConfigureAwait(false);
            await AddAsync(to, value).ConfigureAwait(false);
                
            return Result.Ok();


        }

        private Task<Bicheman> GetUser(ulong id)
        {
            return repository.GetOrCreateAsync(id, new Bicheman(id, "Name"));
        }
    }
}