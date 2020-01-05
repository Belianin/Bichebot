using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Audio;
using Bichebot.Modules.Base;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Modules.Greeter
{
    public class GreeterModule : BaseVoiceModule
    {
        private readonly GreeterModuleSettings settings;

        private readonly AudioSpeaker audio;
        
        public GreeterModule(IBotCore core, AudioSpeaker audio, GreeterModuleSettings settings) : base(core)
        {
            this.settings = settings;
            this.audio = audio;
        }

        protected override async Task HandleMessageAsync(SocketUser user, SocketVoiceState firstState, SocketVoiceState secondState)
        {
            if (!IsBotChannel(firstState.VoiceChannel) &&
             IsBotChannel(secondState.VoiceChannel) &&
             settings.Greetings.TryGetValue(user.Id, out var filename))
            {
                lock (audio)
                {
                    audio.SendMessageAsync(filename).Wait();
                }
            }
        }

        private bool IsBotChannel(IAudioChannel channel)
        {
            if (channel == null || audio.CurrentChannel == null)
                return false;
            return audio.CurrentChannel.Id == channel.Id;
        }
    }
}