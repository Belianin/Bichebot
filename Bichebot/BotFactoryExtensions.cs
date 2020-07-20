using System.IO;
using Newtonsoft.Json;

namespace Bichebot
{
    public static class BotFactoryExtensions
    {
        public static Bot Create(this IBotFactory factory, string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            try
            {

                var settings = JsonConvert.DeserializeObject<BotSettings>(path);

                return factory.Create(settings);
            }
            catch (JsonException e)
            {
                throw e;
            }
        }
    }
}