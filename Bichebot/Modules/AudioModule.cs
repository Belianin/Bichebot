using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;

namespace Bichebot.Modules
{
    public class AudioModule
    {
        private const string FFMPEG = "ffmpeg"; // Windows, Linux; opus.dll and libsodium.dll?

        private readonly IBotCore core;

        public bool IsConnected => core.Guild.AudioClient != null;

        public AudioModule(IBotCore core)
        {
            if (!File.Exists(FFMPEG))
                throw new FileNotFoundException($"{FFMPEG} is not found");

            this.core = core;
        }
        
        public void Connect(IUser user)
        {
            Connect(core.Guild.VoiceChannels.FirstOrDefault(c => c.Users.Contains(user)));
        }

        public void Connect(IAudioChannel channel)
        {
            if (channel == null)
                return;
            
            //new Thread(() => ConnectToChannel(channel)).Start();
            Task.Run(() => ConnectToChannel(channel)).Wait();
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
        
        private void ConnectToChannel(IAudioChannel channel)
        {
            Console.WriteLine("Connecting");
            channel.ConnectAsync().Wait();
            Console.WriteLine("Connected");
        }
    }
}