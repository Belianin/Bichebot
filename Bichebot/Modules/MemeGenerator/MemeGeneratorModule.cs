using System.Drawing;
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
        private readonly HttpClient client = new HttpClient();
        private readonly MemeGeneratorModuleSettings settings;
        
        public MemeGeneratorModule(IBotCore core, MemeGeneratorModuleSettings settings) : base(core)
        {
            this.settings = settings;
        }

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (!message.Content.ToLower().Contains("сделай мем"))
                return;
            
            var attachments = message.Attachments.FirstOrDefault();
            if (attachments == null)
                return;
            

            var picture = await client.GetAsync(attachments.Url).ConfigureAwait(false);

            await using var stream = await picture.Content.ReadAsStreamAsync().ConfigureAwait(false);
            
            var bitmap = new Bitmap(stream);

            var graphics = Graphics.FromImage(bitmap);

            var phrase = Core.Random.Choose(settings.MemePhrases);
            graphics.DrawString(phrase, settings.Font, Brushes.Red, 0, bitmap.Height - settings.Font.Height * 2);

            //graphics.Save();
            
            bitmap.Save("meme.jpeg", ImageFormat.Jpeg);
            
            await message.Channel.SendFileAsync("meme.jpeg", "готово").ConfigureAwait(false);
            
            File.Delete("meme.jpeg");
        }
    }
}