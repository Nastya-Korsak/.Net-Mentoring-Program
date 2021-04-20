using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace FileSystemVisitorLibrary
{
    public class FileSystemVisitor
    {
        private readonly Func<IFileSystemInfo, bool> _filter;

        private readonly IFileSystem _fileSystem;

        public FileSystemVisitor(IFileSystem fileSystem, Func<IFileSystemInfo, bool> filter = null)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _filter = filter;
        }

        public event Action Started;

        public event Action Completed;

        public event Action<SearchEventArgs> FileFound;

        public event Action<SearchEventArgs> DirectoryFound;

        public event Action<SearchEventArgs> FilteredFileFound;

        public event Action<SearchEventArgs> FilteredDirectoryFound;

        public SearchAcions? SearchAcion { get; set; }

        public IEnumerable<string> Search(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
            }

            var directory = _fileSystem.DirectoryInfo.FromDirectoryName(path);

            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException($"Selected directory is not exist: {path}");
            }

            SearchAcion = SearchAcions.Continue;
            Started?.Invoke();

            foreach (var info in Search(directory))
            {
                yield return info.FullName;
            }

            Completed?.Invoke();
            SearchAcion = SearchAcions.Stop;
        }

        private IEnumerable<IFileSystemInfo> Search(IFileSystemInfo info)
        {
            foreach (var fileInfo in FileSearch(info))
            {
                yield return fileInfo;
            }

            foreach (var directoryInfo in DirectionSearch(info))
            {
                yield return directoryInfo;
            }
        }

        private IEnumerable<IFileSystemInfo> FileSearch(IFileSystemInfo root)
        {
            if (SearchAcion == SearchAcions.Stop)
            {
                yield break;
            }

            foreach (var filePath in _fileSystem.Directory.EnumerateFiles(root.FullName))
            {
                var fileInfo = _fileSystem.FileInfo.FromFileName(filePath);

                var isFiltered = _filter?.Invoke(fileInfo) ?? false;

                if (isFiltered)
                {
                    SearchAcion = SearchAcions.Skip;
                    FoudEventExecution(FilteredFileFound, fileInfo);
                }
                else
                {
                    FoudEventExecution(FileFound, fileInfo);
                }

                switch (SearchAcion)
                {
                    case SearchAcions.Continue:
                        yield return fileInfo;
                        break;
                    case SearchAcions.Skip:
                        SearchAcion = SearchAcions.Continue;
                        continue;
                    case SearchAcions.Stop:
                        yield return fileInfo;
                        yield break;
                }
            }
        }

        private IEnumerable<IFileSystemInfo> DirectionSearch(IFileSystemInfo root)
        {
            if (SearchAcion == SearchAcions.Stop)
            {
                yield break;
            }

            foreach (var directoryPath in _fileSystem.Directory.EnumerateDirectories(root.FullName))
            {
                if (SearchAcion == SearchAcions.Stop)
                {
                    yield break;
                }

                var directoryInfo = _fileSystem.DirectoryInfo.FromDirectoryName(directoryPath);

                var isFiltered = _filter?.Invoke(directoryInfo) ?? false;

                var isFilterSkip = false;

                if (isFiltered)
                {
                    SearchAcion = null;
                    FoudEventExecution(FilteredDirectoryFound, directoryInfo);

                    if (SearchAcion == null)
                    {
                        SearchAcion = SearchAcions.Skip;
                        isFilterSkip = true;
                    }
                }
                else
                {
                    FoudEventExecution(DirectoryFound, directoryInfo);
                }

                switch (SearchAcion)
                {
                    case SearchAcions.Continue:
                        yield return directoryInfo;
                        break;
                    case SearchAcions.Skip:
                        {
                            SearchAcion = SearchAcions.Continue;
                            if (!isFilterSkip)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                    case SearchAcions.Stop:
                        yield return directoryInfo;
                        yield break;
                }

                foreach (var fileInfo in Search(directoryInfo))
                {
                    yield return fileInfo;
                }
            }
        }

        private void FoudEventExecution(Action<SearchEventArgs> foudEvent, IFileSystemInfo info)
        {
            var args = new SearchEventArgs(
                info,
                stop: () => SearchAcion = SearchAcions.Stop,
                skip: () => SearchAcion = SearchAcions.Skip,
                @continue: () => SearchAcion = SearchAcions.Continue);

            foudEvent?.Invoke(args);
        }
    }
}
