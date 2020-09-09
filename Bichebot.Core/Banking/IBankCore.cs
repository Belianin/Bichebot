using System.Threading.Tasks;
using Bichebot.Core.Users;
using Bichebot.Core.Utilities;

namespace Bichebot.Core.Banking
{
    public interface IBankCore
    {
        // времено
        void Register(User user);
        Task<Result<User[]>> GetAllBalanceAsync();
        Task<int> GetBalanceAsync(ulong id);

        Task<Result<int>> SetBalanceAsync(ulong id, int value);

        Task<Result<int>> AddAsync(ulong id, int value);
        
        Task<Result<Transaction>> TryTransactAsync(ulong from, ulong to, int value);
    }
}