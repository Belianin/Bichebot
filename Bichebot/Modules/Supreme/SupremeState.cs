namespace Bichebot.Modules.Supreme
{
    public class SupremeState
    {
        public DialogLevel DialogLevel { get; set; } = DialogLevel.First;
    }

    public enum DialogLevel
    {
        First,
        Second
    }
}