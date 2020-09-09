using Bichebot.Core.Users;
using Bichebot.Core.Utilities;

namespace Bichebot.Core.Banking
{
    public interface IBankCore
    {
        Result Register(User user);
        Result<int> GetBalance(ulong id);
        Result<int> Add(ulong id, int value);
        Result<int> SetBalance(ulong id, int value);
        Result<Transaction> TryTransact(ulong from, ulong to, int value);
        Result<User[]> GetAllBalances();
    }
}