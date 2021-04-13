using System;
using System.IO;
using FileSystemVisitorLibrary;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = args.Length > 0 && Directory.Exists(args[0]) ? args[0] : Directory.GetCurrentDirectory();

            Func<FileSystemInfo, bool> filter = (FileSystemInfo e) => e.Extension == ".png";

            var visitor = new FileSystemVisitor(filter);

            visitor.Started += () => Console.WriteLine($"Search started! Entry point {path}");
            visitor.Complited += () => Console.WriteLine($"Search finished!");

            visitor.FilteredFileFound += (FileSystemInfo e) =>
            {
                if (e.Name.Contains("Screenshot"))
                {
                    return Actions.Skip;
                }
                else
                {
                    return Actions.Continue;
                }
            };

            foreach (var f in visitor.Search(path))
            {
                Console.WriteLine(f.Substring(path.Length));
            }
        }
    }
}
