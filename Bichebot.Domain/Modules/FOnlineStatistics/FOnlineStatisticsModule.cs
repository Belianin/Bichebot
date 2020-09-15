using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Core.Modules.Base;
using HtmlAgilityPack;
using Nexus.Core;

namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public class FOnlineStatisticsModule : BaseModule
    {
        private readonly FonlineStatisticsModuleSettings settings;
        private Dictionary<string, FoStatistics> currentStatistics;
        private bool wasFail;

        private DateTime currentDate = DateTime.Now;
        private Dictionary<ulong, Kda> kills = new Dictionary<ulong, Kda>();

        public FOnlineStatisticsModule(IBotCore core, FonlineStatisticsModuleSettings settings) : base(core)
        {
            this.settings = settings;
            Task.Run(StartPollingAsync).ConfigureAwait(false);
        }

        private async Task StartPollingAsync()
        {
            await Task.Delay(10 * 1000).ConfigureAwait(false); // ждем botcore guild
            while (true)
            {
                await PollAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMinutes(2)).ConfigureAwait(false);

                if (DateTime.Now.Date > currentDate.Date)
                {
                    currentDate = DateTime.Now;
                    await PayRewardsAsync().ConfigureAwait(false);
                }
            }
        }

        private async Task PayRewardsAsync()
        {
            try
            {
                var channel = Core.Guild.GetTextChannel(settings.RewardChannelId);

                var players = kills.Select(kda =>
                {
                    var sum = kda.Value.Kills * settings.PriceList.KillReward -
                              kda.Value.Deaths * settings.PriceList.DeathPenalty;

                    var name = channel.GetUser(kda.Key).Nickname;

                    return (sum, kda.Value.Kills, kda.Value.Deaths, name, kda.Key);
                }).ToArray();

                foreach (var player in players)
                {
                    await channel
                        .SendMessageAsync(
                            $"{player.name} убил {player.Kills} и умер {player.Deaths}. Счет: {player.sum}")
                        .ConfigureAwait(false);

                    if (player.sum > 0)
                        Core.Bank.Add(player.Key, player.sum);
                }

                kills = new Dictionary<ulong, Kda>();
            }
            catch (Exception e)
            {
                await Core.Guild.GetTextChannel(settings.ChannelId).SendMessageAsync(e.Message)
                    .ConfigureAwait(false);
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

        private string FormMessage(List<StatisticsDiff> diffs)
        {
            var sb = new StringBuilder();
            sb.Append("**Тем временем в пустоши...**\n");

            if (diffs.Count == 2 &&
                diffs.Any(d => d.Kills == 1 && d.Death == 0) &&
                diffs.Any(d => d.Kills == 0 && d.Death == 1))
            {
                var killer = diffs.First(d => d.Kills == 1);
                var victim = diffs.First(d => d.Death == 1);

                sb.Append($"**{killer.Player}** убил **{victim.Player}** подняв свой рейтинг на **{killer.Rating}**");
                
                if (settings.Color.TryGetValue(killer.Player, out var owner))
                {
                    if (!kills.ContainsKey(owner))
                        kills[owner] = new Kda();

                    kills[owner].Kills += killer.Kills;
                }
                
            }
            else
            {
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
                            sb.Append($"**{diff.Player}** убил **{diff.Kills}** человек\n");
                        else if (diff.Kills == 0)
                            sb.Append($"**{diff.Player}** слился **{diff.Death}** раз\n");
                        else
                            sb.Append($"**{diff.Player}** убил **{diff.Kills}** человек и умер **{diff.Death}** раз\n");
                    }

                    if (settings.Color.TryGetValue(diff.Player, out var owner))
                    {
                        if (!kills.ContainsKey(owner))
                            kills[owner] = new Kda();

                        kills[owner].Kills += diff.Kills;
                        kills[owner].Deaths += diff.Death;
                    }
                }
            }

            return sb.ToString();
        }

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