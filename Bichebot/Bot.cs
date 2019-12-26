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
            var msg = new Message
            {
                DiscordMessage = message,
                Tts = message.Content.Contains("tts")
            };
            var discord = message.DiscordMessage.ToLower();

            if (args[0] == "/t4")
            {
                var days = 3;
                if (args.Length > 1)
                    int.TryParse(args[1], out days);
                
                var rates = GetEmoteReactionsRating(GetMessages(TimeSpan.FromDays(days), message.Channel))
                    .OrderByDescending(r => r.Count)
                    .Take(10);
                
                var response = $"Величайшие смайлы:\n{JoinEmoteStatistics(rates)}";

                await message.Channel.SendMessageAsync(response, msg.Tts).ConfigureAwait(false);
                
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

                await message.Channel.SendMessageAsync(response, msg.Tts).ConfigureAwait(false);
                
            }
            else if (IsSupremeAsked(message))
                await TrySupremeSuggestionAsync(msg).ConfigureAwait(false);
            else if (message.Content.Contains("лол"))
            {
                if (rnd.Next(0, 100) > 50)
                {
                    await discord.Channel.SendMessageAsync(
                            $"{discord.Author.Username}, {ToEmojiString("dobrobamboe")} может лучше в {ToEmojiString("supremebamboe")}?", msg.Tts)
                        .ConfigureAwait(false);
                }
                else if (rnd.Next(0, 100) > 50)
                {
                    await discord.Channel.SendMessageAsync(
                            $"{discord.Author.Username}, {ToEmojiString("dobrobamboe")} ты хотел сказать {ToEmojiString("supremebamboe")}?", msg.Tts)
                        .ConfigureAwait(false);
                }
                else
                {
                    await discord.Channel.SendMessageAsync(
                            $"{ToEmojiString("valera")} ну лан", msg.Tts)
                        .ConfigureAwait(false);
                }
            }
            else if (IsBotHello(message))
                {
                if (rnd.Next(0, 100) < 20)
                {
                    await discord.Channel.SendMessageAsync(
                            $"{discord.Author.Username} Здарова Бро {ToEmojiString("dobrobamboe")}", msg.Tts)
                        .ConfigureAwait(false);
                }
                else if (rnd.Next(0, 100) < 40)
                {
                    await discord.Channel.SendMessageAsync(
                            $"Привет {discord.Author.Username}", msg.Tts)
                        .ConfigureAwait(false);
                }
                else if (rnd.Next(0, 100) < 60)
                {
                    await discord.Channel.SendMessageAsync(
                            $"{discord.Author.Username}, я приветсвую тебя", msg.Tts)
                        .ConfigureAwait(false);
                }
                 else if (rnd.Next(0, 100) < 80)
                {
                    await discord.Channel.SendMessageAsync(
                            $"{discord.Author.Username} Добро пожаловать в Бухту Бичехостов", msg.Tts)
                        .ConfigureAwait(false);
                }
                 else
                {
                     
                    await discord.Channel.SendMessageAsync(
                            $"{discord.Author.Username} Как дела ?", msg.Tts)
                        .ConfigureAwait(false);
                }
            }
            else if (discord.Content.Contains("бот лох")) 
                await discord.Channel.SendMessageAsync($"{discord.Author.Username} Удали", msg.Tts).ConfigureAwait(false);
            
            else if (rnd.Next(0, 1000) == 999)
                await discord.Channel.SendMessageAsync("Скатился...", msg.Tts).ConfigureAwait(false);
        }
         private bool IsBotHello(IMessage message)
        {
            var lower = message.Content.ToLower();
            return lower.ContainsAny(new [] {"бот ", "бот,", " бот"}) &&
                   lower.ContainsAny(new[] {"привет", "здаров", " хай "});
        }
        private bool IsSupremeAsked(IMessage message)
        {
            var lower = message.Content.ToLower();
            return lower.ContainsAny(new [] {"бот ", "бот,"}) &&
                   lower.Contains("суприм") &&
                   lower.ContainsAny(new[] {"игра", "гам", " в ", "го "});
        }

        private async Task TrySupremeSuggestionAsync(Message message)
        {
            var sugg = new []{"Советую", "Предлагаю", "Попробуй", "Го", "А может", "Как насчет", "Мб"};

            var maps = new List<SupremeMap>
            {
                new SupremeMap
                {
                    Names = new[] {"на дуалгепе", "дуалгеп", "поиграть дуалгеп", "DualGap"},
                    Tactics = new[]
                    {
                        $"c фаст ЯДЕРКОЙ{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}",
                        $"мид (спутники{ToEmojiString("qwirchamp")}, толстяки{ToEmojiString("dobrobamboe")}{ToEmojiString("ocean")})",
                        $"на аире (БОМБИИМ{ToEmojiString("gif")}), главное вовремя перейди в {ToEmojiString("t3")} {ToEmojiString("dobrobamboe")}",
                        $"не важно какой слот, главное в конце не забудь телесакушки{ToEmojiString("vasyanbamboe")}",
                        $"не важно какой слот, главное в конце ЭОН телесакушки{ToEmojiString("vasyanbamboe")} (ТЕЛЕПОРТ КОЛОССА{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")})",
                        $"и снайп корсарами НЫЫААААА{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}",
                        $"на эко фаст {ToEmojiString("t3")} арта {ToEmojiString("dobrobamboe")}{ToEmojiString("thumbup")}",
                        $"и ТОПИТЬ за навального {ToEmojiString("coolstory")} (ВЗОРВЕМ либерах {ToEmojiString("deadinside")})",
                        $"c фаст телемазером {ToEmojiString("kripotabamboe")}{ToEmojiString("point_right")}{ToEmojiString("ok_hand")}",
                        $"c дропом двух комов в тылы противника {ToEmojiString("coolstory")}{ToEmojiString("deadinside")}",
                        $"с дропом кома с клоакой+лазер {ToEmojiString("qwirchamp")}",
                        $"с дропом ПАУКА {ToEmojiString("valera")}{ToEmojiString("gif")}",
                        $"мид за серафим ИТОТА{ToEmojiString("heart")}{ToEmojiString("lyabamboe")}",
                        $"с дропом рембо{ToEmojiString("bombitbamboe")} командира за серафим {ToEmojiString("supremebamboe")}",
                        $"с ТРЕМЯ фаст ЯДЕРКАМИ {ToEmojiString("gif")}{ToEmojiString("gif")}{ToEmojiString("gif")}",
                        $"и твори НАГИБ с АИРА фаст бомберами{ToEmojiString("gif")} + {ToEmojiString("t3")} ASF",
                        $"с фаст{ToEmojiString("supremebamboe")} АХВАААССООЙЙ{ToEmojiString("supremebamboe")}"
                    }
                },
                new SupremeMap
                {
                    Names = new[]
                    {
                        "на астрократере", "астрократер", "поиграть астрократер", "на кратере", "кратер", "гантельку",
                        "на гантельке", "на ПРОЦИКЛОНЕ"
                    },
                    Tactics = new[]
                    {
                        $"раш командирами с пушкой {ToEmojiString("coolstory")}{ToEmojiString("deadinside")}{ToEmojiString("vasyanbamboe")}{ToEmojiString("oldbamboe")} с блокпостом + Т2 арта",
                        $"раш командирами с пушкой {ToEmojiString("coolstory")}{ToEmojiString("deadinside")}{ToEmojiString("vasyanbamboe")}{ToEmojiString("oldbamboe")} ИДЕМ ДО КОНЦА{ToEmojiString("gif")}{ToEmojiString("gif")}",
                        $"с гетто ганшипом {ToEmojiString("qwirchamp")}{ToEmojiString("call_me_tone1")}",
                        $"с фаст рембо командиром{ToEmojiString("supremebamboe")}"
                    }
                },
                new SupremeMap
                {
                    Names = new[] {"на сетоне", "сетон", "поиграть сетон", "СЕТОООН", "СЕТОООООООН"},
                    Tactics = new[]
                    {
                        $"на аир позиции. ТРЕНИРУЙСЯ {ToEmojiString("supremebamboe")}{ToEmojiString("lejatbamboe")}",
                        $"на позиции ROCK{ToEmojiString("shark")}, захватывай остров (так лееень((()()",
                        $"на позиции пляж{ToEmojiString("cup_with_straw")}{ToEmojiString("lootecbamboe")}",
                        $"мид РЕКЛЕЙМИ ВОЮЙ СПАМЬ  УБИТЬ           УБИВААЙ {ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}"
                    }
                },
                new SupremeMap
                {
                    Names = new[]
                    {
                        "канис (или ченить такое)", "пирамиду (или подобную)", "норм карту", "поиграть на норм карте",
                        "хилли плато",
                        "поиграть норм карту", "на какой-нибудь норм карте"
                    },
                    Tactics = new[]
                    {
                        $"давай не сцы надо поднимать скилл уже {ToEmojiString("qwirbamboe")}",
                        $"ВПЕРЕЕЕД на фронт главное чтобы не забанили за Т1 спам {ToEmojiString("hitlerbamboe")}",
                        $"там хоть научишься играть в настоящий суприм{ToEmojiString("supremebamboe")}",
                        $", а лучше на Big Bang Lake супер карта и очень динамичная{ToEmojiString("jet")}",
                        $"ведь намного веселее когда бой идет постоянно, надоели геймендеры {ToEmojiString("qwirbamboe")}",
                        $", а то разве не задолбало 10 минут смотреть на экстракторы?{ToEmojiString("newfagbamboe")}",
                        $"ПОРА НА ФРОНТ{ToEmojiString("hitlerbamboe")}{ToEmojiString("raised_hand_tone5")}"
                    }
                },
                new SupremeMap
                {
                    Names = new[]
                    {
                        "юшелнотпас", "на юшелнотпасе", "поиграть юшелнотпас", "ну эту карту где ноу аир",
                        "youshallnotpass",
                        "8vs8 YouShallNotPass", "ULTINATE you shall not pass"
                    },
                    Tactics = new[]
                    {
                        $"c фаст ЯДЕРКОЙ{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}{ToEmojiString("lejatbamboe")}",
                        $"за кебран с фаст пауком{ToEmojiString("lootecbamboe")}",
                        $"с тактикой на двоих - ФАСТ {ToEmojiString("t3")} АРТА {ToEmojiString("dobrobamboe")}",
                        $"с нагиб тактикой на четверых - ЯДЕРКА{ToEmojiString("radioactive")}+ПАУК{ToEmojiString("lootecbamboe")}+ИТОТА{ToEmojiString("bombitbamboe")}+FATBOY{ToEmojiString("oldbamboe")}",
                        $"просто почилить главное не забудь антинюку и турели к 15 минуте {ToEmojiString("spongebamboe")}",
                        $"и забань {ToEmojiString("t3")}радар потом го распиливать с клоакой {ToEmojiString("qwirbamboe")}"
                    },
                },
                new SupremeMap
                {
                    Names = new[] {string.Empty},
                    Tactics = new[]
                    {
                        $"2vs2 или 3vs3 НАГИИБ (слив) {ToEmojiString("supremebamboe")}",
                        $"поиграть ладдер (no greys{ToEmojiString("face_with_monocle")})",
                        $"сыграть уже в ладдер че как нуб серый{ToEmojiString("qwirchamp")}",
                        $"Ctrl+K и в лол{ToEmojiString("lol2")}",
                        $"смотреть каст с Сидом играть как-то лень"
                    }
                }
            };

            var map = maps.Random(rnd, m => m.Tactics.Count);
            var discord = message.DiscordMessage;
            await discord.Channel.SendMessageAsync(
                    $"{discord.Author.Username}, {sugg.Random(rnd)} {map.Names.Random(rnd)} {map.Tactics.Random(rnd)}",
                    message.Tts)
                .ConfigureAwait(false);
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
            Console.WriteLine(ReplaceEmojiStrings("c фаст ЯДЕРКОЙ:lejatbamboe::lejatbamboe::lejatbamboe:"));
            
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

        private string ReplaceEmojiStrings(string text)
        {
            return Regex.Replace(text, @":.*?:", x => ToEmojiString(x.Value.Substring(1, x.Value.Length - 2)));
        }

        private string ToEmojiString(string text)
        {
            var emote = Guild.Emotes.FirstOrDefault(e => e.Name == text);
            if (emote == null)
                return $":{text}:";
            
            return emote.Animated ? $"<a:{emote.Name}:{emote.Id}>" : $"<:{emote.Name}:{emote.Id}>";
        }
    }
}