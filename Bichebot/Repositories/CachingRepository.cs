using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Utilities;

namespace Bichebot.Repositories
{
    public class CachingRepository<TKey, TValue> : IRepository<TKey, TValue>
    {
        private readonly IRepository<TKey, TValue> innerRepository;

        private readonly IDictionary<TKey, TValue> memory;

        public CachingRepository(IRepository<TKey, TValue> innerRepository)
        {
            memory = new Dictionary<TKey, TValue>();
            this.innerRepository = innerRepository;
            
            var data = innerRepository.GetAllAsync().Result;
            foreach (var (key, value) in data) 
                memory[key] = value;

            AppDomain.CurrentDomain.ProcessExit += (s, e) => SaveStates().Wait();
            
            Task.Run(SaveStates);
        }

        public async Task<Result<TValue>> TryGetAsync(TKey key)
        {
            if (memory.TryGetValue(key, out var result))
                return result;

            return "Key not found";
        }

        public async Task<IDictionary<TKey, TValue>> GetAllAsync()
        {
            var result = await innerRepository.GetAllAsync().ConfigureAwait(false);
            foreach (var (key, value) in memory) 
                result[key] = value;

            return result;
        }

        public async Task CreateOrUpdateAsync(TKey key, TValue item)
        {
            memory[key] = item;
        }

        public async Task DeleteAsync(TKey key)
        {
            memory.Remove(key);
            await innerRepository.DeleteAsync(key).ConfigureAwait(false);
        }

        private async Task SaveStates() // and clear redundant memory
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromHours(1));
                foreach (var (key, value) in memory)
                    await innerRepository.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
            }
        }
    }
}