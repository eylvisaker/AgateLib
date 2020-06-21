using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VermilionTower.ContentPipeline.FileProcessors
{
    public class CreditsSource : FileAccessor, IFileSource
    {
        private CreditsCollection credits;

        public CreditsSource(IFileSystem fileSystem, CreditsCollection credits)
            : base(fileSystem)
        {
            this.credits = credits;
        }

        public bool IsFile => false;

        public string SourcePath => throw new NotSupportedException();

        public void Process(string outputPath)
        {
            var categories = credits.CreditsFiles
                .Select(x => x.Content)
                .GroupBy(x => x.Category);

            using (var writer = new StreamWriter(File.Open(outputPath, FileMode.Create, FileAccess.Write)))
            {
                foreach (var group in categories)
                {
                    writer.WriteLine("# " + group.Key);

                    var authors = group.SelectMany(x => x.Authors.Split(','))
                                       .Select(x => x.Trim())
                                       .Distinct();

                    foreach (var author in authors)
                    {
                        writer.WriteLine(author);
                    }

                    var urls = group.Select(x => x.Url).Distinct();

                    writer.WriteLine();

                    foreach (var url in urls)
                    {
                        writer.WriteLine(url);
                    }

                    if (urls.Any())
                        writer.WriteLine();
                }
            }
        }
    }
}
