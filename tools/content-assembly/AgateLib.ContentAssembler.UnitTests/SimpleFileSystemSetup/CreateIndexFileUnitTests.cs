using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VermilionTower.Content;
using Xunit;

namespace VermilionTower.ContentPipeline
{
    public partial class SimpleFileSystemSetup
    {
        public class CreateIndexFileUnitTests : SimpleFileSystemSetup
        {
            public CreateIndexFileUnitTests()
            {
                build.Index = new List<CreateIndex>
                {
                    new CreateIndex 
                    {
                        Output = "Images/images.index",
                        Filter = "*.png|*.bmp|*.jpg",
                        Recurse = true,
                    },
                    new CreateIndex 
                    {
                        Output = "Images/subfolder/subimages.index",
                        Filter = "*.png|*.jpg",
                        Recurse = true,
                    }
                };
            }

            [Theory]
            [InlineData("Images/images.index")]
            [InlineData("Images/subfolder/subimages.index")]
            public void IndexFilesShouldBeInOutput(string expectedFile)
            {
                ContentIndex index = builder.BuildContentIndex();

                index.OutputFiles.Should().ContainKey(expectedFile);
            }

            [Theory]
            [InlineData("Images/pointer")]
            [InlineData("Images/pointer1")]
            [InlineData("Images/subfolder/xyz")]
            [InlineData("Images/subfolder/xyz1")]
            [InlineData("Images/subfolder/xyz2")]
            public void RootIndexFileShouldHaveAllImages(string expectedFile)
            {
                IndexedFile[] files = GetContentsOfIndex("Images/images.index");

                files.Select(x => x.Path).Should().Contain(expectedFile);

                var indexRecord = files.First(x => x.Path == expectedFile);

                indexRecord.Type.Should().Be(ContentType.Texture);
            }


            [Theory]
            [InlineData("bodyparts/face/nostrils.txt")]
            [InlineData("bodyparts/face/nostrils")]
            [InlineData("Images/pointer.xml")]
            public void RootIndexFileShouldNotContainOtherFiles(string unexpectedFile)
            {
                IndexedFile[] files = GetContentsOfIndex("Images/images.index");

                files.Select(x => x.Path).Should().NotContain(unexpectedFile);
            }


            [Theory]
            [InlineData("Images/subfolder/xyz")]
            [InlineData("Images/subfolder/xyz1")]
            [InlineData("Images/subfolder/xyz2")]
            public void SubfolderIndexFileShouldHaveAllImages(string expectedFile)
            {
                IndexedFile[] files = GetContentsOfIndex("Images/subfolder/subimages.index");

                files.Select(x => x.Path).Should().Contain(expectedFile);

                var indexRecord = files.First(x => x.Path == expectedFile);

                indexRecord.Type.Should().Be(ContentType.Texture);
            }


            [Theory]
            [InlineData("Images/pointer")]
            [InlineData("Images/pointer1")]
            [InlineData("bodyparts/face/nostrils.txt")]
            [InlineData("bodyparts/face/nostrils")]
            [InlineData("Images/pointer.xml")]
            public void SubfolderIndexFileShouldNotContainOtherFiles(string unexpectedFile)
            {
                IndexedFile[] files = GetContentsOfIndex("Images/subfolder/subimages.index");

                files.Select(x => x.Path).Should().NotContain(unexpectedFile);
            }


            private IndexedFile[] GetContentsOfIndex(string outputFilename)
            {
                var index = builder.BuildContentIndex();

                index.OutputFiles[outputFilename].Source.Process(outputFilename);

                var contents = fileSystem.FileContents[outputFilename];

                var files = JsonConvert.DeserializeObject<IndexedFile[]>(contents);

                return files;
            }
        }
    }
}
