namespace Bichebot.Core.Users
{
    public class User
    {
        public User(ulong id, string name)
        {
            Id = id;
            Name = name;
        }

        public ulong Id { get; set; }
        public string Name { get; set; }
        public int Bichecoins { get; set; } = 0;
    }
}