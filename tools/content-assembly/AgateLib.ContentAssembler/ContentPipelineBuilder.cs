using System;
using System.IO;
using CommandLine;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VermilionTower.ContentPipeline
{
    public class ContentPipelineBuilder : FileAccessor
    {
        private Options options;
        private readonly ILogger log;
        private ProjectBuild build;

        public ContentPipelineBuilder(Options options, IFileSystem fileSystem, ILogger log) : base(fileSystem)
        {
            this.options = options;
            this.log = log;
        }

        public void Run()
        {
            FileSystem.PathRoot = Path.GetDirectoryName(options.ContentBuild);

            ReadBuildFile();

            ContentIndex index = new IndexBuilder(options, build, FileSystem, log).BuildContentIndex();
            new FileProcessor(build, index, FileSystem).ProcessFiles();
            new MgcbFileCreator(build, index, FileSystem).CreateMgcbFile();
        }

        private void ReadBuildFile()
        {
            string fileName = options.ContentBuild;

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .Build();

            try
            {
                build = deserializer.Deserialize<ProjectBuild>(File.ReadAllText(fileName));
            }
            catch (YamlException e)
            {
                log.LogError("YamlDeserialization",
                             "13",
                             "YAML",
                             Path.GetFullPath(fileName),
                             e.Start.Line,
                             e.Start.Column,
                             e.End.Line,
                             e.End.Column,
                             e.Message);

                throw new ContentException("Failed to deserialize content.build file.", e);
            }
        }
    }
}
