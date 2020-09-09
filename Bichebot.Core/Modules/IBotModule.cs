namespace Bichebot.Core.Modules
{
    public interface IBotModule
    {
        bool IsRunning { get; }
        
        void Run();

        void Stop();
    }
}