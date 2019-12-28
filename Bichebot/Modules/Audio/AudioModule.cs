using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Discord.WebSocket;

namespace Bichebot.Modules.Audio
{
    public class AudioModule : BaseModule
    {
        private readonly AudioSpeaker audio;
        
        public AudioModule(IBotCore core) : base(core)
        {
            audio = new AudioSpeaker(core);    
        }

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (message.Content == "go")
                audio.Connect(message.Author);
            else if (message.Content == "ilidan")
                await audio.SendMessageAsync("ilidan.mp3").ConfigureAwait(false);
        }
    }
}