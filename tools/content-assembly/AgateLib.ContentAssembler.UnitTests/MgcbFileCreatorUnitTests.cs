using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.ContentAssembler.Mocks;
using Xunit;

namespace AgateLib.ContentAssembler
{
    public class MgcbFileCreatorUnitTests
    {
        ContentIndex index;
        ProjectBuild build = new ProjectBuild
        {
            CreateMgcb = "Content.mgcb",
            Output = "Content"
        };

        FakeFileSystem fileSystem = new FakeFileSystem();

        public MgcbFileCreatorUnitTests()
        {
            index = new ContentIndex(fileSystem);
        }

        [Fact]
        public void CopyFileInMgcb()
        {
            string filename = "abc.txt";

            CopyFile(filename);

            MgcbFileCreator creator = new MgcbFileCreator(build, index, fileSystem);

            creator.CreateMgcbFile();

            VerifyFileWasCopied(filename);
        }

        private void VerifyFileWasCopied(string filename)
        {
            var contents = fileSystem.FileContents["Content/Content.mgcb"];

            int startIndex = contents.IndexOf($"#begin {filename}");
            startIndex.Should().BeGreaterOrEqualTo(0);

            int endIndex = contents.IndexOf($"/copy:{filename}", startIndex);
            endIndex.Should().BeGreaterThan(startIndex);
        }

        private void CopyFile(string filename)
        {
            index.OutputFiles[filename] = new FileContent
            {
                MonoGame = new MonoGameParameters
                {
                    AdditionType = AdditionType.Copy
                }
            };
        }
    }
}
