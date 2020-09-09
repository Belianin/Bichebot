namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public class FoStatistics
    {
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

        public static FoStatistics operator -(FoStatistics current, FoStatistics prev)
        {
            return new FoStatistics
            {
                Player = current.Player,
                Death = current.Death - prev.Death,
                Kills = current.Kills - prev.Kills,
                Rating = current.Rating - prev.Rating
            };
        }
    }
}