using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Domain;

namespace Bichebot
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            AppDomain.CurrentDomain.ProcessExit += (s, e) => cts.Cancel();

            var bot = BotFactory.Instance.Create(WellKnown.Settings);
            
            var token = GetEnvironmentVariable("BICHEBOT_TOKEN") ?? File.ReadAllText("BICHEBOT_TOKEN");
            
            await bot.RunAsync(token, cts.Token).ConfigureAwait(false);
        }

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process) ??
                   Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User) ??
                   Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        }
    }
}