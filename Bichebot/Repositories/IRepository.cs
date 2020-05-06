using System.Collections.Generic;
using System.Threading.Tasks;
using Bichebot.Utilities;

namespace Bichebot.Repositories
{
    public interface IRepository<TKey, TValue>
    {
        Task<Result<TValue>> TryGetAsync(TKey key);
        Task<IDictionary<TKey, TValue>> GetAllAsync();
        Task CreateOrUpdateAsync(TKey key, TValue item);
        Task DeleteAsync(TKey key);
    }

    public static class RepositoryExtensions
    {
        public static async Task<TValue> GetOrCreateAsync<TKey, TValue>(this IRepository<TKey, TValue> repo, TKey key, TValue value)
        {
            var getResult = await repo.TryGetAsync(key).ConfigureAwait(false);
            if (getResult.IsSuccess)
                return getResult.Value;

            await repo.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
            return value;
        }
    }
}