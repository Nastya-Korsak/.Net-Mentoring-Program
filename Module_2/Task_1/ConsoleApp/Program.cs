using System;
using System.Collections.Generic;
using HelloLibrary;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args, new Dictionary<string, string>
            {
                ["-Name"] = "Name"
            });

            var config = builder.Build();

            var name = config["Name"];

            var printLine = WelcomeMessage.GetWelcomeMessage(name);

            Console.WriteLine(printLine);
        }
    }
}
