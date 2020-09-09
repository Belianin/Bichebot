using System.Threading.Tasks;
using Bichebot.Core.Audio;
using Bichebot.Core.Pipeline;
using Discord.WebSocket;

namespace Bichebot.Domain.Pipeline.Audio
{
    public class AudioHandler : IMessageHandler
    {
        private readonly AudioSpeaker audio;

        public AudioHandler(AudioSpeaker audio)
        {
            this.audio = audio;
        }

        public async Task<bool> HandleAsync(SocketMessage message)
        {
            if (message.Content == "go")
            {
                audio.Connect(message.Author);
                return true;
            }

            if (message.Content == "ilidan")
            {
                await audio.SendMessageAsync("ilidan.mp3").ConfigureAwait(false);
                return true;
            }

            return false;
        }
    }
}