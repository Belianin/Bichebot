namespace Bichebot.Modules
{
    public interface IBotModule
    {
        bool IsRunning { get; }
        
        void Run();

        void Stop();
    }
}