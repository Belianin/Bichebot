using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Core.Pipeline;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Domain.Pipeline.Statistics
{
    public class StatisticsHandler : IMessageHandler
    {
        private readonly IBotCore core;
        private readonly TimeSpan defaultSearchPeriod = TimeSpan.FromDays(7);

        private readonly Dictionary<string, Func<IEnumerable<IUserMessage>, IEnumerable<Statistic>>>
            statisticsFunctions;

        public StatisticsHandler(IBotCore core)
        {
            this.core = core;
            statisticsFunctions = new Dictionary<string, Func<IEnumerable<IUserMessage>, IEnumerable<Statistic>>>
            {
                {"/t4", GetEmoteReactionsRating},
                {"/t5", GetEmoteUsageRating}
            };
        }

        public async Task<bool> HandleAsync(SocketMessage message)
        {
            var args = Regex.Split(message.Content, @"\s+");
            if (!statisticsFunctions.TryGetValue(args[0], out var statisticsFunction))
                return false;

            var searchPeriod = defaultSearchPeriod;
            if (args.Length > 1 && int.TryParse(args[1], out var days))
                searchPeriod = TimeSpan.FromDays(days);

            var messages = core.GetMessages(message.Channel, searchPeriod);

            var rates = statisticsFunction(messages)
                .OrderByDescending(r => r.Count)
                .Take(10);

            var response = $"Величайшие смайлы:\n{JoinEmoteStatistics(rates)}";

            await message.Channel.SendMessageAsync(response).ConfigureAwait(false);

            return true;
        }

        private string JoinEmoteStatistics(IEnumerable<Statistic> statistics)
        {
            return string.Join("\n", statistics.Select(e => $"{core.ToEmojiString(e.Value)}: {e.Count}"));
        }

        private static IEnumerable<Statistic> GetEmoteUsageRating(IEnumerable<IUserMessage> messages)
        {
            var result = new Dictionary<string, int>();
            var regex = new Regex(@":(.*?):");

            var emotes = messages
                .Where(m => !m.Author.IsBot)
                .SelectMany(m => regex.Matches(m.Content));

            foreach (var emote in emotes)
            {
                var value = emote.ToString().Substring(1, emote.ToString().Length - 2);
                if (!result.ContainsKey(value))
                    result[value] = 0;

                result[value] += 1;
            }

            return result.Select(v => new Statistic
            {
                Count = v.Value,
                Value = v.Key
            });
        }

        private static IEnumerable<Statistic> GetEmoteReactionsRating(IEnumerable<IUserMessage> messages)
        {
            var result = new Dictionary<string, int>();

            foreach (var reaction in messages.SelectMany(m => m.Reactions))
            {
                if (!result.ContainsKey(reaction.Key.Name))
                    result[reaction.Key.Name] = 0;

                result[reaction.Key.Name] += reaction.Value.ReactionCount;
            }

            return result.Select(v => new Statistic
            {
                Count = v.Value,
                Value = v.Key
            });
        }
    }
}