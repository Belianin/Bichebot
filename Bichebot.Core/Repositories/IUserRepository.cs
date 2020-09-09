using Bichebot.Core.Users;
using Bichebot.Core.Utilities;

namespace Bichebot.Core.Repositories
{
    public interface IUserRepository
    {
        Result<User> Get(ulong id);
        
        Result CreateOrUpdate(User user);

        Result<User[]> GetAll();
    }
}