using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Core.Audio;
using Bichebot.Core.Modules.Base;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Domain.Trash.Greeter
{
    // unused
    internal class GreeterModule : BaseVoiceModule
    {
        private readonly AudioSpeaker audio;
        private readonly GreeterModuleSettings settings;

        public GreeterModule(IBotCore core, AudioSpeaker audio, GreeterModuleSettings settings) : base(core)
        {
            this.settings = settings;
            this.audio = audio;
        }

        protected override async Task HandleMessageAsync(SocketUser user, SocketVoiceState firstState,
            SocketVoiceState secondState)
        {
            if (!IsBotChannel(firstState.VoiceChannel) &&
                IsBotChannel(secondState.VoiceChannel) &&
                settings.Greetings.TryGetValue(user.Id, out var filename))
                lock (audio)
                {
                    audio.SendMessageAsync(filename).Wait();
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