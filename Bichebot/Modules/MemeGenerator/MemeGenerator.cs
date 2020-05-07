using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using Bichebot.Utilities;

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
            
            var fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile("Resources/lobster.ttf");
            memeFont = fontCollection.Families.First();
        }

        public Image GenerateMeme(Image image)
        {
            var phrase = new Random().Choose(phrases.ToArray());
            
            var bitmap = new Bitmap(image);
            var graphics = Graphics.FromImage(bitmap);

            var font = new Font(memeFont, 40);//(bitmap.Width - border * 2) / (float)phrase.Length);
            var fontRectangle = graphics.MeasureString(phrase, font);
            
            font = new Font(font.FontFamily, (bitmap.Width / (fontRectangle.Width - border * 2)) * font.Size);
            fontRectangle = graphics.MeasureString(phrase, font);
            
            var textPoint = new PointF(
                (bitmap.Width / 2.0f - fontRectangle.Width / 2 + border), 
                bitmap.Height - (fontRectangle.Height + border));
            graphics.DrawString(phrase, font, Brushes.Black, textPoint.X + (font.Size / 10), textPoint.Y + (font.Size / 10));
            graphics.DrawString(phrase, font, Brushes.White, textPoint);

            graphics.Save();

            return bitmap;
        }
    }
}