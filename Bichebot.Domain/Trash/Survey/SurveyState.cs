using System.Collections.Generic;

namespace Bichebot.Domain.Trash.Survey
{
    public class SurveyState
    {
        public ulong LastMessageId { get; set; }
        public SurveyStatus Status { get; set; }
        public int CurrentQuestion { get; set; }
        public List<UserAnswer> Answers { get; set; } = new List<UserAnswer>();
    }

    public enum SurveyStatus
    {
        Open,
        InProgress,
        Done
    }
}