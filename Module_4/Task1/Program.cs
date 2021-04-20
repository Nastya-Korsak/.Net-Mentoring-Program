using System;
using System.IO;
using System.IO.Abstractions;
using FileSystemVisitorLibrary;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = args.Length > 0 && Directory.Exists(args[0]) ? args[0] : Directory.GetCurrentDirectory();

            Func<IFileSystemInfo, bool> filter = (IFileSystemInfo e) => e.Extension == ".dll";

            var visitor = new FileSystemVisitor(new FileSystem(), filter);

            visitor.Started += () => Console.WriteLine($"Search started! Entry point {path}");
            visitor.Completed += () => Console.WriteLine($"Search finished!");

            visitor.FilteredFileFound += e =>
            {
                if (e.Info.Name == "Task1.dll")
                {
                    e.Stop();
                }
            };

            foreach (var f in visitor.Search(path))
            {
                Console.WriteLine(f[path.Length..]);
            }
        }
    }
}
