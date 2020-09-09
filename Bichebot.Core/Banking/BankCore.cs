using Bichebot.Core.Users;
using Bichebot.Core.Utilities;

namespace Bichebot.Core.Banking
{
    // дерьмовая абстракция
    public class BankCore : IBankCore
    {
        private readonly IUserRepository repository;

        public BankCore(IUserRepository repository)
        {
            this.repository = repository;
        }

        public Result Register(User user)
        {
            if (repository.Get(user.Id).IsFail)
                return repository.CreateOrUpdate(user);
            
            return Result.Ok();
        }

        public Result<int> GetBalance(ulong id)
        {
            var userResult = GetUser(id);
            if (userResult.IsFail)
                return userResult.Error;
            var user = userResult.Value;

            return user.Bichecoins;
        }

        public Result<int> Add(ulong id, int value)
        {
            var userResult = GetUser(id);
            if (userResult.IsFail)
                return userResult.Error;
            var user = userResult.Value;

            var balance = userResult.Value.Bichecoins + value;
            if (balance < 0)
                return "Negative balance";

            user.Bichecoins = balance;
            repository.CreateOrUpdate(user);

            return balance;
        }

        public Result<int> SetBalance(ulong id, int value)
        {
            if (value < 0)
                return "Negative value";

            var userResult = GetUser(id);
            if (userResult.IsFail)
                return $"Failed to get user: {userResult.Error}";
            var user = userResult.Value;

            user.Bichecoins = value;
            repository.CreateOrUpdate(user);

            return value;
        }

        public Result<Transaction> TryTransact(ulong from, ulong to, int value)
        {
            if (value < 1)
                return "Must be a positive number";
            var fromUser = GetUser(from);
            if (fromUser.IsFail)
                return fromUser.Error;
            if (fromUser.Value.Bichecoins < value)
                return "Not enough balance";

            var fromBalance = Add(from, -value);
            if (fromBalance.IsFail)
                return $"Failed to reduce from-balance: {fromBalance.Error}";
            var toBalance = Add(to, value);
            if (fromBalance.IsFail)
            {
                var revertResult = Add(from, value);
                if (revertResult.IsFail)
                    return "Failed to revert transaction!";
                return "Failed to add value to to-balance";
            }

            return new Transaction
            {
                Form = from,
                To = to,
                Value = value,
                FromBalance = fromBalance.Value,
                ToBalance = toBalance.Value
            };
        }

        public Result<User[]> GetAllBalances()
        {
            return repository.GetAll();
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