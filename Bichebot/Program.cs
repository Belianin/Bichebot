using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Bichebot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            
            CreateBot().Run(cts.Token);
        }

        private static Bot CreateBot()
        {
            var directories = Directory.GetDirectories(Directory.GetCurrentDirectory());
            Console.WriteLine(string.Join("\n", directories));
            
            var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Bichebot");
            Console.WriteLine(string.Join("\n", files));
            
            var d = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/Bichebot");
            Console.WriteLine(string.Join("\n", d));
            var token = GetEnvironmentVariable("BICHEBOT_TOKEN");
            var mongoPassword = GetEnvironmentVariable("MONGODB_PASSWORD");

            var settings = new BotSettings
            {
                Token = token
            };
            
            return BotFactory.Create(settings);
        }

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process) ??
                   Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User) ??
                   Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        }

    }
}