using System;

namespace Bichebot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var token = GetEnvironmentVariable("BICHEBOT_TOKEN");
            var mongoPassword = GetEnvironmentVariable("MONGODB_PASSWORD");

            var config = new BotConfig
            {
                Token = token
            };
            
            new Bot(config)
                .RunAsync().Wait();
        }

        static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User) ??
                   Environment.GetEnvironmentVariable(name);
        }
    }
}