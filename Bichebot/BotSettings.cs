using System.Collections.Generic;
using Bichebot.Modules.Bank;
using Bichebot.Modules.Best;
using Bichebot.Modules.FOnlineStatistics;
using Bichebot.Modules.Greeter;
using Bichebot.Modules.Withermans;

namespace Bichebot
{
    public class BotSettings
    {
        public string Token { get; set; }

        public ulong GuildId { get; set; } = 307817006021738507;

        public BestModuleSettings BestModule { get; set; } = new BestModuleSettings
        {
            BestChannelId = 602475515391246358,
            ReactionCountToBeBest = 3
        };
        
        public BankModuleSettings BankModule { get; set; } = new BankModuleSettings
        {
            Admins = new List<ulong> {272446177163608066}
        };

        public GreeterModuleSettings GreeterModule { get; set; } = new GreeterModuleSettings
        {
            Greetings = new Dictionary<ulong, string>
            {
                {177461516646219776, "host.mp3"},
                {177455722248798208, "qwear.mp3"},
                {272446177163608066, "aler.mp3"},
                {490208786036948994, "ilidan.mp3"}
            }
        };
        
        public WithermansSettings WithermansModule { get; set; } = new WithermansSettings();

        public FonlineStatisticsModuleSettings FonlineStatisticsModule { get; set; } =
            new FonlineStatisticsModuleSettings
            {
                ChannelId = 656922777344802882
            };
    }
}