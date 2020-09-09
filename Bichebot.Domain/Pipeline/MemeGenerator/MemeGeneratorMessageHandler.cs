using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bichebot.Core.Pipeline;
using Discord;
using Discord.WebSocket;
using Nexus.Logging;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Bichebot.Domain.Pipeline.MemeGenerator
{
    public class MemeGeneratorMessageHandler : IMessageHandler
    {
        private readonly MemeGenerator generator;
        private readonly ILog log; 
        
        public MemeGeneratorMessageHandler(MemeGeneratorSettings settings, ILog log)
        {
            this.log = log.ForContext(GetType().Name);
            generator = new MemeGenerator(settings.MemePhrases);
        }

        public async Task<bool> HandleAsync(SocketMessage message)
        {
            if (!message.Content.ToLower().Contains("сделай мем"))
                return false;

            log.Info("Received a meme-request");
            
            var attachment = message.Attachments.FirstOrDefault();
            if (attachment == null)
            {
                log.Info("No attachments found");
                return false;
            }
            
            var regex = new Regex("\"(.*?)\"");
            var phrase = regex.Match(message.Content).Value;
            phrase = string.IsNullOrEmpty(phrase) ? null : phrase[1..^1];
            log.Info(phrase == null ? "No meme phrase required" : $"Meme phrase is \'{phrase}\'");

            var bitmap = await DownloadImageAsync(attachment.Url);

            var meme = generator.GenerateMeme(bitmap, phrase);

            await message.DeleteAsync().ConfigureAwait(false);
            await SendMemeAsync(message, meme).ConfigureAwait(false);

            return true;
        }

        private static async Task<SixLabors.ImageSharp.Image> DownloadImageAsync(string url)
        {
            using var client = new HttpClient();
            var picture = await client.GetAsync(url).ConfigureAwait(false);

            await using var stream = await picture.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return SixLabors.ImageSharp.Image.Load(stream);
        }

        private static async Task SendMemeAsync(IMessage message, SixLabors.ImageSharp.Image meme)
        {
            var stream = new MemoryStream();
            meme.Save(stream, new JpegEncoder());
            stream.Position = 0;

            await message.Channel.SendFileAsync(stream, "meme.jpeg", $"Шляпа от {message.Author.Username}").ConfigureAwait(false);
        }
    }
}