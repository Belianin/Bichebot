using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Bichebot
{
    public class Bot
    {
        private SocketGuild Guild => discordClient.Guilds.First(g => g.Id == config.GuildId);

        private readonly BotConfig config;
        
        private readonly DiscordSocketClient discordClient;
        
        private readonly HashSet<ulong> alreadyBest = new HashSet<ulong>();
        
        private readonly Random rnd = new Random();
        
        public Bot(BotConfig config)
        {
            this.config = config;
            
            discordClient = new DiscordSocketClient();
            discordClient.ReactionAdded += HandleReactionAsync;
            discordClient.MessageReceived += HandleMessageAsync;
        }


        public async Task RunAsync(CancellationToken token)
        {
            Console.WriteLine("Starting");
            await discordClient.LoginAsync(TokenType.Bot, config.Token)
                .ConfigureAwait(false);
            await discordClient.StartAsync().ConfigureAwait(false);
            Console.WriteLine("Started");

            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
        }

        private async Task HandleMessageAsync(SocketMessage message)
        {
            Console.WriteLine(message.Content); //  /t4 7
            var args = Regex.Split(message.Content, @"\s+");
            
            if (args[0] == "/t4")
            {
                var days = 3;
                if (args.Length > 1)
                    int.TryParse(args[1], out days);
                
                var rates = GetEmoteReactionsRating(GetMessages(TimeSpan.FromDays(days), message.Channel))
                    .OrderByDescending(r => r.Count)
                    .Take(10);
                
                var response = $"Величайшие смайлы:\n{JoinEmoteStatistics(rates)}";

                await message.Channel.SendMessageAsync(response).ConfigureAwait(false);
                
            }
            else if (args[0] == "/t5")
            {
                var days = 3;
                if (args.Length > 1)
                    int.TryParse(args[1], out days);
                
                var rates = GetEmoteUsingsRating(GetMessages(TimeSpan.FromDays(days), message.Channel))
                    .OrderByDescending(r => r.Count)
                    .Take(10);
            
                var response = $"Величайшие смайлы:\n{JoinEmoteStatistics(rates)}";

                await message.Channel.SendMessageAsync(response).ConfigureAwait(false);
                
            }
            else if (message.Content.ToLowerCase().Contains("Бот" && ("игра" || "гам" || " в ") && "суприм"))
                {
                
                string[] Sugg = {"Советую", "Предлагаю", "Попробуй", "Го", "А может", "Как насчет", "Мб"};

                string[][] Map = new string[5][];
                Map[0] = new string[] {"на дуалгепе", "дуалгеп", "поиграть дуалгеп", "DualGap"};
                Map[1] = new string[] {"на астрократере", "астрократер", "поиграть астрократер", "на кратере", "кратер", "гантельку", "на гантельке", "на ПРОЦИКЛОНЕ"};
                Map[2] = new string[] {"на сетоне", "сетон", "поиграть сетон", "СЕТОООН", "СЕТОООООООН"};
                Map[3] = new string[] {"канис (или ченить такое)", "пирамиду (или подобную)", "норм карту", "на норм карте", "хилли плато", "поиграть норм карту", "на какой-нибудь норм карте"};
                Map[4] = new string[] {"юшелнотпас", "на юшелнотпасе", "поиграть юшелнотпас", "ну эту карту где ноу аир", "youshallnotpass", "8vs8 YouShallNotPass", "ULTINATE you shall not pass"};

                string[][] Tactics = new string[6][];

                Tactics[0] = new string[] {
                    $"c фаст ЯДЕРКОЙ{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}", 
                    $"мид (спутники{ToEmojiString("qwirchamp")}, толстяки{ToEmojiString("dobrobamboe")}{ToEmojiString("ocean")}",
                    $"на аире (БОМБИИМ{ToEmojiString("oroobamboe")}), главное вовремя перейди в {ToEmojiString("t3")} {ToEmojiString("dobrobamboe")}",
                    $"не важно какой слот, главное в конце не забудь телесакушки{ToEmojiString("vasyanbamboe")}",
                    $"не важно какой слот, главное в конце ЭОН телесакушки{ToEmojiString("vasyanbamboe")} (ТЕЛЕПОРТ КОЛОССА{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")})",
                    $"и снайп корсарами НЫЫААААА{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}",
                    $"на эко фаст {ToEmojiString("t3")} арта {ToEmojiString("dobrobamboe")}{ToEmojiString("thumbup")}",
                    $"и ТОПИТЬ за навального {ToEmojiString("coolstory")} (ВЗОРВЕМ либерах {ToEmojiString("deadinside")})",
                    $"c фаст телемазером {ToEmojiString("kripotabamboe")}{ToEmojiString("point_right")}{ToEmojiString("ok_hand")}",
                    $"c дропом двух комов в тылы противника {ToEmojiString("coolstory")}{ToEmojiString("deadinside")}",
                    $"с дропом кома с клоакой+лазер {ToEmojiString("qwirchamp")}",
                    $"с дропом ПАУКА {ToEmojiString("valera")}{ToEmojiString("oroobamboe")}",
                    $"мид за серафим ИТОТА{ToEmojiString("heart")}{ToEmojiString("lyabamboe")}",
                    $"с дропом рембо{ToEmojiString("bombitbamboe")} командира за серафим {ToEmojiString("supremebamboe")}",
                    $"с ТРЕМЯ фаст ЯДЕРКАМИ {ToEmojiString("oroobamboe")}{ToEmojiString("oroobamboe")}{ToEmojiString("oroobamboe")}",
                };
                Tactics[1] = new string[] {
                    $"раш командирами с пушкой {ToEmojiString("coolstory")}{ToEmojiString("deadinside")}{ToEmojiString("vasyanbamboe")}{ToEmojiString("oldbamboe")} с блокпостом + Т2 арта",
                    $"раш командирами с пушкой {ToEmojiString("coolstory")}{ToEmojiString("deadinside")}{ToEmojiString("vasyanbamboe")}{ToEmojiString("oldbamboe")} ИДЕМ ДО КОНЦА{ToEmojiString("oroobamboe")}{ToEmojiString("oroobamboe")}",
                    $"с гетто ганшипом {ToEmojiString("qwirchamp")}{ToEmojiString("call_me_tone1")}",
                    $"с фаст рембо командиром{ToEmojiString("supremebamboe")}"
                };
                Tactics[2] = new string[] {
                    $"на аир позиции. ТРЕНИРУЙСЯ {ToEmojiString("supremebamboe")}{ToEmojiString("lejatbamboe")}",
                    $"на позиции ROCK{ToEmojiString("shark")}, захватывай остров (так лееень((()()",
                    $"на позиции пляж{ToEmojiString("cup_with_straw")}{ToEmojiString("lootecbamboe")}",
                    $"мид РЕКЛЕЙМИ ВОЮЙ СПАМЬ  УБИТЬ           УБИВААЙ {ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}"
                };
                Tactics[3] = new string[] {
                    $"давай не сцы надо поднимать скилл уже {ToEmojiString("qwirnbamboe")}",
                    $"ВПЕРЕЕЕД на фронт главное чтобы не забанили за Т1 спам {ToEmojiString("hitlerbamboe")}",
                    $"",
                    $"",
                    $"",
                    $"",
                };
                Tactics[4] = new string[] {
                    $"c фаст ЯДЕРКОЙ{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}", 
                    $"за кебран с фаст пауком{ToEmojiString("lootecbamboe")}",
                    $"с тактикой на двоих - ФАСТ {ToEmojiString("t3")} АРТА {ToEmojiString("dobrobamboe")}",
                    $"с нагиб тактикой на четверых - ЯДЕРКА{ToEmojiString("radioactive")}+ПАУК{ToEmojiString("lootecbamboe")}+ИТОТА{ToEmojiString("bombitbamboe")}+FATBOY{ToEmojiString("oldbamboe")}",
                    $"просто почилить главное не забудь антинюку и турели к 15 минуте {ToEmojiString("spongebamboe")}"
                };
                Tactics[5] = new string[] {
                    $"2vs2 или 3vs3 НАГИИБ (слив) {ToEmojiString("supremebamboe")}",
                    $"",
                    $"",
                    $"",
                    $"",
                };

                var tact = rnd.Next(0, Tactics.Select(a => a.Length).Sum());

                var T1 = Tactics[0].Length;
                var T2 = Tactics[0].Length+Tactics[1].Length;
                var T3 = Tactics[0].Length+Tactics[1].Length+Tactics[2].Length;
                var T4 = Tactics[0].Length+Tactics[1].Length+Tactics[2].Length+Tactics[3].Length;
                var T5 = Tactics[0].Length+Tactics[1].Length+Tactics[2].Length+Tactics[3].Length+Tactics[4].Length;

                var MSG = tact switch
                    {
                        var x when (x>=0 && x<=T1) => $"{Map[0][tact]} {Tactics[0][tact]}",

                        var x when (x>T1 && x<=T2) => $"{Map[1][tact-T1]} {Tactics[1][tact-T1]}",
                        var x when (x>T2 && x<=T3) => $"{Map[2][tact-T2]} {Tactics[1][tact-T2]}",
                        var x when (x>T3 && x<=T4) => $"{Map[3][tact-T3]} {Tactics[1][tact-T3]}",
                        var x when (x>T4 && x<=T5) => $"{Map[4][tact-T4]} {Tactics[1][tact-T4]}",
                        var x when (x>T5) => $"{Map[5][tact-T5]} {Tactics[1][tact-T5]}",
                    };
               
                    await message.Channel.SendMessageAsync(
                    $"{Sugg[rnd.Next(0, Sugg.Length-1)]} {MSG}")
                     .ConfigureAwait(false);               
            }
            else if (message.Content.Contains("лол"))
            {
                if (rnd.Next(0, 100) > 50)
                {
                    await message.Channel.SendMessageAsync(
                            $"{ToEmojiString("dobrobamboe")} может лучше в {ToEmojiString("supremebamboe")}?")
                        .ConfigureAwait(false);
                }
                else if (rnd.Next(0, 100) > 50)
                {
                    await message.Channel.SendMessageAsync(
                            $"{ToEmojiString("dobrobamboe")} ты хотел сказать {ToEmojiString("supremebamboe")}?")
                        .ConfigureAwait(false);
                }
                else
                {
                    await message.Channel.SendMessageAsync(
                            $"{ToEmojiString("valera")} ну лан")
                        .ConfigureAwait(false);
                }
            }
            else
            {
                if (rnd.Next(0, 1000) == 999)
                {
                    await message.Channel.SendMessageAsync("Скатился...").ConfigureAwait(false);
                }
            }
        }

        private string JoinEmoteStatistics(IEnumerable<Statistic> statistics)
        {
            return string.Join("\n", statistics.Select(e => $"{ToEmojiString(e.Value)}: {e.Count}"));
        }

        private static IEnumerable<IUserMessage> GetMessages(TimeSpan period, IMessageChannel channel)
        {
            var timestamp = DateTime.UtcNow;
            ulong lastId = 0;
            
            var messages = channel.GetMessagesAsync().Flatten();
            var enumerator = messages.GetEnumerator();
            var isEmpty = true;
            do
            {
                while (enumerator.MoveNext().Result)
                {
                    isEmpty = false;
                    lastId = enumerator.Current.Id;
                    if (enumerator.Current.Timestamp.UtcDateTime < timestamp)
                        timestamp = enumerator.Current.Timestamp.UtcDateTime;
                    if (enumerator.Current is IUserMessage userMessage)
                        yield return userMessage;
                }

                Console.WriteLine(timestamp);

                enumerator = channel.GetMessagesAsync(lastId, Direction.Before).Flatten().GetEnumerator();
            } while (!isEmpty && timestamp.Add(period) > DateTime.UtcNow);
        }

        private static IEnumerable<Statistic> GetEmoteUsingsRating(IEnumerable<IUserMessage> messages)
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

        private async Task HandleReactionAsync(
            Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            Console.WriteLine("Reaction");
            var message = await channel.GetMessageAsync(reaction.MessageId).ConfigureAwait(false);
            
            if (message is RestUserMessage userMessage)
            {
                await SendToBestChannelAsync(userMessage).ConfigureAwait(false);
            }
        }

        private async Task SendToBestChannelAsync(IUserMessage userMessage)
        {
            if (alreadyBest.Contains(userMessage.Id) ||
                !userMessage.Reactions.Values.Any(r => r.ReactionCount >= config.ReactionCountToBeBest) ||
                userMessage.Channel.Id == config.BestChannelId)
                return;
            
            alreadyBest.Add(userMessage.Id);

            var emotes = string.Join("", userMessage
                .Reactions
                .OrderByDescending(r => r.Value.ReactionCount)
                .SelectMany(e => Enumerable.Repeat(ToEmojiString(e.Key.Name), e.Value.ReactionCount)));

            var embed = new EmbedBuilder()
                .WithAuthor(userMessage.Author)
                .WithTitle(userMessage.Content)
                .WithFooter("#бичехосты-лучшее")
                .WithDescription(emotes)
                .WithTimestamp(userMessage.Timestamp);

            if (userMessage.Attachments.Count > 0)
                embed.WithImageUrl(userMessage.Attachments.First().Url);

            await Guild.GetTextChannel(config.BestChannelId).SendMessageAsync(embed: embed.Build())
                .ConfigureAwait(false);
        }

        private string ToEmojiString(string text)
        {
            var emote = Guild.Emotes.FirstOrDefault(e => e.Name == text);
            if (emote == null)
                return text;
            
            return $"<:{emote.Name}:{emote.Id}>";
        }
    }
}