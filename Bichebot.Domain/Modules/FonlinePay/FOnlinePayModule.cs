using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Core.Modules.Base;

namespace Bichebot.Domain.Modules.FonlinePay
{
    // unused
    public class FOnlinePayModule : BaseModule
    {
        private FOnlinePayModuleSettings settings;
        private Dictionary<ulong, Kda> kills = new Dictionary<ulong, Kda>();
        public FOnlinePayModule(IBotCore core, FOnlinePayModuleSettings settings) : base(core)
        {
            this.settings = settings;
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
                await Core.Guild.GetTextChannel(settings.RewardChannelId).SendMessageAsync(e.Message)
                    .ConfigureAwait(false);
            }
        }
    }
}