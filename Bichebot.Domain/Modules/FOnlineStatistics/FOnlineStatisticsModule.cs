using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;
using Bichebot.Core;
using Bichebot.Core.Modules.Base;
using Discord.WebSocket;
using HtmlAgilityPack;
using Nexus.Core;

namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public class FOnlineStatisticsModule : BaseModule
    {
        private readonly FonlineStatisticsModuleSettings settings;
        private Dictionary<string, FoStatistics> currentStatistics;
        private readonly IStatisticsProvider statisticsProvider;
        private bool wasFail;
        
        public FOnlineStatisticsModule(
            IBotCore core,
            FonlineStatisticsModuleSettings settings) : base(core)
        {
            this.settings = settings;
            this.statisticsProvider = new StatisticsProvider();
        }

        protected override async Task DoWorkAsync(CancellationToken token)
        {
            await Task.Delay(10 * 1000, token).ConfigureAwait(false); // ждем botcore guild
            while (!token.IsCancellationRequested)
            {
                await PollAsync().ConfigureAwait(false);

                await Task.Delay(TimeSpan.FromMinutes(2), token).ConfigureAwait(false);
            }
        }

        private async Task PollAsync()
        {
            var newStats = statisticsProvider.GetTotalStatistics();
            if (newStats.IsFail)
            {
                if (wasFail)
                    return;
                wasFail = true;
                await Core.Guild.GetTextChannel(settings.ChannelId).SendMessageAsync($"Мужики, помогите. Не получилось :\n{newStats}")
                    .ConfigureAwait(false);
            }
            else
            {
                wasFail = false;
                if (currentStatistics == null)
                {
                    currentStatistics = newStats.Value.ToDictionary(k => k.Player);
                    return;
                }

                var diffs = ShowDifferences(newStats.Value)
                    .Where(s => s.Kills > 0)
                    .Select(s => (statisticsProvider.GetCharacterStatistics(s.Link).Value, s.Kills)) // check IsFail
                    .ToArray();
                if (diffs.Length > 0)
                    try
                    {
                        var message = FormMessage(diffs);
                        foreach (var batch in SplitMessage(message, 1600))
                            await Core.Guild.GetTextChannel(settings.ChannelId).SendMessageAsync(batch)
                                .ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        await Core.Guild.GetTextChannel(settings.ChannelId).SendMessageAsync(e.Message)
                            .ConfigureAwait(false);
                    }

                currentStatistics = newStats.Value.ToDictionary(k => k.Player);
            }
        }

        private IEnumerable<string> SplitMessage(string message, int batchLength)
        {
            for (var i = 0; i < message.Length; i += batchLength)
                if (message.Length <= batchLength * (i + 1))
                    yield return message.Substring(i);
                else
                    yield return message.Substring(i, i + batchLength);
        }

        private string FormMessage(ICollection<(CharacterStatistics, int)> diffs)
        {
            var sb = new StringBuilder();
            sb.Append($"**Тем временем в пустоши...**\n");

            foreach (var diff in diffs)
            {
                sb.Append($"**{diff.Item1.Name}** убил **{string.Join(", ", diff.Item1.Kills.Take(diff.Item2).Select(n => n.Name))}**\n");
            }

            return sb.ToString();
        }

        private List<StatisticsDiff> ShowDifferences(IEnumerable<FoStatistics> newStats)
        {
            var diffs = new List<StatisticsDiff>();
            foreach (var stat in newStats)
                if (!currentStatistics.TryGetValue(stat.Player, out var oldStat))
                {
                    diffs.Add(stat.ToNew());
                }
                else
                {
                    var diff = new StatisticsDiff
                    {
                        Link = stat.Link,
                        Player = stat.Player,
                        Death = stat.Death - oldStat.Death,
                        Kills = stat.Kills - oldStat.Kills,
                        Rating = stat.Rating - oldStat.Rating
                    };

                    if (diff.Death != 0 || diff.Kills != 0)
                        diffs.Add(diff);
                }

            return diffs;
        }

        // unused
        private string FormMessageEndForNewbie(StatisticsDiff diff)
        {
            if (diff.Death == 0 && diff.Kills == 0)
                return "";
            if (diff.Death == 0)
                return $" и даже успел кого-то **{diff.Kills}** человек";
            if (diff.Kills == 0)
                return $" и уже успел слиться **{diff.Death}** раз";
            return $"и уже успел натворить дел: **{diff.Death}** раз умер, убил **{diff.Kills}** человек";
        }
    }
}