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
            
            using var client = new HttpClient();
            var picture = await client.GetAsync(attachment.Url).ConfigureAwait(false);

            await using var stream = await picture.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var bitmap = new Bitmap(stream);

            var meme = generator.GenerateMeme(bitmap);
            
            meme.Save("meme.jpeg", ImageFormat.Jpeg);
            await message.Channel.SendFileAsync("meme.jpeg", "готово").ConfigureAwait(false);
            File.Delete("meme.jpeg");
        }
    }
}