using System.Linq;
using Discord;

namespace Bichebot.Core
{
    public static class BotCoreExtensions
    {
        public static IEmote GetEmote(this IBotCore core, string name)
        {
            return core.Guild.Emotes.FirstOrDefault(e => e.Name == name);
        }
    }
}