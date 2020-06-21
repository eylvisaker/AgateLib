using System;
using AgateLib.ContentAssembler.FileProcessors;
using FluentAssertions;
using Xunit;
using Moq;

namespace AgateLib.ContentAssembler
{
    public partial class SimpleFileSystemSetup
    {
        public class IndexBuilderUnitTests : SimpleFileSystemSetup
        {
            [Theory]
            [InlineData("cc-by/Images/CREDITS.txt")]
            [InlineData("cc-by/Images/subfolder/xyz-CREDITS.txt")]
            public void MissingCreditsFileCausesFailure(string creditsFileToRemove)
            {
                RemoveFile(creditsFileToRemove);

                Action build = () => builder.BuildContentIndex();

                build.Should().Throw<ContentException>("missing credits file should cause exception");
            }

            [Fact]
            public void IgnoredFileIsNotIncludedInOutput()
            {
                ContentIndex index = builder.BuildContentIndex();

                index.OutputFiles.Should().ContainKey("Images/pointer.png");
                index.OutputFiles.Should().ContainKey("Images/pointer.xml");
                index.OutputFiles.Should().NotContainKey("Images/pointer.pyxel");
            }

            [Fact]
            public void CreditsFileShouldBeIncludedInOutput()
            {
                ContentIndex index = builder.BuildContentIndex();

                index.OutputFiles.Should().ContainKey("credits.txt");
            }

            [Fact]
            public void FilesMappedCorrectlyToOutputFolder()
            {
                ContentIndex index = builder.BuildContentIndex();

                index.OutputFiles.Should().ContainKey("help.txt");
                index.OutputFiles.Should().ContainKey("bodyparts/face/nostrils.txt");
                index.OutputFiles.Should().NotContainKey("content.index");

                var nos = index.OutputFiles["bodyparts/face/nostrils.txt"];

                nos.MonoGame.Should().NotBeNull();
                nos.MonoGame.AdditionType.Should().Be(AdditionType.Copy);

                nos.Source.Should().BeOfType<FileAsFileSource>();
                (nos.Source as FileAsFileSource).SourcePath.Should().Be("cc0/bodyparts/face/nostrils.txt");
            }

            [Fact]
            public void ExcludedFoldersShouldNotBeInOutput()
            {
                ContentIndex index = builder.BuildContentIndex();

                index.OutputFiles.Should().NotContainKey("Images/SOURCE/0.png");
            }

            [Fact]
            public void ShaderFileMappedForImport()
            {
                ContentIndex index = builder.BuildContentIndex();

                index.OutputFiles.Should().ContainKey("Shaders/Sprites.fx");

                var sprites = index.OutputFiles["Shaders/Sprites.fx"];

                sprites.MonoGame.AdditionType.Should().Be(AdditionType.Import);
                sprites.MonoGame.Importer.Should().Be("EffectImporter");
                sprites.MonoGame.Processor.Should().Be("EffectProcessor");
                sprites.MonoGame.ProcessorParams.Should().HaveCount(1);
                sprites.MonoGame.ProcessorParams[0].Should().Be("DebugMode=Auto");
            }

            [Fact]
            public void PngFileMappedForImport()
            {
                ContentIndex index = builder.BuildContentIndex();

                index.OutputFiles.Should().ContainKey("Images/pointer.png");

                var pointer = index.OutputFiles["Images/pointer.png"];

                pointer.MonoGame.AdditionType.Should().Be(AdditionType.Import);
                pointer.MonoGame.Importer.Should().Be("TextureImporter");
                pointer.MonoGame.Processor.Should().Be("TextureProcessor");
                pointer.MonoGame.ProcessorParams.Should().HaveCount(7);
                pointer.MonoGame.ProcessorParams.Should().Contain("ColorKeyColor=255,0,255,255");
                pointer.MonoGame.ProcessorParams.Should().Contain("ColorKeyEnabled=True");
                pointer.MonoGame.ProcessorParams.Should().Contain("GenerateMipmaps=False");
                pointer.MonoGame.ProcessorParams.Should().Contain("PremultiplyAlpha=True");
                pointer.MonoGame.ProcessorParams.Should().Contain("ResizeToPowerOfTwo=False");
                pointer.MonoGame.ProcessorParams.Should().Contain("MakeSquare=False");
                pointer.MonoGame.ProcessorParams.Should().Contain("TextureFormat=Color");
            }

            [Fact]
            public void JpgFileMappedForImport()
            {
                ContentIndex index = builder.BuildContentIndex();

                index.OutputFiles.Should().ContainKey("Images/subfolder/xyz.jpg");

                var pointer = index.OutputFiles["Images/subfolder/xyz.jpg"];

                pointer.MonoGame.AdditionType.Should().Be(AdditionType.Import);
                pointer.MonoGame.Importer.Should().Be("TextureImporter");
                pointer.MonoGame.Processor.Should().Be("TextureProcessor");
                pointer.MonoGame.ProcessorParams.Should().HaveCount(7);
                pointer.MonoGame.ProcessorParams.Should().Contain("ColorKeyColor=255,0,255,255");
                pointer.MonoGame.ProcessorParams.Should().Contain("ColorKeyEnabled=True");
                pointer.MonoGame.ProcessorParams.Should().Contain("GenerateMipmaps=False");
                pointer.MonoGame.ProcessorParams.Should().Contain("PremultiplyAlpha=True");
                pointer.MonoGame.ProcessorParams.Should().Contain("ResizeToPowerOfTwo=False");
                pointer.MonoGame.ProcessorParams.Should().Contain("MakeSquare=False");
                pointer.MonoGame.ProcessorParams.Should().Contain("TextureFormat=Color");
            }
        }
    }
}
