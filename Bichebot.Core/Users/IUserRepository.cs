using Bichebot.Core.Utilities;

namespace Bichebot.Core.Users
{
    public interface IUserRepository
    {
        Result<User> Get(ulong id);

        Result CreateOrUpdate(User user);

        Result<User[]> GetAll();
    }
}