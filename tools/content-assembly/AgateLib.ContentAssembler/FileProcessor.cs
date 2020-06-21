using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VermilionTower.ContentPipeline
{
    public class FileProcessor : FileAccessor
    {
        private readonly ProjectBuild build;
        private readonly ContentIndex index;

        public FileProcessor(ProjectBuild build, ContentIndex index, IFileSystem fileSystem)
            : base(fileSystem)
        {
            this.build = build;
            this.index = index;
        }

        public void ProcessFiles()
        {
            foreach (var outputFile in index.OutputFiles.Keys)
            {
                var file = index.OutputFiles[outputFile];

                string outputPath = Path.Combine(build.Output, outputFile);

                file.Source.Process(outputPath);
            }
        }
    }
}
