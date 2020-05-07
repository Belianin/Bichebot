using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Bichebot.Utilities;
using Discord;
using Discord.WebSocket;
using Image = System.Drawing.Image;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Bichebot.Modules.MemeGenerator
{
    public class MemeGeneratorModule : BaseModule
    {
        private readonly MemeGenerator generator;
        
        public MemeGeneratorModule(IBotCore core, MemeGeneratorModuleSettings settings) : base(core)
        {
            generator = new MemeGenerator(settings.MemePhrases);
        }

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (!message.Content.ToLower().Contains("сделай мем"))
                return;
            
            var attachment = message.Attachments.FirstOrDefault();
            if (attachment == null)
                return;

            var bitmap = await DownloadImageAsync(attachment.Url);

            var meme = generator.GenerateMeme(bitmap);

            await SendMemeAsync(message, meme).ConfigureAwait(false);
        }

        private static async Task<Image> DownloadImageAsync(string url)
        {
            using var client = new HttpClient();
            var picture = await client.GetAsync(url).ConfigureAwait(false);

            await using var stream = await picture.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return new Bitmap(stream);
        }

        private static async Task SendMemeAsync(IMessage message, Image meme)
        {
            var stream = new MemoryStream();
            meme.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            
            await message.Channel.SendFileAsync(stream, "meme.jpeg", "готово").ConfigureAwait(false);
        }
    }
}