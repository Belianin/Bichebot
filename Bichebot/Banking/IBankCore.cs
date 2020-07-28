using System.Threading.Tasks;
using Bichebot.Utilities;

namespace Bichebot.Banking
{
    public interface IBankCore
    {
        Task<(string username, int balance)[]> GetAllBalanceAsync();
        Task<int> GetBalanceAsync(ulong id);

        Task<Result<int>> SetBalanceAsync(ulong id, int value);

        Task<Result<int>> AddAsync(ulong id, int value);
        
        Task<Result> TryTransactAsync(ulong from, ulong to, int value);
    }
}