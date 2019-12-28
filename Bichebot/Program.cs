using System;
using System.Threading;

namespace Bichebot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var token = GetEnvironmentVariable("BICHEBOT_TOKEN");
            var mongoPassword = GetEnvironmentVariable("MONGODB_PASSWORD");

            var config = new BotConfig
            {
                Token = token
            };

            var cts = new CancellationTokenSource();
            
            new Bot(config)
                .RunAsync(cts.Token).Wait(cts.Token);
        }

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process) ??
                   Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User) ??
                   Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        }

    }
}