using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace FileSystemVisitorLibrary.UnitTests
{
    public class FileSystemVisitorUnitTests
    {
        private readonly MockFileSystem _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\tes1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\tes1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(Array.Empty<byte>()) },
            });

        [Fact]
        public void Search_WithoutFilterAndEvents_ShouldReturnAllFilesAndDirictories()
        {
            // Act
            var visitor = new FileSystemVisitor(_fileSystem);
            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Should().BeEquivalentTo(@"d:\test1\im.gif", @"d:\test1\tes1_1", @"d:\test1\tes1_1\text.txt", @"d:\test1\tes1_2", @"d:\test1\tes1_2\text2.txt");
        }

        [Fact]
        public void Search_FilterTxtFiles_ShouldNotReturnTxtFiles()
        {
            // Arrange
            Func<IFileSystemInfo, bool> filter = (IFileSystemInfo e) => e.Extension == ".txt";

            // Act
            var visitor = new FileSystemVisitor(_fileSystem, filter);
            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Should().BeEquivalentTo(@"d:\test1\im.gif", @"d:\test1\tes1_1", @"d:\test1\tes1_2");
        }

        [Fact]
        public void Search_SkipDirectoryByDirectoryFoundEvent_ShouldNotReturnSkipedDirectory()
        {
            // Act
            var visitor = new FileSystemVisitor(_fileSystem);

            visitor.DirectoryFound += e =>
            {
                if (e.Info.Name.Contains("tes1_1"))
                {
                    e.Skip();
                }
            };

            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Should().BeEquivalentTo(@"d:\test1\im.gif", @"d:\test1\tes1_2", @"d:\test1\tes1_2\text2.txt");
        }

        [Fact]
        public void Search_StopSearchWhenFileFound_LastItemIsSearchedFile()
        {
            // Arrange
            _fileSystem.AddFile(@"d:\test1\image1.png", new MockFileData(Array.Empty<byte>()));
            _fileSystem.AddFile(@"d:\test1\image2.png", new MockFileData(Array.Empty<byte>()));

            // Act
            var visitor = new FileSystemVisitor(_fileSystem);

            visitor.FileFound += e =>
            {
                if (e.Info.Name.Contains("image2") && e.Info.Extension == ".png")
                {
                    e.Stop();
                }
            };

            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Should().BeEquivalentTo(@"d:\test1\im.gif", @"d:\test1\image1.png", @"d:\test1\image2.png");
        }

        [Fact]
        public void Search_FilterTxtFilesAndSkipFileByFileFoundEvent_ShouldFilterTxtFilesAndNotReturnSkipedFile()
        {
            // Arrange
            Func<IFileSystemInfo, bool> filter = (IFileSystemInfo e) => e.Extension == ".txt";

            // Act
            var visitor = new FileSystemVisitor(_fileSystem, filter);

            visitor.FileFound += e =>
            {
                if (e.Info.Name == "im.gif")
                {
                    e.Skip();
                }
            };

            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Should().BeEquivalentTo(@"d:\test1\tes1_1", @"d:\test1\tes1_2");
        }

        [Fact]
        public void Search_FilterFilesAndStopOnSearchedDirectoryByFilteredDirectoryFoundEvent_FilesAreFilteredAndLastItemIsSearchedDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            fileSystem.AddDirectory(@"d:\test1\dir1");
            fileSystem.AddFile(@"d:\test1\dir1\text.txt", new MockFileData("Text"));
            fileSystem.AddDirectory(@"d:\test1\dir2");
            fileSystem.AddDirectory(@"d:\test1\test1_1\dir3");
            fileSystem.AddDirectory(@"d:\test1\test1_2\dir4");

            Func<IFileSystemInfo, bool> filter = (IFileSystemInfo e) => e.Name.Contains("dir");

            // Act
            var visitor = new FileSystemVisitor(fileSystem, filter);

            visitor.FilteredDirectoryFound += e =>
            {
                if (e.Info.Name.Contains("dir3"))
                {
                    e.Stop();
                }
            };

            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Count().Should().Be(3);
            items.Should().BeEquivalentTo(@"d:\test1\dir1\text.txt", @"d:\test1\test1_1", @"d:\test1\test1_1\dir3");
        }

        [Fact]
        public void Search_PathParameterIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var visitor = new FileSystemVisitor(_fileSystem);
            Func<IEnumerable<string>> searchFunc = () => visitor.Search(null);

            // Assert
            searchFunc.Enumerating().Should().Throw<ArgumentException>()
                .WithMessage($"'path' cannot be null or empty. (Parameter 'path')")
                .And.ParamName.Should().Be("path");
        }

        [Fact]
        public void Search_PathParameterIsNotExist_ShouldThrowArgumentNullException()
        {
            // Arrange
            var notExistPath = @"d:\notExist";

            // Act
            var visitor = new FileSystemVisitor(_fileSystem);
            Func<IEnumerable<string>> searchFunc = () => visitor.Search(notExistPath);

            // Assert
            searchFunc.Enumerating().Should().Throw<DirectoryNotFoundException>()
                .WithMessage($"Selected directory is not exist: {notExistPath}");
        }

        [Fact]
        public void Search_PathParameterIsEmpty_ShouldThrowArgumentNullException()
        {
            // Act
            var visitor = new FileSystemVisitor(_fileSystem);
            Func<IEnumerable<string>> searchFunc = () => visitor.Search(string.Empty);

            // Assert
            searchFunc.Enumerating().Should().Throw<ArgumentException>()
                .WithMessage($"'path' cannot be null or empty. (Parameter 'path')")
                .And.ParamName.Should().Be("path");
        }
    }
}
