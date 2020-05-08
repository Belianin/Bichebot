using System;
using System.Collections.Generic;
using System.Linq;
using Bichebot.Utilities;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Bichebot.Modules.MemeGenerator
{
    public class MemeGenerator
    {
        private readonly FontFamily memeFont;
        private readonly int border = 8;
        private readonly string[] phrases;

        public MemeGenerator(IEnumerable<string> phrases)
        {
            this.phrases = phrases.ToArray();
            
            var fontCollection = new FontCollection();
            fontCollection.Install("Resources/lobster.ttf");
            memeFont = fontCollection.Families.First();
        }

        public Image GenerateMeme(Image image)
        {
            var phrase = new Random().Choose(phrases.ToArray());
            
            var font = new Font(memeFont, 40);
            var imageSize = image.Size();
            
            var size = TextMeasurer.Measure(phrase, new RendererOptions(font));

            var scalingFactor = Math.Min((imageSize.Width - border * 2) / size.Width, imageSize.Height / size.Height);
            var scaledFont = new Font(font, scalingFactor * font.Size);
            var scaledSize = TextMeasurer.Measure(phrase, new RendererOptions(scaledFont));

            var point = new PointF(imageSize.Width / 2, imageSize.Height - border - scaledSize.Height);
            var textGraphicOptions = new TextGraphicsOptions(true) {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            var shadowPoint = new PointF(point.X + scaledFont.Size / 20, point.Y + scaledFont.Size / 20);
            
            image.Mutate(i => i
                .DrawText(textGraphicOptions, phrase, scaledFont, Color.Black, shadowPoint)
                .DrawText(textGraphicOptions, phrase, scaledFont, Color.White, point));

            return image;
        }
    }
}