using System.Collections.Generic;
using System.Threading.Tasks;
using Bichebot.Utilities;

namespace Bichebot.Repositories
{
    public class FakeRepository<TKey, TValue> : IRepository<TKey, TValue>
    {
        public async Task<IDictionary<TKey, TValue>> GetAllAsync()
        {
            return new Dictionary<TKey, TValue>();
        }

        public async Task CreateOrUpdateAsync(TKey key, TValue item)
        {
            
        }

        public async Task<Result<TValue>> TryGetAsync(TKey key)
        {
            return default;
        }

        public async Task DeleteAsync(TKey key)
        {
            
        }
    }
}