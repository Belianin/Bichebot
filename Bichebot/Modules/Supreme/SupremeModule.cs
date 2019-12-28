using System.Collections.Generic;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Bichebot.Utilities;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Modules.Supreme
{
    public class SupremeModule : StatefulBaseModule<SupremeState>
    {
        public SupremeModule(IBotCore core) : base(core, () => new SupremeState()) {}

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            var state = GetState(message.Author.Id);
            if (IsSupremeAsked(message))
            {
                await TrySupremeSuggestionAsync(message).ConfigureAwait(false);
                state.DialogLevel = DialogLevel.Second;
            }
            else if (state.DialogLevel == DialogLevel.Second && message.Content.Contains("не"))
            {
                await message.Channel.SendMessageAsync("Ага").ConfigureAwait(false);
                state.DialogLevel = DialogLevel.First;
            }
        }

        private bool IsSupremeAsked(IMessage message)
        {
            var lower = message.Content.ToLower();
            return lower.ContainsAny(new [] {"бот ", "бот,"}) &&
                   lower.Contains("суприм") &&
                   lower.ContainsAny(new[] {"игра", "гам", " в ", "го "});
        }

        private async Task TrySupremeSuggestionAsync(SocketMessage message)
        {
            var sugg = new []{"Советую", "Предлагаю", "Попробуй", "Го", "А может", "Как насчет", "Мб"};

            var maps = new List<SupremeMap>
            {
                new SupremeMap
                {
                    Names = new[] {"на дуалгепе", "дуалгеп", "поиграть дуалгеп", "DualGap"},
                    Tactics = new[]
                    {
                        $"c фаст ЯДЕРКОЙ{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}",
                        $"мид (спутники{Core.ToEmojiString("qwirchamp")}, толстяки{Core.ToEmojiString("dobrobamboe")}{Core.ToEmojiString("ocean")})",
                        $"на аире (БОМБИИМ{Core.ToEmojiString("gif")}), главное вовремя перейди в {Core.ToEmojiString("t3")} {Core.ToEmojiString("dobrobamboe")}",
                        $"не важно какой слот, главное в конце не забудь телесакушки{Core.ToEmojiString("vasyanbamboe")}",
                        $"не важно какой слот, главное в конце ЭОН телесакушки{Core.ToEmojiString("vasyanbamboe")} (ТЕЛЕПОРТ КОЛОССА{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")})",
                        $"и снайп корсарами НЫЫААААА{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}",
                        $"на эко фаст {Core.ToEmojiString("t3")} арта {Core.ToEmojiString("dobrobamboe")}{Core.ToEmojiString("thumbup")}",
                        $"и ТОПИТЬ за навального {Core.ToEmojiString("coolstory")} (ВЗОРВЕМ либерах {Core.ToEmojiString("deadinside")})",
                        $"c фаст телемазером {Core.ToEmojiString("kripotabamboe")}{Core.ToEmojiString("point_right")}{Core.ToEmojiString("ok_hand")}",
                        $"c дропом двух комов в тылы противника {Core.ToEmojiString("coolstory")}{Core.ToEmojiString("deadinside")}",
                        $"с дропом кома с клоакой+лазер {Core.ToEmojiString("qwirchamp")}",
                        $"с дропом ПАУКА {Core.ToEmojiString("valera")}{Core.ToEmojiString("gif")}",
                        $"мид за серафим ИТОТА{Core.ToEmojiString("heart")}{Core.ToEmojiString("lyabamboe")}",
                        $"с дропом рембо{Core.ToEmojiString("bombitbamboe")} командира за серафим {Core.ToEmojiString("supremebamboe")}",
                        $"с ТРЕМЯ фаст ЯДЕРКАМИ {Core.ToEmojiString("gif")}{Core.ToEmojiString("gif")}{Core.ToEmojiString("gif")}",
                        $"и твори НАГИБ с АИРА фаст бомберами{Core.ToEmojiString("gif")} + {Core.ToEmojiString("t3")} ASF",
                        $"с фаст{Core.ToEmojiString("supremebamboe")} АХВАААССООЙЙ{Core.ToEmojiString("supremebamboe")}"
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
                        $"раш командирами с пушкой {Core.ToEmojiString("coolstory")}{Core.ToEmojiString("deadinside")}{Core.ToEmojiString("vasyanbamboe")}{Core.ToEmojiString("oldbamboe")} с блокпостом + Т2 арта",
                        $"раш командирами с пушкой {Core.ToEmojiString("coolstory")}{Core.ToEmojiString("deadinside")}{Core.ToEmojiString("vasyanbamboe")}{Core.ToEmojiString("oldbamboe")} ИДЕМ ДО КОНЦА{Core.ToEmojiString("gif")}{Core.ToEmojiString("gif")}",
                        $"с гетто ганшипом {Core.ToEmojiString("qwirchamp")}{Core.ToEmojiString("call_me_tone1")}",
                        $"с фаст рембо командиром{Core.ToEmojiString("supremebamboe")}"
                    }
                },
                new SupremeMap
                {
                    Names = new[] {"на сетоне", "сетон", "поиграть сетон", "СЕТОООН", "СЕТОООООООН"},
                    Tactics = new[]
                    {
                        $"на аир позиции. ТРЕНИРУЙСЯ {Core.ToEmojiString("supremebamboe")}{Core.ToEmojiString("lejatbamboe")}",
                        $"на позиции ROCK{Core.ToEmojiString("shark")}, захватывай остров (так лееень((()()",
                        $"на позиции пляж{Core.ToEmojiString("cup_with_straw")}{Core.ToEmojiString("lootecbamboe")}",
                        $"мид РЕКЛЕЙМИ ВОЮЙ СПАМЬ  УБИТЬ           УБИВААЙ {Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}"
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
                        $"давай не сцы надо поднимать скилл уже {Core.ToEmojiString("qwirbamboe")}",
                        $"ВПЕРЕЕЕД на фронт главное чтобы не забанили за Т1 спам {Core.ToEmojiString("hitlerbamboe")}",
                        $"там хоть научишься играть в настоящий суприм{Core.ToEmojiString("supremebamboe")}",
                        $", а лучше на Big Bang Lake супер карта и очень динамичная{Core.ToEmojiString("jet")}",
                        $"ведь намного веселее когда бой идет постоянно, надоели геймендеры {Core.ToEmojiString("qwirbamboe")}",
                        $", а то разве не задолбало 10 минут смотреть на экстракторы?{Core.ToEmojiString("newfagbamboe")}",
                        $"ПОРА НА ФРОНТ{Core.ToEmojiString("hitlerbamboe")}{Core.ToEmojiString("raised_hand_tone5")}"
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
                        $"c фаст ЯДЕРКОЙ{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}{Core.ToEmojiString("lejatbamboe")}",
                        $"за кебран с фаст пауком{Core.ToEmojiString("lootecbamboe")}",
                        $"с тактикой на двоих - ФАСТ {Core.ToEmojiString("t3")} АРТА {Core.ToEmojiString("dobrobamboe")}",
                        $"с нагиб тактикой на четверых - ЯДЕРКА{Core.ToEmojiString("radioactive")}+ПАУК{Core.ToEmojiString("lootecbamboe")}+ИТОТА{Core.ToEmojiString("bombitbamboe")}+FATBOY{Core.ToEmojiString("oldbamboe")}",
                        $"просто почилить главное не забудь антинюку и турели к 15 минуте {Core.ToEmojiString("spongebamboe")}",
                        $"и забань {Core.ToEmojiString("t3")}радар потом го распиливать с клоакой {Core.ToEmojiString("qwirbamboe")}"
                    },
                },
                new SupremeMap
                {
                    Names = new[] {string.Empty},
                    Tactics = new[]
                    {
                        $"2vs2 или 3vs3 НАГИИБ (слив) {Core.ToEmojiString("supremebamboe")}",
                        $"поиграть ладдер (no greys{Core.ToEmojiString("face_with_monocle")})",
                        $"сыграть уже в ладдер че как нуб серый{Core.ToEmojiString("qwirchamp")}",
                        $"Ctrl+K и в лол{Core.ToEmojiString("lol2")}",
                        $"смотреть каст с Сидом играть как-то лень"
                    }
                }
            };
            
            var map = Core.Random.Choose(maps, m => m.Tactics.Count);
            var response = $"{message.Author.Username}, {Core.Random.Choose(sugg)} {Core.Random.Choose(map.Names)} {Core.Random.Choose(map.Tactics)}";
            await message.Channel.SendMessageAsync(response,message.Content.Contains("/tts"))
                .ConfigureAwait(false);
        }

    }
}