using Bichebot.Core;

namespace Bichebot
{
    public interface IBotFactory
    {
        public Bot Create(BotSettings settings);
    }
}