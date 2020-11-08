namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public class CharacterStatistics
    {
        public string Name { get; set; }
        public Kill[] Kills { get; set; }
        public Kill[] Deaths { get; set; }
    }
}