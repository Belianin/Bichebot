using Bichebot.Domain.Modules.Best;
using Bichebot.Domain.Modules.FOnlineStatistics;
using Bichebot.Domain.Pipeline.Bank;
using Bichebot.Domain.Pipeline.JumpGame;
using Bichebot.Domain.Pipeline.MemeGenerator;
using Bichebot.Domain.Trash.Greeter;

namespace Bichebot.Domain
{
    public class BotSettings
    {
        public ulong GuildId { get; set; }
        public BestModuleSettings Best { get; set; }
        public BankSettings Bank { get; set; }
        public GreeterModuleSettings Greeter { get; set; }
        public WithermansSettings Withermans { get; set; }
        public MemeGeneratorSettings MemeGenerator { get; set; }
        public FonlineStatisticsModuleSettings FoStatistics { get; set; }
    }
}