using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules;
using Bichebot.Modules.Best;
using Bichebot.Modules.Statistics;
using Bichebot.Utilities;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Bichebot
{
    public class Bot
    {
        private readonly IBotCore core;

        private readonly AudioSpeaker audio;

        private readonly ICollection<IBotModule> modules;

        private readonly BotConfig config;

        private readonly Random rnd = new Random();

        public Bot(BotConfig config)
        {
            this.config = config;
            var discordClient = new DiscordSocketClient();
            
            core = new BotCore(config.GuildId, discordClient);
            audio = new AudioSpeaker(core);
            modules = new List<IBotModule>
            {
                new StatisticsModule(core),
                new BestModule(core, new BestModuleSettings
                {
                    BestChannelId = config.BestChannelId,
                    ReactionCountToBeBest = config.ReactionCountToBeBest
                })
            };
            
            foreach (var module in modules)
                module.Run();

            discordClient.MessageReceived += HandleMessageAsync;
        }

        public async Task RunAsync(CancellationToken token)
        {
            Console.WriteLine("Starting");
            await core.Client.LoginAsync(TokenType.Bot, config.Token).ConfigureAwait(false);
            await core.Client.StartAsync().ConfigureAwait(false);
            Console.WriteLine("Started");

            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
        }

        private async Task HandleMessageAsync(SocketMessage message)
        {
            Console.WriteLine(message.Content); //  /t4 7
            var msg = new Message
            {
                DiscordMessage = message,
                Tts = message.Content.Contains("tts")
            };

            if (message.Content == "go")
            {
                audio.Connect(message.Author);
            }
            
            if (message.Content == "ilidan")
            {
                await audio.SendMessageAsync("ilidan.mp3").ConfigureAwait(false);
            }

            if (message is IUserMessage userMessage)
            {
                if (IsDeservingLike(message, out var reaction))
                    await userMessage.AddReactionAsync(core.Guild.Emotes.First(n => n.Name == reaction)).ConfigureAwait(false);

                if (rnd.Next(0, 100) > 90)
                    await userMessage.AddReactionAsync(core.Guild.Emotes.RandomReadonly(rnd)).ConfigureAwait(false);
            }
            
            if (message.Content.Contains("бот") && message.Content.Contains("удали"))
            {
                await DeletePreviousMessageAsync(message).ConfigureAwait(false);
            }
            else if (IsSupremeAsked(message))
                await TrySupremeSuggestionAsync(msg).ConfigureAwait(false);
            else if (message.Content.Contains("лол"))
            {
                if (rnd.Next(0, 100) > 50)
                {
                    await message.Channel.SendMessageAsync(
                            $"{core.ToEmojiString("dobrobamboe")} может лучше в {core.ToEmojiString("supremebamboe")}?", msg.Tts)
                        .ConfigureAwait(false);
                }
                else if (rnd.Next(0, 100) > 50)
                {
                    await message.Channel.SendMessageAsync(
                            $"{core.ToEmojiString("dobrobamboe")} ты хотел сказать {core.ToEmojiString("supremebamboe")}?", msg.Tts)
                        .ConfigureAwait(false);
                }
                else
                {
                    await message.Channel.SendMessageAsync(
                            $"{core.ToEmojiString("valera")} ну лан", msg.Tts)
                        .ConfigureAwait(false);
                }
            }
            else
            {
                if (rnd.Next(0, 1000) == 999)
                {
                    await message.Channel.SendMessageAsync("Скатился...", msg.Tts).ConfigureAwait(false);
                }
            }
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
                        $"c фаст ЯДЕРКОЙ{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}",
                        $"мид (спутники{core.ToEmojiString("qwirchamp")}, толстяки{core.ToEmojiString("dobrobamboe")}{core.ToEmojiString("ocean")}",
                        $"на аире (БОМБИИМ{core.ToEmojiString("oroobamboe")}), главное вовремя перейди в {core.ToEmojiString("t3")} {core.ToEmojiString("dobrobamboe")}",
                        $"не важно какой слот, главное в конце не забудь телесакушки{core.ToEmojiString("vasyanbamboe")}",
                        $"не важно какой слот, главное в конце ЭОН телесакушки{core.ToEmojiString("vasyanbamboe")} (ТЕЛЕПОРТ КОЛОССА{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")})",
                        $"и снайп корсарами НЫЫААААА{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}",
                        $"на эко фаст {core.ToEmojiString("t3")} арта {core.ToEmojiString("dobrobamboe")}{core.ToEmojiString("thumbup")}",
                        $"и ТОПИТЬ за навального {core.ToEmojiString("coolstory")} (ВЗОРВЕМ либерах {core.ToEmojiString("deadinside")})",
                        $"c фаст телемазером {core.ToEmojiString("kripotabamboe")}{core.ToEmojiString("point_right")}{core.ToEmojiString("ok_hand")}",
                        $"c дропом двух комов в тылы противника {core.ToEmojiString("coolstory")}{core.ToEmojiString("deadinside")}",
                        $"с дропом кома с клоакой+лазер {core.ToEmojiString("qwirchamp")}",
                        $"с дропом ПАУКА {core.ToEmojiString("valera")}{core.ToEmojiString("oroobamboe")}",
                        $"мид за серафим ИТОТА{core.ToEmojiString("heart")}{core.ToEmojiString("lyabamboe")}",
                        $"с дропом рембо{core.ToEmojiString("bombitbamboe")} командира за серафим {core.ToEmojiString("supremebamboe")}",
                        $"с ТРЕМЯ фаст ЯДЕРКАМИ {core.ToEmojiString("oroobamboe")}{core.ToEmojiString("oroobamboe")}{core.ToEmojiString("oroobamboe")}",
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
                        $"раш командирами с пушкой {core.ToEmojiString("coolstory")}{core.ToEmojiString("deadinside")}{core.ToEmojiString("vasyanbamboe")}{core.ToEmojiString("oldbamboe")} с блокпостом + Т2 арта",
                        $"раш командирами с пушкой {core.ToEmojiString("coolstory")}{core.ToEmojiString("deadinside")}{core.ToEmojiString("vasyanbamboe")}{core.ToEmojiString("oldbamboe")} ИДЕМ ДО КОНЦА{core.ToEmojiString("oroobamboe")}{core.ToEmojiString("oroobamboe")}",
                        $"с гетто ганшипом {core.ToEmojiString("qwirchamp")}{core.ToEmojiString("call_me_tone1")}",
                        $"с фаст рембо командиром{core.ToEmojiString("supremebamboe")}"
                    }
                },
                new SupremeMap
                {
                    Names = new[] {"на сетоне", "сетон", "поиграть сетон", "СЕТОООН", "СЕТОООООООН"},
                    Tactics = new[]
                    {
                        $"на аир позиции. ТРЕНИРУЙСЯ {core.ToEmojiString("supremebamboe")}{core.ToEmojiString("lejatbamboe")}",
                        $"на позиции ROCK{core.ToEmojiString("shark")}, захватывай остров (так лееень((()()",
                        $"на позиции пляж{core.ToEmojiString("cup_with_straw")}{core.ToEmojiString("lootecbamboe")}",
                        $"мид РЕКЛЕЙМИ ВОЮЙ СПАМЬ  УБИТЬ           УБИВААЙ {core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}"
                    }
                },
                new SupremeMap
                {
                    Names = new[]
                    {
                        "канис (или ченить такое)", "пирамиду (или подобную)", "норм карту", "на норм карте",
                        "хилли плато",
                        "поиграть норм карту", "на какой-нибудь норм карте"
                    },
                    Tactics = new[]
                    {
                        $"давай не сцы надо поднимать скилл уже {core.ToEmojiString("qwirnbamboe")}",
                        $"ВПЕРЕЕЕД на фронт главное чтобы не забанили за Т1 спам {core.ToEmojiString("hitlerbamboe")}"
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
                        $"c фаст ЯДЕРКОЙ{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}{core.ToEmojiString("lejatbamboe")}",
                        $"за кебран с фаст пауком{core.ToEmojiString("lootecbamboe")}",
                        $"с тактикой на двоих - ФАСТ {core.ToEmojiString("t3")} АРТА {core.ToEmojiString("dobrobamboe")}",
                        $"с нагиб тактикой на четверых - ЯДЕРКА{core.ToEmojiString("radioactive")}+ПАУК{core.ToEmojiString("lootecbamboe")}+ИТОТА{core.ToEmojiString("bombitbamboe")}+FATBOY{core.ToEmojiString("oldbamboe")}",
                        $"просто почилить главное не забудь антинюку и турели к 15 минуте {core.ToEmojiString("spongebamboe")}"
                    },
                },
                new SupremeMap
                {
                    Names = new[] {string.Empty},
                    Tactics = new[]
                    {
                        $"2vs2 или 3vs3 НАГИИБ (слив) {core.ToEmojiString("supremebamboe")}",
                    }
                }
            };

            var map = rnd.GetRandom(maps, m => m.Tactics.Count);
            var discord = message.DiscordMessage;
            await discord.Channel.SendMessageAsync(
                    $"{discord.Author.Username}, {sugg.Random(rnd)} {map.Names.Random(rnd)} {map.Tactics.Random(rnd)}",
                    message.Tts)
                .ConfigureAwait(false);
        }

        private bool IsDeservingLike(IMessage message, out string emoji)
        {
            var lower = message.Content.ToLower();
            var goodWords = new[] {"красавчик", "молор", "найс", "дядя", "мужик", "васян", "гунирал", "ля", "какой"};
            var emojies = new []
            {
                "valera", "oldbamboe", "kadikbamboe", "dobrobamboe", "lejatbabmoe"
            };
            if (lower.Contains("бот") && !lower.Contains("не") && lower.ContainsAny(goodWords))
            {
                emoji = emojies.Random(rnd);
                return true;
            }

            emoji = null;
            return false;
        }

        private static async Task DeletePreviousMessageAsync(SocketMessage message)
        {
            var messages = await message.Channel.GetMessagesAsync(message, Direction.Before, 1).FlattenAsync()
                .ConfigureAwait(false);

            var messageToDelete = messages.FirstOrDefault();
            
            if (messageToDelete is IUserMessage userMessage && !userMessage.Content.Contains("~~"))
            {
                await userMessage.DeleteAsync().ConfigureAwait(false);
                await message.Channel.SendMessageAsync($"||~~{userMessage.Content}~~||")
                    .ConfigureAwait(false);
            }
        }
    }
}