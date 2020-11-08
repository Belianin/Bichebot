using Bichebot.Domain.Modules.FOnlineStatistics;
using NUnit.Framework;

namespace Bichebot.Tests
{
    [TestFixture]
    public class StatisticsProviderTests
    {
        [Test]
        public void Should_get_character_statistics()
        {
            var provider = new StatisticsProvider();

            var result = provider.GetCharacterStatistics("char_info.php?s=23&char_id=2336");
            
            Assert.IsTrue(result.IsSuccess);
        }
        
        [Test]
        public void Should_get_total_statistics()
        {
            var provider = new StatisticsProvider();

            var result = provider.GetTotalStatistics();
            
            Assert.IsTrue(result.IsSuccess);
        }
    }
}