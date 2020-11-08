using System.Collections.Generic;
using Bichebot.Core.Utilities;

namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public interface IStatisticsProvider
    {
        Result<IEnumerable<FoStatistics>> GetTotalStatistics();
        Result<CharacterStatistics> GetCharacterStatistics(string link); // дерьмовый контракт
    }
}