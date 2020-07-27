namespace Bichebot
{
    public static class BotFactory
    {
        public static IBotFactory Instance => new DefaultBotFactory();
    }
}