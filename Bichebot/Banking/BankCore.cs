using System.Collections.Generic;
using System.Threading.Tasks;
using Bichebot.Repositories;
using Bichebot.Utilities;

namespace Bichebot.Banking
{
    public class BankCore : IBankCore
    {
        private readonly BichemansRepository repository;
        private readonly Dictionary<ulong, Bicheman> bichemans = new Dictionary<ulong, Bicheman>();

        public BankCore(BichemansRepository repository)
        {
            this.repository = repository;
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
            
            return value;
        }

        public async Task<Result<int>> AddAsync(ulong id, int value)
        {
            var user = await GetUser(id).ConfigureAwait(false);
            var balance = user.Bichecoins + value;
            if (balance < 0)
                balance = 0;

            user.Bichecoins = balance;

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

        private async Task<Bicheman> GetUser(ulong id)
        {
            if (bichemans.TryGetValue(id, out var user))
                return user;

            return await repository.GetOrCreateAsync(id, new Bicheman {Id = id, Bichecoins = 0}).ConfigureAwait(false);
        }
    }
}