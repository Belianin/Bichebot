using System;
using System.Collections.Generic;
using System.Linq;
using Bichebot.Core.Utilities;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Bichebot.Domain.Pipeline.MemeGenerator
{
    public class MemeGenerator
    {
        private readonly int border = 8;
        private readonly FontFamily memeFont;
        private readonly string[] phrases;
        private readonly Dictionary<string, int> usage;

        public MemeGenerator(IEnumerable<string> phrases)
        {
            this.phrases = phrases.ToArray();
            usage = this.phrases.ToDictionary(p => p, _ => 1);

            var fontCollection = new FontCollection();
            fontCollection.Install("Resources/lobster.ttf");
            memeFont = fontCollection.Families.First();
        }

        public Image GenerateMeme(Image image, string customPhrase = null)
        {
            var phrase = customPhrase ?? new Random().Choose(phrases.ToArray(), p => 1d / usage[p]);
            if (customPhrase == null)
                usage[phrase]++;

            var font = new Font(memeFont, 40);
            var imageSize = image.Size();

            var size = TextMeasurer.Measure(phrase, new RendererOptions(font));

            var scalingFactor = Math.Min((imageSize.Width - border * 2) / size.Width, imageSize.Height / size.Height);
            var scaledFont = new Font(font, scalingFactor * font.Size);
            var scaledSize = TextMeasurer.Measure(phrase, new RendererOptions(scaledFont));

            var point = new PointF(imageSize.Width / 2, imageSize.Height - border - scaledSize.Height);
            var textGraphicOptions = new TextGraphicsOptions(true)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var shadowPoint = new PointF(point.X + scaledFont.Size / 20, point.Y + scaledFont.Size / 20);

            image.Mutate(i => i
                .DrawText(textGraphicOptions, phrase, scaledFont, Color.Black, shadowPoint)
                .DrawText(textGraphicOptions, phrase, scaledFont, Color.White, point));

            return image;
        }
    }
}