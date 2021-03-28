using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args, new Dictionary<string, string>{
                ["-Name"] = "Name"
            });

            var config = builder.Build();

            var name = config["Name"];

            Console.WriteLine($"Hello {name}!");
        }
    }
}
