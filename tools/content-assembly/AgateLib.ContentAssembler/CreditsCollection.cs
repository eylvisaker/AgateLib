using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VermilionTower.ContentPipeline
{
    public class CreditsCollection : FileAccessor
    {
        private List<CreditsFile> credits = new List<CreditsFile>();
        private IDeserializer deserialier;

        public CreditsCollection(IFileSystem fileSystem) : base(fileSystem)
        {
            deserialier = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .Build();
        }

        public List<CreditsFile> CreditsFiles => credits;

        public CreditsFile Add(string creditsFilePath)
        {
            var result = new CreditsFile
            {
                SourcePath = creditsFilePath,
                Content = deserialier.Deserialize<CreditsFileContents>(
                    File.ReadAllText(creditsFilePath))
            };

            VerifyCreditsContent(result);

            credits.Add(result);

            return result;
        }

        private void VerifyCreditsContent(CreditsFile result)
        {
            string message = "";

            if (result.Content == null)
                throw new InvalidCreditsException("Credits file should have the following fields: authors, category, license.");

            if (string.IsNullOrWhiteSpace(result.Content.Authors))
                message += "Authors should not be blank. ";

            if (string.IsNullOrWhiteSpace(result.Content.Category))
                message += "Category should not be blank. ";

            if (string.IsNullOrWhiteSpace(result.Content.License))
                message += "License should not be blank. ";

            if (message.Length > 0)
                throw new InvalidCreditsException(message);
        }

        public bool MissingCreditsFor(FileContent value)
        {
            if (!value.Source.IsFile)
                return false;

            string sourcePath = value.Source.SourcePath;
            string dir = Path.GetDirectoryName(sourcePath);

            IEnumerable<string> searchCreditsFiles = ValidCreditsFilesFor(value);

            string dirCredits = Path.Combine(dir, "CREDITS.txt");

            string fileCredits = Path.Combine(
                dir,
                Path.GetFileNameWithoutExtension(sourcePath) + "-CREDITS.txt");

            if (credits.Any(x => x.SourcePath.Equals(dirCredits)))
                return false;

            if (credits.Any(x => x.SourcePath.Equals(fileCredits)))
                return false;

            return true;
        }

        public IEnumerable<string> ValidCreditsFilesFor(FileContent value)
        {
            if (!value.Source.IsFile)
                yield break;

            string sourcePath = value.Source.SourcePath;
            string dir = Path.GetDirectoryName(sourcePath);

            // Credits file for whole directory
            yield return Path.Combine(dir, "CREDITS.txt");

            // Credits file specific to this file.
            yield return Path.Combine(
                dir,
                Path.GetFileNameWithoutExtension(sourcePath) + "-CREDITS.txt");
        }
    }

    public class CreditsFile
    {
        public string SourcePath { get; set; }
        public CreditsFileContents Content { get; set; }
    }

    public class CreditsFileContents
    {
        public string License { get; set; }
        public string Url { get; set; }
        public string Authors { get; set; }
        public string Category { get; set; }
    }
}