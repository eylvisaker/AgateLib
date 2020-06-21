using System;
using System.IO;
using System.Reflection;

namespace AgateLib.ContentAssembler
{
    public class MgcbFileCreator : FileAccessor
    {
        private readonly ProjectBuild build;
        private readonly ContentIndex index;
        private string mgcbFile;

        public MgcbFileCreator(ProjectBuild build, ContentIndex index, IFileSystem fileSystem)
            : base(fileSystem)
        {
            this.build = build;
            this.index = index;
        }

        public void CreateMgcbFile()
        {
            string contentTemplate;
            Assembly assembly = Assembly.GetExecutingAssembly();

            var names = assembly.GetManifestResourceNames();

            using (Stream stream = assembly.GetManifestResourceStream("AgateLib.ContentAssembler.Content-template.mgcb"))
            using (var reader = new StreamReader(stream))
            {
                contentTemplate = reader.ReadToEnd();
            }

            mgcbFile = Path.Combine(build.Output, "Content.mgcb");

            File.Delete(mgcbFile);

            using (StreamWriter writer = new StreamWriter(File.Open(mgcbFile, FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine(contentTemplate);

                foreach (var outputFile in index.OutputFiles)
                {
                    Append(writer, outputFile.Key.Replace("\\", "/"), outputFile.Value.MonoGame);
                }
            }
        }

        private void Append(StreamWriter writer, string filename, MonoGameParameters p)
        {
            writer.WriteLine($@"#begin {filename}");

            switch (p.AdditionType)
            {
                case AdditionType.Copy:
                    writer.WriteLine($@"/copy:{filename}");
                    break;

                case AdditionType.Import:
                    writer.WriteLine($@"/importer:{p.Importer}");
                    writer.WriteLine($@"/processor:{p.Processor}");

                    foreach (var processorParam in p.ProcessorParams)
                    {
                        writer.WriteLine($@"/processorParam:{processorParam}");
                    }

                    writer.WriteLine($@"/build:{filename}");

                    break;

                default:
                    throw new NotSupportedException();
            }
            writer.WriteLine();
        }
    }
}