using Bichebot.Core;
using Bichebot.Settings;

namespace Bichebot
{
    public interface IBotFactory
    {
        public Bot Create(BotSettings settings);
    }
}