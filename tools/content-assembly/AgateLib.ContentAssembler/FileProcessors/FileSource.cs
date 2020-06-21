using System;
using System.Collections.Generic;
using System.Text;

namespace VermilionTower.ContentPipeline.FileProcessors
{

    public interface IFileSource
    {
        bool IsFile { get; }

        string SourcePath { get; }

        void Process(string outputPath);
    }

    public class FileAsFileSource : FileAccessor, IFileSource
    {
        public FileAsFileSource(string sourcePath, IFileSystem fileSystem) : base(fileSystem)
        {
            SourcePath = sourcePath;
        }

        public string SourcePath { get; set; }

        public bool IsFile => true;

        public void Process(string outputPath)
        {
            if (File.Exists(outputPath))
            {
                if (File.GetLastWriteTimeUtc(outputPath) >=
                    File.GetLastWriteTimeUtc(SourcePath))
                {
                    return;
                }
            }

            Console.WriteLine($"Copying: {SourcePath} -> {outputPath}");

            var dir = Path.GetDirectoryName(outputPath);

            Directory.CreateDirectory(dir);

            File.Delete(outputPath);
            File.Copy(SourcePath, outputPath);
        }

        public override string ToString()
            => "Copy From: " + SourcePath;
    }
}
