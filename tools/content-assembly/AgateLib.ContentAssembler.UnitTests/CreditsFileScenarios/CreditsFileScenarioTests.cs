using System;
using VermilionTower.ContentPipeline.FileProcessors;
using FluentAssertions;
using Xunit;
using Moq;
using System.Collections.Generic;
using VermilionTower.ContentPipeline.Mocks;
using System.Linq;

namespace VermilionTower.ContentPipeline.CreditsFileScenarios
{
    public class CreditsFileScenarioTests
    {
        private FakeFileSystem fileSystem = new FakeFileSystem();
        private IndexBuilder builder;
        private Options options;
        private ProjectBuild build;
        private MockLogger log = new MockLogger();

        public CreditsFileScenarioTests()
        {
            options = new Options { ContentBuild = "unusedInTests" };
            build = new ProjectBuild
            {
                Output = "Content",
                Include = { "cc-by" },
                CreateMgcb = "Content.mgcb",
                CreateCredits = "credits.txt",
            };

            //AddFile("cc-by/Images/pointer.xml");
            //AddFile("cc-by/Images/pointer.pyxel");
            //AddFile("cc-by/Images/subfolder/xyz.png");
            //AddFile("cc-by/Images/subfolder/xyz1.png");
            //AddFile("cc-by/Images/subfolder/xyz2.png");
            //AddFile("cc-by/Images/subfolder/xyz-CREDITS.txt", ValidCreditsFile());
            //AddFile("cc-by/Images/subfolder/xyz1-CREDITS.txt", ValidCreditsFile());
            //AddFile("cc-by/Images/subfolder/xyz2-CREDITS.txt", ValidCreditsFile());
            //AddFile("cc-by/Images/CREDITS.txt", ValidCreditsFile());
            AddFile("cc-by/content.index", ContentIndexForCCBY());

            builder = new IndexBuilder(options, build, fileSystem, log.Object);

        }

        [Fact]
        public void DirectoryCreditsFileAppliesCorrectly()
        {
            AddFile("cc-by/Images/categoricallyCreditedImage.png");
            AddFile("cc-by/Images/CREDITS.txt", ValidCreditsFile());

            ExpectSuccessfulBuild();
        }

        [Fact]
        public void CorrectlyCreditedImageIsIncluded()
        {
            AddFile("cc-by/Images/correctlyCreditedImage.png");
            AddFile("cc-by/Images/correctlyCreditedImage-CREDITS.txt", ValidCreditsFile());

            ExpectSuccessfulBuild();
        }

        [Fact]
        public void BuildFailsForUncreditedFile()
        {
            AddFile("cc-by/Images/uncreditedFile.png");

            Exception result = ExpectFailedBuild();

            result.Message.Should().Be("Credits incomplete.");

            log.ErrorLogs.Count.Should().Be(1);
            log.ErrorLogs.First().FormattedMessage.Should().Be(
                "Credits file missing or invalid for Images/uncreditedFile.png. Expecting one of these files to exist: cc-by/Images/CREDITS.txt,cc-by/Images/uncreditedFile-CREDITS.txt");
        }

        [Fact]
        public void BuildFailsForEmptyCreditsFile()
        {
            AddFile("cc-by/Images/emptyCredits.png");
            AddFile("cc-by/Images/emptyCredits-CREDITS.txt", "");

            Exception result = ExpectFailedBuild();

            result.Message.Should().Be("Credits incomplete.");

            log.ErrorLogs.Count.Should().Be(2);

            log.ErrorLogs.First().Message.Should().StartWith("Invalid credits file. Credits file should have the following fields: authors, category, license.");
            log.ErrorLogs.Last().FormattedMessage.Should().Be(
                "Credits file missing or invalid for Images/emptyCredits.png. Expecting one of these files to exist: cc-by/Images/CREDITS.txt,cc-by/Images/emptyCredits-CREDITS.txt");
        }

        [Theory]
        [InlineData("Authors",  "       ", "category", "license")]
        [InlineData("Category", "authors", "        ", "license")]
        [InlineData("License",  "authors", "category", "       ")]
        public void BuildFailsForCreditsFileMissing(string emptyField, string authors, string category, string license)
        {
            AddFile("cc-by/Images/emptyCredits.png");
            AddFile("cc-by/Images/emptyCredits-CREDITS.txt", $@"
authors: {authors}
category: {category}
license: {license}");

            Exception result = ExpectFailedBuild();

            result.Message.Should().Be("Credits incomplete.");

            log.ErrorLogs.Count.Should().Be(2);

            log.ErrorLogs.First().Message.Should().StartWith($"Invalid credits file. {emptyField} should not be blank.");
            log.ErrorLogs.Last().FormattedMessage.Should().Be(
                "Credits file missing or invalid for Images/emptyCredits.png. Expecting one of these files to exist: cc-by/Images/CREDITS.txt,cc-by/Images/emptyCredits-CREDITS.txt");
        }

        [Fact]
        public void BuildFailsForInvalidCreditsFile()
        {
            AddFile("cc-by/Images/emptyCredits.png");
            AddFile("cc-by/Images/emptyCredits-CREDITS.txt", "invalid content");

            Exception result = ExpectFailedBuild();

            result.Message.Should().Be("Credits incomplete.");

            log.ErrorLogs.Count.Should().Be(2);

            log.ErrorLogs.First().Message.Should().StartWith("Parsing failure. Invalid YAML or JSON file. ");
            log.ErrorLogs.Last().FormattedMessage.Should().Be(
                "Credits file missing or invalid for Images/emptyCredits.png. Expecting one of these files to exist: cc-by/Images/CREDITS.txt,cc-by/Images/emptyCredits-CREDITS.txt");
        }

        [Fact]
        public void ExcludedFileDoesNotNeedCredits()
        {
            AddFile("cc-by/Images/SOURCE/ignored-file.png");

            ExpectSuccessfulBuild();
        }

        private string ValidCreditsFile()
        {
            return @"
license: CC-BY 3.0
url: https://opengameart.org/some-url
authors: Davey Jones
category: Graphics";
        }

        private string ContentIndexForCCBY()
        {
            return @"
credits: require
files:
- pattern: .*\.pyxel
  as: ignore
exclude-folders:
- SOURCE";
        }

        protected void AddFile(string fileName, string content = null)
        {
            fileSystem.AddFile(fileName, content ?? fileName);
        }

        private void ExpectSuccessfulBuild()
        {
            builder.BuildContentIndex();
        }

        private Exception ExpectFailedBuild(string reason = "")
        {
            try
            {
                builder.BuildContentIndex();

                throw new InvalidOperationException("Expected indexing failure did not happen because " + reason);
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
