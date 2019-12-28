using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core;
using Discord;
using Discord.Audio;

namespace Bichebot.Modules.Audio
{
    public class AudioSpeaker
    {
        private const string FFMPEG = "ffmpeg";

        private readonly IBotCore core;

        private bool IsConnected => core.Guild.AudioClient != null;

        public AudioSpeaker(IBotCore core)
        {
            this.core = core;
        }
        
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
            if (!IsConnected)
                return;
            
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
            }
        }
        
        private static void ConnectToChannel(IAudioChannel channel)
        {
            Console.WriteLine("Connecting");
            channel.ConnectAsync().Wait();
            Console.WriteLine("Connected");
        }
    }
}