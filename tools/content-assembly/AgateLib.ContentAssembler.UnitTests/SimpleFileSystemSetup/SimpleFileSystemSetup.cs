using System;
using AgateLib.ContentAssembler.FileProcessors;
using FluentAssertions;
using Xunit;
using Moq;
using System.Collections.Generic;
using AgateLib.ContentAssembler.Mocks;

namespace AgateLib.ContentAssembler
{
    public partial class SimpleFileSystemSetup
    {
        private FakeFileSystem fileSystem = new FakeFileSystem();
        private IndexBuilder builder;
        private ProjectBuild build;
        private Mock<ILogger> log = new Mock<ILogger>();

        public SimpleFileSystemSetup()
        {
            Options options = new Options { ContentBuild = "unusedInTests" };
            build = new ProjectBuild
            {
                Output = "Content",
                Include = { "cc0", "cc-by" },
                CreateMgcb = "Content.mgcb",
                CreateCredits = "credits.txt",
            };

            AddFile("cc0/content.index",
                @"credits: none");

            AddFile("cc0/help.txt");
            AddFile("cc0/bodyparts/face/nostrils.txt");
            AddFile("cc0/bodyparts/face/nostrils.png");
            AddFile("cc0/Shaders/Sprites.fx");
            AddFile("cc-by/Images/SOURCE/0.png");
            AddFile("cc-by/Images/pointer.png");
            AddFile("cc-by/Images/pointer.xml");
            AddFile("cc-by/Images/pointer.pyxel");
            AddFile("cc-by/Images/pointer1.png");
            AddFile("cc-by/Images/subfolder/xyz.jpg");
            AddFile("cc-by/Images/subfolder/xyz1.png");
            AddFile("cc-by/Images/subfolder/xyz2.png");
            AddFile("cc-by/Images/subfolder/xyz-CREDITS.txt", CreditsFileCCBY());
            AddFile("cc-by/Images/subfolder/xyz1-CREDITS.txt", CreditsFileCCBY());
            AddFile("cc-by/Images/subfolder/xyz2-CREDITS.txt", CreditsFileCCBY());
            AddFile("cc-by/Images/CREDITS.txt", CreditsFileCCBY());
            AddFile("cc-by/content.index", ContentIndexForCCBY());

            builder = new IndexBuilder(options, build, fileSystem, log.Object);
        }

        private string CreditsFileCCBY()
        {
            return @"license: CC-BY 3.0
url: https://opengameart.org/some-url
authors: Davey Jones
category: Graphics";
        }

        private string ContentIndexForCCBY()
        {
            return @"credits: require
files:
- pattern: .*\.pyxel
  as: ignore
- pattern: .*\.jpg
  as: texture
exclude-folders:
- SOURCE";
        }

        protected void AddFile(string fileName, string content = null)
        {
            fileSystem.AddFile(fileName, content ?? fileName);
        }

        protected void RemoveFile(string fileName) {
            fileSystem.RemoveFile(fileName);
        }
    }
}
