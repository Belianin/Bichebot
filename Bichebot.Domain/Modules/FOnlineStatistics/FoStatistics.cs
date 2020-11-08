namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public class FoStatistics
    {
        public string Link { get; set; }
        public string Player { get; set; }
        public int Rating { get; set; }
        public int Kills { get; set; }
        public int Death { get; set; }

        public StatisticsDiff ToNew()
        {
            return new StatisticsDiff
            {
                Player = Player,
                Rating = Rating,
                Kills = Kills,
                Death = Death,
                IsNew = true
            };
        }
    }
}