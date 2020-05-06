using System.Collections.Generic;
using System.Drawing;

namespace Bichebot.Modules.MemeGenerator
{
    public class MemeGeneratorModuleSettings
    {
        public Font Font { get; set; } = new Font(FontFamily.GenericMonospace, 32, FontStyle.Regular);

        public string[] MemePhrases { get; set; } = 
        {
            "го в доту",
            "опять не выпала дисперсия",
            "закрытие бичехостов",
            "тест мем плиз игнор",
        };
    }
}