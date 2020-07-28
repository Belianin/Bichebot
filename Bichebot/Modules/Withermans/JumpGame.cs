using System;
using System.Linq;
using System.Threading.Tasks;
using Bichebot.Banking;
using Bichebot.Core;
using Bichebot.Utilities;
using Discord;

namespace Bichebot.Modules.Withermans
{
    public class JumpGame
    {
        private readonly IBotCore core;
        private readonly IBankCore bank;
        private readonly ulong channelId;

        public JumpGame(IBotCore core, IBankCore bank, ulong channelId)
        {
            this.core = core;
            this.channelId = channelId;
            this.bank = bank;

            Task.Run(Play).ConfigureAwait(false);
        }

        public async Task Play()
        {
            while (true)
            {
                await Task.Delay(1000 * core.Random.Next(60, 60 * 48)).ConfigureAwait(false);
                await Run().ConfigureAwait(false);
            }
        }

        public async Task Run()
        {
            var channel = core.Guild.GetTextChannel(channelId);
            
            var smile = core.Random.Choose(core.Guild.Emotes);
            var timeLeft = TimeSpan.FromMinutes(1);
            var initialMessage = $"Срочно упарываемся и прыгаем!!! Ставь {smile} кто честный. Осталось: {timeLeft:g}";
            var sentMessage = await channel.SendMessageAsync(initialMessage).ConfigureAwait(false);

            do
            {
                var message = $"Срочно упарываемся и прыгаем!!! Ставь {smile} кто честный. Осталось: {timeLeft:g}";
                await sentMessage.ModifyAsync(m => m.Content = message).ConfigureAwait(false);

                var delay = TimeSpan.FromSeconds(5);
                
                await Task.Delay(delay).ConfigureAwait(false);
                timeLeft = timeLeft.Subtract(delay);
            } while (timeLeft >= TimeSpan.Zero);


            var finalMessage = await channel.GetMessageAsync(sentMessage.Id).ConfigureAwait(false);
            var winners = await finalMessage.GetReactionUsersAsync(smile, 100).Flatten().Where(u => !u.IsBot).ToListAsync();

            if (winners.Count == 0)
                await channel.SendMessageAsync($"Никто не прыгнул. ВСЕМ БАН{core.ToEmojiString("lejatbamboe")}")
                    .ConfigureAwait(false);
            if (winners.Count == 1)
            {
                var winner = winners.First();
                var winnerRoles = core.Guild.GetUser(winner.Id).Roles.Where(r => !r.IsEveryone);
                await bank.AddAsync(winner.Id, 50).ConfigureAwait(false);
                await channel
                    .SendMessageAsync(
                        $"Так держать {string.Join(" ", winnerRoles.Select(r => r.Name))} {winner.Username}. Лови джекпот 50 бичекоинов")
                    .ConfigureAwait(false);
            }
            else
            {
                await channel.SendMessageAsync($"Мужики, молодцы. {string.Join(", ", winners.Select(w => w.Username))}: всем по 25 бичекоинов")
                    .ConfigureAwait(false);
            }
        }
    }
}