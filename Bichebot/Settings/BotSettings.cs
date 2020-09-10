using System.Collections.Generic;
using System.IO;
using Bichebot.Domain.Modules.Best;
using Bichebot.Domain.Modules.FOnlineStatistics;
using Bichebot.Domain.Pipeline.Bank;
using Bichebot.Domain.Pipeline.JumpGame;
using Bichebot.Domain.Pipeline.MemeGenerator;
using Bichebot.Domain.Trash.Greeter;

namespace Bichebot.Settings
{
    public class BotSettings
    {
        public ulong GuildId { get; set; }
        public BestModuleSettings BestModule { get; set; }
        public BankModuleSettings BankModule { get; set; }
        public GreeterModuleSettings GreeterModule { get; set; }
        public WithermansSettings WithermansModule { get; set; }
        public MemeGeneratorSettings MemeGenerator { get; set; }
        public FonlineStatisticsModuleSettings FonlineStatisticsModule { get; set; }

        public static BotSettings Default => new BotSettings
        {
            GuildId = WellKnown.GuildId,
            BestModule = new BestModuleSettings
            {
                BestChannelId = WellKnown.Channels.Best,
                ReactionCountToBeBest = 3,
                Reward = 100,
                SourceChannelIds = new HashSet<ulong>
                {
                    WellKnown.Channels.Discussions,
                    WellKnown.Channels.Zero,
                    WellKnown.Channels.Debug
                }
            },
            BankModule = new BankModuleSettings
            {
                Admins = new List<ulong>
                {
                    WellKnown.Users.Aler
                }
            },
            GreeterModule = new GreeterModuleSettings
            {
                Greetings = new Dictionary<ulong, string>
                {
                    {WellKnown.Users.Host, "host.mp3"},
                    {WellKnown.Users.Qwear, "qwear.mp3"},
                    {WellKnown.Users.Aler, "aler.mp3"},
                    {WellKnown.Users.Luno, "ilidan.mp3"}
                }
            },
            WithermansModule = new WithermansSettings
            {
                Admin = WellKnown.Users.Aler,
                JumpGame = new JumpGameSettings
                {
                    ChannelId = WellKnown.Channels.Discussions
                }
            },
            FonlineStatisticsModule = new FonlineStatisticsModuleSettings
            {
                ChannelId = WellKnown.Channels.Debug
            },
            MemeGenerator = new MemeGeneratorSettings
            {
                MemePhrases = File.ReadAllLines("Resources/memes.txt")
            }
        };
    }
}