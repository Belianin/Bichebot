using System.Linq;

namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public static class CharacterOwnerExtensions
    {
        public static bool IsBichehost(this CharacterOwner owner)
        {
            return new[]
            {
                CharacterOwner.Aler,
                CharacterOwner.Host,
                CharacterOwner.Fanta,
                CharacterOwner.Ness,
                CharacterOwner.Qwear,
            }.Contains(owner);
        }
    }
}