namespace Bichebot.Core
{
    public class Bicheman
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public int Bichecoins { get; set; } = 0;

        public Bicheman(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}