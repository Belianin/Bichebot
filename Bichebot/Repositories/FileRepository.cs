using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bichebot.Utilities;
using Newtonsoft.Json;

namespace Bichebot.Repositories
{
    // JsonFileRepository
    public class FileRepository<TKey, TValue> : IRepository<TKey, TValue>
    {
        private readonly string directory;
        private readonly Func<string, TKey> convertFileName;

        public FileRepository(string directory, Func<string, TKey> convertFileName)
        {
            this.directory = directory;
            this.convertFileName = convertFileName;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public async Task<Result<TValue>> TryGetAsync(TKey key)
        {
            var path = $"{directory}/{key.ToString()}";
            if (!File.Exists(path))
                return $"File {path} not found";

            var data = await File.ReadAllTextAsync(path).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TValue>(data);
        }

        public async Task<IDictionary<TKey, TValue>> GetAllAsync()
        {
            var result = new Dictionary<TKey, TValue>();
            foreach (var file in Directory.EnumerateFiles(directory))
            {
                var data = await File.ReadAllTextAsync(file).ConfigureAwait(false);

                result[convertFileName(file)] = JsonConvert.DeserializeObject<TValue>(data);
            }

            return result;
        }

        public Task CreateOrUpdateAsync(TKey key, TValue item)
        {
            return File.WriteAllTextAsync(
                $"{directory}/{key.ToString()}",
                JsonConvert.SerializeObject(item, Formatting.Indented));
        }

        public async Task DeleteAsync(TKey key)
        {
            var path = $"{directory}/{key.ToString()}";
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}