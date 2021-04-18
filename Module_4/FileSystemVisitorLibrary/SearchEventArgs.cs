using System;
using System.IO.Abstractions;

namespace FileSystemVisitorLibrary
{
    public sealed class SearchEventArgs : EventArgs
    {
        private readonly Action _stop;
        private readonly Action _skip;
        private readonly Action _continue;

        public SearchEventArgs(
            IFileSystemInfo info,
            Action stop,
            Action skip,
            Action @continue)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            _stop = stop ?? throw new ArgumentNullException(nameof(stop));
            _skip = skip ?? throw new ArgumentNullException(nameof(skip));
            _continue = @continue ?? throw new ArgumentNullException(nameof(stop));
        }

        public IFileSystemInfo Info { get; }

        public void Stop() => _stop();

        public void Skip() => _skip();

        public void Continue() => _continue();
    }
}
