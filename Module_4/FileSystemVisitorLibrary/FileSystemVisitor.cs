using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace FileSystemVisitorLibrary
{
    public class FileSystemVisitor
    {
        private readonly Func<FileSystemInfo, bool> _filterFunc;

        private readonly IFileSystem _fileSystem;

        private Actions? _action = Actions.Continue;

        public FileSystemVisitor(IFileSystem fileSystem, Func<FileSystemInfo, bool> filterFunc = null)
        {
            _fileSystem = fileSystem;
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
            if (path == null)
            {
                throw new ArgumentNullException(path, "Directory path is null");
            }

            if (path == string.Empty)
            {
                throw new ArgumentException("Directory path is empty", path);
            }

            if (!_fileSystem.Directory.Exists(path))
            {
                throw new ArgumentException("Selected directory is not exist", path);
            }

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
            if (_action == Actions.Stop)
            {
                yield break;
            }

            foreach (var f in _fileSystem.Directory.EnumerateFiles(path).Where(f => _filterFunc == null || !_filterFunc(new FileInfo(f))))
            {
                var fileInfo = new FileInfo(f);

                if (_filterFunc == null && FileFound != null)
                {
                    _action = FileFound.Invoke(fileInfo);
                }
                else if (FilteredFileFound != null)
                {
                    _action = FilteredFileFound.Invoke(fileInfo);
                }

                switch (_action)
                {
                    case Actions.Continue:
                        yield return f;
                        break;
                    case Actions.Skip:
                        _action = Actions.Continue;
                        continue;
                    case Actions.Stop:
                        yield return f;
                        yield break;
                }
            }

            foreach (var d in _fileSystem.Directory.EnumerateDirectories(path))
            {
                foreach (var f in FileSearch(d))
                {
                    yield return f;
                }
            }
        }

        private IEnumerable<string> DirectionSearch(string path)
        {
            if (_action == Actions.Stop)
            {
                yield break;
            }

            foreach (var f in _fileSystem.Directory.EnumerateDirectories(path).Where(f => _filterFunc == null || !_filterFunc(new FileInfo(f))))
            {
                var fileInfo = new FileInfo(f);

                if (_filterFunc == null && DirectoryFound != null)
                {
                    _action = DirectoryFound?.Invoke(fileInfo);
                }
                else if (FilteredDirectoryFound != null)
                {
                    _action = FilteredDirectoryFound?.Invoke(fileInfo);
                }

                switch (_action)
                {
                    case Actions.Continue:
                        yield return f;
                        break;
                    case Actions.Skip:
                        _action = Actions.Continue;
                        continue;
                    case Actions.Stop:
                        yield return f;
                        yield break;
                }
            }

            foreach (var d in _fileSystem.Directory.EnumerateDirectories(path))
            {
                foreach (var f in DirectionSearch(d))
                {
                    yield return f;
                }
            }
        }
    }
}
