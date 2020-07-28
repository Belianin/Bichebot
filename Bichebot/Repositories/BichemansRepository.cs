namespace Bichebot.Repositories
{
    public class BichemansRepository : CachingRepository<ulong, Bicheman>
    {
        public BichemansRepository() : base(new FileRepository<ulong, Bicheman>("Bichemans", ulong.Parse))
        {
        }
    }
}