// ReSharper disable IdentifierTypo

using System.Collections.Generic;
using System.IO;
using Bichebot.Domain;
using Bichebot.Domain.Modules.Best;
using Bichebot.Domain.Modules.FOnlineStatistics;
using Bichebot.Domain.Pipeline.Bank;
using Bichebot.Domain.Pipeline.JumpGame;
using Bichebot.Domain.Pipeline.MemeGenerator;
using Bichebot.Domain.Trash.Greeter;

namespace Bichebot
{
    public static class WellKnown
    {
        public const ulong GuildId = 307817006021738507;

        public static class Users
        {
            public const ulong Aler = 272446177163608066;
            public const ulong Host = 177461516646219776;
            public const ulong Qwear = 177455722248798208;
            public const ulong Luno = 490208786036948994;
        }

        public static class Channels
        {
            public const ulong Best = 602475515391246358;
            public const ulong Debug = 656922777344802882;
            public const ulong Zero = 676118662205276163;
            public const ulong Discussions = 553146693743280128;
        }
        
        public static BotSettings Settings => new BotSettings
        {
            GuildId = GuildId,
            Best = new BestModuleSettings
            {
                BestChannelId = Channels.Best,
                ReactionCountToBeBest = 3,
                Reward = 100,
                SourceChannelIds = new HashSet<ulong>
                {
                    Channels.Discussions,
                    Channels.Zero,
                    Channels.Debug
                }
            },
            Bank = new BankSettings
            {
                Admins = new List<ulong>
                {
                    Users.Aler
                }
            },
            Greeter = new GreeterModuleSettings
            {
                Greetings = new Dictionary<ulong, string>
                {
                    {Users.Host, "host.mp3"},
                    {Users.Qwear, "qwear.mp3"},
                    {Users.Aler, "aler.mp3"},
                    {Users.Luno, "ilidan.mp3"}
                }
            },
            Withermans = new WithermansSettings
            {
                Admin = Users.Aler,
                JumpGame = new JumpGameSettings
                {
                    ChannelId = Channels.Discussions
                }
            },
            FoStatistics = new FonlineStatisticsModuleSettings
            {
                ChannelId = Channels.Debug
            },
            MemeGenerator = new MemeGeneratorSettings
            {
                MemePhrases = File.ReadAllLines("Resources/memes.txt")
            }
        };
    }
}