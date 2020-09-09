using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;

namespace Bichebot.Core.Audio
{
    public class AudioSpeaker
    {
        private const string FFMPEG = "ffmpeg";

        private readonly IBotCore core;

        public AudioSpeaker(IBotCore core)
        {
            this.core = core;
        }

        public IAudioChannel CurrentChannel { get; private set; }

        public bool IsSpeaking { get; private set; }

        private bool IsConnected => core.Guild.AudioClient != null;

        public void Connect(IUser user)
        {
            Connect(core.Guild.VoiceChannels.FirstOrDefault(c => c.Users.Contains(user)));
        }

        private void Connect(IAudioChannel channel)
        {
            if (channel == null)
                return;

            new Thread(() => ConnectToChannel(channel)).Start();
        }

        public async Task SendMessageAsync(string fileName)
        {
            if (!IsConnected || IsSpeaking)
                return;

            IsSpeaking = true;

            var psi = new ProcessStartInfo
            {
                FileName = FFMPEG,
                Arguments = $"-hide_banner -loglevel panic -i \"{fileName}\" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            using var ffmpeg = Process.Start(psi);

            if (ffmpeg == null)
                return;

            await using var output = ffmpeg.StandardOutput.BaseStream;
            await using var discord = core.Guild.AudioClient.CreatePCMStream(AudioApplication.Mixed);
            try
            {
                await output.CopyToAsync(discord);
            }
            finally
            {
                await discord.FlushAsync();
                IsSpeaking = false;
            }
        }

        private void ConnectToChannel(IAudioChannel channel)
        {
            Console.WriteLine("Connecting");
            channel.ConnectAsync().Wait();
            CurrentChannel = channel;
            Console.WriteLine("Connected");
        }
    }
}