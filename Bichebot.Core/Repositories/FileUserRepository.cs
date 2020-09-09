using System.IO;
using System.Linq;
using Bichebot.Core.Users;
using Bichebot.Core.Utilities;
using Newtonsoft.Json;

namespace Bichebot.Core.Repositories
{
    public class FileUserRepository : IUserRepository
    {
        private readonly string directory;

        public FileUserRepository(string directory)
        {
            this.directory = directory;
        }

        public Result<User> Get(ulong id)
        {
            var path = Path.Combine(directory, id.ToString());
            if (!File.Exists(path))
                return $"File {path} not found";

            var data = File.ReadAllText(path);

            return data.TryDeserialize<User>();
        }

        public Result CreateOrUpdate(User user)
        {
            File.WriteAllTextAsync(
                $"{directory}/{user.Id.ToString()}",
                JsonConvert.SerializeObject(user, Formatting.Indented));

            return Result.Ok();
        }

        public Result<User[]> GetAll()
        {
            var users = Directory.EnumerateFiles(directory)
                .Select(File.ReadAllText)
                .Select(data => data.TryDeserialize<User>())
                .ToArray();

            return users.Any(u => u.IsFail)
                ? "Failed to get all users"
                : Result<User[]>.Ok(users.Select(u => u.Value).ToArray());
        }
    }
}