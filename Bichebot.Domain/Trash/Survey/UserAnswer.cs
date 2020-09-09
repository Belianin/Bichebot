namespace Bichebot.Domain.Trash.Survey
{
    public class UserAnswer
    {
        public UserAnswer(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }

        public string Question { get; set; }
        public string Answer { get; set; }
    }
}