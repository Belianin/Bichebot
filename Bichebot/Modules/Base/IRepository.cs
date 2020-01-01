using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bichebot.Modules.Base
{
    public interface IRepository<TKey, TValue>
    {
        Task<TValue> GetAsync(TKey key);
        Task<IEnumerable<(TKey key, TValue value)>> GetAllAsync();
        Task CreateOrUpdateAsync(TKey key, TValue item);
        Task DeleteAsync(TKey key);
    }

    public class FakeRepository<TKey, TValue> : IRepository<TKey, TValue>
    {
        public async Task<IEnumerable<(TKey key, TValue value)>> GetAllAsync()
        {
            return new (TKey, TValue)[0];
        }

        public async Task CreateOrUpdateAsync(TKey key, TValue item)
        {
            
        }

        public async Task<TValue> GetAsync(TKey key)
        {
            return default;
        }

        public async Task DeleteAsync(TKey key)
        {
            
        }
    }
}