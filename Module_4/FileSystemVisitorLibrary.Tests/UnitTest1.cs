using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace FileSystemVisitorLibrary.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Search_WithoutFilterAndEvents_ShouldReturnAllFilesAndDirictories()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\tes1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\tes1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(System.Array.Empty<byte>()) }
            });

            // Act
            var visitor = new FileSystemVisitor(fileSystem);
            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Should().BeEquivalentTo(@"d:\test1\im.gif", @"d:\test1\tes1_1\text.txt", @"d:\test1\tes1_2\text2.txt", @"d:\test1\tes1_1", @"d:\test1\tes1_2");
        }

        [Fact]
        public void Search_FilterTxtFiles_ShouldNotReturnTxtFiles()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\tes1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\tes1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(System.Array.Empty<byte>()) }
            });

            Func<FileSystemInfo, bool> filter = (FileSystemInfo e) => e.Extension == ".txt";

            // Act
            var visitor = new FileSystemVisitor(fileSystem, filter);
            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Should().BeEquivalentTo(@"d:\test1\im.gif", @"d:\test1\tes1_1", @"d:\test1\tes1_2");
        }

        [Fact]
        public void Search_SkipDirectoryByDirectoryFoundEvent_ShouldNotReturnSkipedDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\tes1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\tes1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(System.Array.Empty<byte>()) }
            });

            // Act
            var visitor = new FileSystemVisitor(fileSystem);

            visitor.DirectoryFound += (FileSystemInfo e) =>
            {
                if (e.Name.Contains("tes1_1"))
                {
                    return Actions.Skip;
                }
                else
                {
                    return Actions.Continue;
                }
            };

            var items = visitor.Search(@"d:\test1");

            // Assert
            items.Should().BeEquivalentTo(@"d:\test1\im.gif", @"d:\test1\tes1_1\text.txt", @"d:\test1\tes1_2\text2.txt", @"d:\test1\tes1_2");
        }

        [Fact]
        public void Search_StopSearchWhenFileFound_LastItemIsSearchedFile()
        {
            // Arrange
            var items = new List<string>();

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\tes1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\tes1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\image1.png", new MockFileData(System.Array.Empty<byte>()) },
                { @"d:\test1\image2.png", new MockFileData(System.Array.Empty<byte>()) }
            });

            // Act
            var visitor = new FileSystemVisitor(fileSystem);

            visitor.FileFound += (FileSystemInfo e) =>
            {
                if (e.Name.Contains("image2") && e.Extension == ".png")
                {
                    return Actions.Stop;
                }
                else
                {
                    return Actions.Continue;
                }
            };

            foreach (var f in visitor.Search(@"d:\test1"))
            {
                items.Add(f);
            }

            // Assert
            items.Last().Should().Be(@"d:\test1\image2.png");
        }

        [Fact]
        public void Search_FilterTxtFilesAndSkipFilteredFileByFilteredFileFoundEvent_ShouldFilterTxtFilesAndNotReturnSkipedFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\tes1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\tes1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(System.Array.Empty<byte>()) }
            });

            Func<FileSystemInfo, bool> filter = (FileSystemInfo e) => e.Extension == ".txt";

            // Act
            var visitor = new FileSystemVisitor(fileSystem, filter);

            visitor.FilteredFileFound += (FileSystemInfo e) =>
            {
                if (e.Name.Contains("im"))
                {
                    return Actions.Skip;
                }
                else
                {
                    return Actions.Continue;
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
            var items = new List<string>();

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\test1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\test1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(System.Array.Empty<byte>()) }
            });

            fileSystem.AddDirectory(@"d:\test1\dir1");
            fileSystem.AddDirectory(@"d:\test1\dir2");
            fileSystem.AddDirectory(@"d:\test1\test1_1\dir3");
            fileSystem.AddDirectory(@"d:\test1\test1_1\dir4");

            Func<FileSystemInfo, bool> filter = (FileSystemInfo e) => e.Name.Contains("test");

            // Act
            var visitor = new FileSystemVisitor(fileSystem, filter);

            visitor.FilteredDirectoryFound += (FileSystemInfo e) =>
            {
                if (e.Name.Contains("dir3"))
                {
                    return Actions.Stop;
                }
                else
                {
                    return Actions.Continue;
                }
            };

            foreach (var f in visitor.Search(@"d:\test1"))
            {
                items.Add(f);
            }

            // Assert
            items.Select(i => i.Substring(i.LastIndexOf('\\'))).Should().NotContain("test");
            items.Last().Should().Be(@"d:\test1\test1_1\dir3");
        }

        [Fact]
        public void Search_NullPath_ArgumentNullException()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\test1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\test1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(System.Array.Empty<byte>()) }
            });

            // Act
            var visitor = new FileSystemVisitor(fileSystem);
            Func<IEnumerable<string>> searchFunc = () => visitor.Search(null);

            // Assert
            searchFunc.Enumerating().Should().Throw<ArgumentNullException>()
                .WithMessage("Directory path is null")
                .And.ParamName.Should().Be(null);
        }

        [Fact]
        public void Search_NotExistPath_ArgumentExceptionWithMessage()
        {
            // Arrange
            var notExistPath = @"d:\notExist";

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\test1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\test1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(System.Array.Empty<byte>()) }
            });

            // Act
            var visitor = new FileSystemVisitor(fileSystem);
            Func<IEnumerable<string>> searchFunc = () => visitor.Search(notExistPath);

            // Assert
            searchFunc.Enumerating().Should().Throw<ArgumentException>()
                .WithMessage($"Selected directory is not exist (Parameter '{notExistPath}')")
                .And.ParamName.Should().Be(notExistPath);
        }

        [Fact]
        public void Search_EmptyPath_ArgumentExceptionWithMessage()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"d:\test1\test1_1\text.txt", new MockFileData("Text") },
                { @"d:\test1\test1_2\text2.txt", new MockFileData("Text2") },
                { @"d:\test1\im.gif", new MockFileData(System.Array.Empty<byte>()) }
            });

            // Act
            var visitor = new FileSystemVisitor(fileSystem);
            Func<IEnumerable<string>> searchFunc = () => visitor.Search(string.Empty);

            // Assert
            searchFunc.Enumerating().Should().Throw<ArgumentException>()
                .WithMessage("Directory path is empty")
                .And.ParamName.Should().Be(string.Empty);
        }
    }
}
