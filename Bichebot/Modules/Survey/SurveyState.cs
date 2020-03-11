using System.Collections.Generic;

namespace Bichebot.Modules.Survey
{
    public class SurveyState
    {
        public ulong LastMessageId { get; set; }
        public SurveyStatus Status { get; set; }
        public int CurrentQuestion { get; set; }
        public List<UserAnswer> Answers { get; set; }
    }

    public enum SurveyStatus
    {
        Open,
        InProgress,
        Done
    }
}