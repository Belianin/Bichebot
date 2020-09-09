using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core.Repositories;
using Bichebot.Core.Users;
using Bichebot.Core.Utilities;

namespace Bichebot.Core.Banking
{
    public class BankCore : IBankCore
    {
        private readonly IUserRepository repository;

        public BankCore(IUserRepository repository)
        {
            this.repository = repository;
        }

        public void Register(User user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<User[]>> GetAllBalanceAsync()
        {
            return repository.GetAll();
        }

        public async Task<int> GetBalanceAsync(ulong id)
        {
            var user = GetUser(id);
            
            return user.IsSuccess ? user.Value.Bichecoins : -20; // ну и дерьмо
        }

        public async Task<Result<int>> SetBalanceAsync(ulong id, int value)
        {
            if (value < 0)
                return "Negative value";

            var user = GetUser(id);
            if (user.IsFail)
                return user.Error;
            user.Value.Bichecoins = value;

            repository.CreateOrUpdate(user.Value);
            
            return value;
        }

        public async Task<Result<int>> AddAsync(ulong id, int value)
        {
            var user = GetUser(id);
            if (user.IsFail)
                return user.Error;
            
            var balance = user.Value.Bichecoins + value;
            if (balance < 0)
                balance = 0;

            user.Value.Bichecoins = balance;

            repository.CreateOrUpdate(user.Value);

            return balance;
        }

        // В теории конечно потокобезопастности бы
        public async Task<Result<Transaction>> TryTransactAsync(ulong @from, ulong to, int value)
        {
            if (value < 1)
                return "Must be a positive number";
            var fromUser = GetUser(from);
            if (fromUser.IsFail)
                return fromUser.Error;
            if (fromUser.Value.Bichecoins < value)
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

        private Result<User> GetUser(ulong id)
        {
            return repository.Get(id);
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