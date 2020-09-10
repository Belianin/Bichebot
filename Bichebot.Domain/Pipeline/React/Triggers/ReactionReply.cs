using System.Collections.Generic;
using System.Linq;
using Discord;

namespace Bichebot.Domain.Pipeline.React.Triggers
{
    public class ReactionReply
    {
        public ReactionReply(string text, params IEmote[] emotes)
        {
            Text = text;
            Emotes = emotes;
        }

        public ReactionReply(string text)
        {
            Text = text;
            Emotes = new IEmote[0];
        }

        public ReactionReply(params IEmote[] emotes)
        {
            Emotes = emotes;
        }

        public string Text { get; }
        public ICollection<IEmote> Emotes { get; set; } // временный set

        public override string ToString()
        {
            return $"text: '{Text ?? ""}'; emotes: [{string.Join(", ", Emotes.Select(e => e.Name))}]";
        }
    }
}