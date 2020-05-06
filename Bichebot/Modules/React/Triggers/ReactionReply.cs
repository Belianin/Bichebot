using System.Collections.Generic;
using Discord;

namespace Bichebot.Modules.React.Triggers
{
    public class ReactionReply
    {
        public string Text { get; }
        public ICollection<IEmote> Emotes { get; }

        public ReactionReply(string text, params IEmote[] emotes)
        {
            Text = text;
            Emotes = emotes;
        }
        
        public ReactionReply(string text)
        {
            Text = text;
        }

        public ReactionReply(params IEmote[] emotes)
        {
            Emotes = emotes;
        }
    }
}