using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Bichebot.Modules.Best
{
    public class BestModule : BaseModule
    {
        private readonly HashSet<ulong> alreadyBest = new HashSet<ulong>();
        private readonly BestModuleSettings settings;
        private readonly IBankCore bank;
        
        public BestModule(IBotCore core, IBankCore bank, BestModuleSettings settings) : base(core)
        {
            this.settings = settings;
            this.bank = bank;
        }
        
        protected override async Task HandleReactionAsync(
            Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            var message = await channel.GetMessageAsync(reaction.MessageId).ConfigureAwait(false);
            
            if (message is RestUserMessage userMessage)
            {
                await SendToBestChannelAsync(userMessage).ConfigureAwait(false);
            }
        }

        private async Task SendToBestChannelAsync(IUserMessage userMessage)
        {
            if (alreadyBest.Contains(userMessage.Id) ||
                !settings.SourceChannelIds.Contains(userMessage.Channel.Id) ||
                !userMessage.Reactions.Values.Any(r => r.ReactionCount >= settings.ReactionCountToBeBest) ||
                userMessage.Channel.Id == settings.BestChannelId)
                return;
            
            alreadyBest.Add(userMessage.Id);

            var emotes = string.Join("", userMessage
                .Reactions
                .OrderByDescending(r => r.Value.ReactionCount)
                .SelectMany(e => Enumerable.Repeat(Core.ToEmojiString(e.Key.Name), e.Value.ReactionCount)));

            var description = userMessage.Content.StartsWith("https://")
                ? userMessage.Content
                : $"[оригинал]({userMessage.GetJumpUrl()})\n{emotes}";

            var embed = new EmbedBuilder()
                .WithAuthor(userMessage.Author)
                .WithTitle(userMessage.Content)
                .WithFooter("#бичехосты-лучшее")
                .WithDescription(description)
                .WithTimestamp(userMessage.Timestamp);

            if (userMessage.Attachments.Count > 0)
                embed.WithImageUrl(userMessage.Attachments.First().Url);

            await Core.Guild.GetTextChannel(settings.BestChannelId).SendMessageAsync(embed: embed.Build())
                .ConfigureAwait(false);

            bank.AddAsync(userMessage.Author.Id, settings.Reward);
        }
    }
}