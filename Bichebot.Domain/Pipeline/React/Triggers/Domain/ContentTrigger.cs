using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bichebot.Core;
using Bichebot.Core.Utilities;
using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers.Domain
{
    public class ContentTrigger : IReactionTrigger
    {
        private readonly IBotCore core;

        public ContentTrigger(IBotCore core)
        {
            this.core = core;
        }

        public bool IsNeedReaction(IMessage message)
        {
            if (message.Author.IsBot)
                return false;
            if (message.Attachments.Count > 0 || message.Content.Length >= 100)
                return true;

            return core.Random.Roll(message.Content.Length, 500);
        }

        public ReactionReply GetReply(IMessage message)
        {
            if (TryGetMessageEmotes(message.Content, out var emotes))
            {
                var emote = core.GetEmote(core.Random.Choose(emotes));
                if (emote != null)
                    return new ReactionReply(emote);
            }

            return new ReactionReply(core.Random.Choose(core.Guild.Emotes));
        }

        private bool TryGetMessageEmotes(string text, out ICollection<string> emotes)
        {
            var regex = new Regex(@":(.*?):");

            emotes = regex.Matches(text).Select(m => m.Value).ToArray();

            return emotes.Count > 0;
        }
    }
}