using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemVisitorLibrary
{
    public class FileSystemVisitor
    {
        private readonly Func<FileSystemInfo, bool> _filterFunc;

        public FileSystemVisitor(Func<FileSystemInfo, bool> filterFunc = null)
        {
            _filterFunc = filterFunc;
        }

        public event Action Started;

        public event Action Complited;

        public event Func<FileSystemInfo, Actions> FileFound;

        public event Func<FileSystemInfo, Actions> DirectoryFound;

        public event Func<FileSystemInfo, Actions> FilteredFileFound;

        public event Func<FileSystemInfo, Actions> FilteredDirectoryFound;

        public IEnumerable<string> Search(string path)
        {
            Started?.Invoke();

            foreach (var f in FileSearch(path))
            {
                yield return f;
            }

            foreach (var d in DirectionSearch(path))
            {
                yield return d;
            }

            Complited?.Invoke();
        }

        private IEnumerable<string> FileSearch(string path)
        {
            foreach (var f in Directory.EnumerateFiles(path).Where(f => _filterFunc == null || _filterFunc(new FileInfo(f))))
            {
                var fileInfo = new FileInfo(f);
                Actions? action = Actions.Continue;

                if (_filterFunc == null)
                {
                    action = FileFound?.Invoke(fileInfo);
                }
                else
                {
                    action = FilteredFileFound?.Invoke(fileInfo);
                }

                switch (action)
                {
                    case Actions.Continue:
                        yield return f;
                        break;
                    case Actions.Skip:
                        continue;
                    case Actions.Stop:
                        yield return f;
                        yield break;
                }
            }

            foreach (var d in Directory.EnumerateDirectories(path))
            {
                foreach (var f in FileSearch(d))
                {
                    yield return f;
                }
            }
        }

        private IEnumerable<string> DirectionSearch(string path)
        {
            foreach (var f in Directory.EnumerateDirectories(path).Where(f => _filterFunc == null || _filterFunc(new FileInfo(f))))
            {
                var fileInfo = new FileInfo(f);
                Actions? action = Actions.Continue;

                if (_filterFunc == null)
                {
                    DirectoryFound?.Invoke(fileInfo);
                }
                else
                {
                    FilteredDirectoryFound?.Invoke(fileInfo);
                }

                switch (action)
                {
                    case Actions.Continue:
                        yield return f;
                        break;
                    case Actions.Skip:
                        continue;
                    case Actions.Stop:
                        yield return f;
                        yield break;
                }
            }
        }
    }
}
