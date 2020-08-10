using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using HtmlAgilityPack;
using Nexus.Core;

namespace Bichebot.Modules.FOnlineStatistics
{
    public class FOnlineStatisticsModule : BaseModule
    {
        private Dictionary<string, FoStatistics> currentStatistics;
        private readonly FonlineStatisticsModuleSettings settings;
        private bool wasFail = false;

        public FOnlineStatisticsModule(IBotCore core, FonlineStatisticsModuleSettings settings) : base(core)
        {
            this.settings = settings;
            Task.Run(StartPollingAsync).ConfigureAwait(false);
        }

        private async Task StartPollingAsync()
        {
            while (true)
            {
                await PollAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMinutes(5)).ConfigureAwait(false);
            }
        }
        
        private async Task PollAsync()
        {
            var newStats = GetStatistics();
            if (newStats.IsFail)
            {
                if (wasFail)
                    return;
                wasFail = true;
                await Core.Guild.GetTextChannel(settings.ChannelId).SendMessageAsync($"Мужики, помогите:\n{newStats}")
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

                var diffs = ShowDifferences(newStats.Value);
                if (diffs.Count > 0)
                {
                    var message = FormMessage(diffs);
                    await Core.Guild.GetTextChannel(settings.ChannelId).SendMessageAsync(message)
                        .ConfigureAwait(false);
                }
            }
        }

        private string FormMessage(List<StatisticsDiff> diffs)
        {
            var sb = new StringBuilder();
            sb.Append("Тем временем в пустоши...\n\n");
            foreach (var diff in diffs)
            {
                if (diff.IsNew)
                {
                    if (diff.Death == 0 && diff.Kills == 0)
                        sb.Append($"На пустоши появился новенький: {diff.Player}{FormMessageEndForNewbie(diff)}\n");
                }
                else
                {
                    if (diff.Death == 0)
                        sb.Append($"{diff.Player} убил {diff.Kills} человек\n");
                    else if (diff.Kills == 0)
                        sb.Append($"{diff.Player} слился {diff.Death} раз\n");
                    else
                        sb.Append($"{diff.Player} убил {diff.Kills} человек и умер {diff.Death} раз\n");
                }
            }

            return sb.ToString();
        }

        private string FormMessageEndForNewbie(StatisticsDiff diff)
        {
            if (diff.Death == 0 && diff.Kills == 0)
                return "";
            if (diff.Death == 0)
                return $" и даже успел кого-то убить ({diff.Kills} человек)";
            if (diff.Kills == 0)
                return $" и уже успел слиться (об {diff.Death} человек)";
            return $"и уже успел натворить дел {diff.Death} раз умер, {diff.Kills} убил";
        }

        private List<StatisticsDiff> ShowDifferences(IEnumerable<FoStatistics> newStats)
        {
            var diffs = new List<StatisticsDiff>();
            foreach (var stat in newStats)
            {
                if (!currentStatistics.TryGetValue(stat.Player, out var oldStat))
                    diffs.Add(stat.ToNew());
                else
                {
                    diffs.Add(new StatisticsDiff
                    {
                        Player = stat.Player,
                        Death = stat.Death - oldStat.Death,
                        Kills = stat.Kills - oldStat.Kills,
                        Rating = stat.Rating - oldStat.Rating
                    });
                }
            }

            return diffs;
        }

        private Result<IEnumerable<FoStatistics>> GetStatistics()
        {
            try
            {
                var url = "http://www.fallout-requiem.ru/main.php";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var table = doc.GetElementbyId("table");
                
                var rows = table.ChildNodes.Where(n => n.Name == "tr");

                return Result<IEnumerable<FoStatistics>>.Ok(rows.Select(ParsePlayer).ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private FoStatistics ParsePlayer(HtmlNode node)
        {
            var nodes = node.ChildNodes.Where(n => n.Name == "td").ToArray();
            return new FoStatistics
            {
                Rating = int.Parse(nodes[0].InnerText),
                Player = nodes[1].ChildNodes[0].InnerText,
                Kills = int.Parse(nodes[3].InnerText),
                Death = int.Parse(nodes[5].InnerText)
            };
        }
    }
}