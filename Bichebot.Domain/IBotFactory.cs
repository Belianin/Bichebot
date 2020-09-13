using Bichebot.Core;

namespace Bichebot.Domain
{
    public interface IBotFactory
    {
        public Bot Create(BotSettings settings);
    }
}