using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgateLib.ContentAssembler.FileProcessors;
using AgateLib.ContentAssembler.FolderContexts;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.ContentAssembler
{
    public class IndexBuilder : FileAccessor
    {
        private Options options;
        private ProjectBuild build;
        private readonly ILogger log;
        private IDeserializer deserializer;

        public IndexBuilder(Options options, ProjectBuild build, IFileSystem fileSystem, ILogger log) : base(fileSystem)
        {
            this.options = options;
            this.build = build;
            this.log = log;

            deserializer = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .Build();
        }

        public ContentIndex BuildContentIndex()
        {
            ContentIndex index = new ContentIndex(FileSystem);

            foreach (var inputFolder in build.Include)
            {
                AddToContentIndex(index, inputFolder);
            }


            foreach (var createIndexFile in build.Index ?? Enumerable.Empty<CreateIndex>())
            {
                CreateIndexFile(index, createIndexFile);
            }

            bool validCredits = VerifyCredits(index);

            if (!validCredits)
                throw new ContentException("Credits incomplete.");

            if (!string.IsNullOrWhiteSpace(build.CreateCredits))
            {
                index.OutputFiles.Add(build.CreateCredits,
                    new FileContent
                    {
                        Credits = CreditsRequirement.None,
                        MonoGame = new MonoGameParameters { AdditionType = AdditionType.Copy },
                        Source = new CreditsSource(FileSystem, index.Credits)
                    });
            }

            return index;
        }

        private bool VerifyCredits(ContentIndex index)
        {
            bool result = true;

            foreach (var file in index.OutputFiles)
            {
                if (file.Value.Credits == CreditsRequirement.None)
                    continue;

                bool missingCredits = index.Credits.MissingCreditsFor(file.Value);

                if (missingCredits)
                {
                    string missingCreditsMessage = $"Credits file missing or invalid for {file.Key}. Expecting one of these files to exist: "
                        + string.Join(",", index.Credits.ValidCreditsFilesFor(file.Value));

                    if (file.Value.Credits == CreditsRequirement.Require)
                    {
                        log.LogError(null, "14", null, file.Value.Source.SourcePath, 1, 1, 1, 1, missingCreditsMessage);
                        result = false;
                    }
                    else
                    {
                        log.LogWarning(null, "14", null, file.Value.Source.SourcePath, 1, 1, 1, 1, missingCreditsMessage);
                    }
                }
            }

            return result;
        }

        private void AddToContentIndex(ContentIndex index, string path)
        {
            IFolderContext context = ReadFolderContext(path);

            AddToContentIndex(index, path, context);
        }

        private void AddToContentIndex(ContentIndex index, string path, IFolderContext context)
        {
            foreach (string file in Directory.EnumerateFiles(path))
            {
                ProcessFile(index, file, context);
            }

            foreach (string directory in Directory.EnumerateDirectories(path))
            {
                var directoryName = Path.GetFileName(directory);

                if (context.ExcludeFolders.Contains(directoryName))
                    continue;

                var folderContext = ReadFolderContext(directory, context);

                AddToContentIndex(index, directory, folderContext);
            }
        }

        private void CreateIndexFile(ContentIndex index, CreateIndex file)
        {
            index.OutputFiles[file.Output] = new FileContent
            {
                Credits = CreditsRequirement.None,
                MonoGame = new MonoGameParameters { AdditionType = AdditionType.Copy },
                Source = new IndexFileCreator(FileSystem, index, file),
            };
        }

        private void ProcessFile(ContentIndex index, string file, IFolderContext context)
        {
            string fileName = Path.GetFileName(file);

            if (fileName == "content.index")
                return;

            string outputFileName = context.GetOutputFileName(fileName);

            FileInstruction instruction = context.FindMatch(outputFileName);

            switch (instruction?.As)
            {
                case FileProcessType.Texture:
                    index.OutputFiles[outputFileName] = new FileContent
                    {
                        Credits = context.Credits,
                        Source = new FileAsFileSource(file, FileSystem),
                        MonoGame = new MonoGameParameters
                        {
                            AdditionType = AdditionType.Import,
                            Importer = "TextureImporter",
                            Processor = "TextureProcessor",
                            ProcessorParams = new List<string>
                            {
                                "ColorKeyColor=255,0,255,255",
                                "ColorKeyEnabled=True",
                                "GenerateMipmaps=False",
                                "PremultiplyAlpha=True",
                                "ResizeToPowerOfTwo=False",
                                "MakeSquare=False",
                                "TextureFormat=Color",
                            },
                        }
                    };
                    break;

                case FileProcessType.Song:
                    index.OutputFiles[outputFileName] = new FileContent
                    {
                        Credits = context.Credits,
                        Source = new FileAsFileSource(file, FileSystem),
                        MonoGame = new MonoGameParameters
                        {
                            AdditionType = AdditionType.Import,
                            Importer = instruction.Importer,
                            Processor = "SongProcessor",
                            ProcessorParams = new List<string> {
                                "Quality=Best"
                            },
                        }
                    };
                    break;

                case FileProcessType.SoundEffect:
                    index.OutputFiles[outputFileName] = new FileContent
                    {
                        Credits = context.Credits,
                        Source = new FileAsFileSource(file, FileSystem),
                        MonoGame = new MonoGameParameters
                        {
                            AdditionType = AdditionType.Import,
                            Importer = instruction.Importer,
                            Processor = "SoundEffectProcessor",
                            ProcessorParams = new List<string> {
                                "Quality=Best"
                            },
                        }
                    };
                    break;

                case FileProcessType.Effect:
                    index.OutputFiles[outputFileName] = new FileContent
                    {
                        Credits = context.Credits,
                        Source = new FileAsFileSource(file, FileSystem),
                        MonoGame = new MonoGameParameters
                        {
                            AdditionType = AdditionType.Import,
                            Importer = "EffectImporter",
                            Processor = "EffectProcessor",
                            ProcessorParams = new List<string> { "DebugMode=Auto" }
                        }
                    };
                    break;

                case FileProcessType.Build:
                    index.OutputFiles[outputFileName] = new FileContent
                    {
                        Credits = context.Credits,
                        Source = new FileAsFileSource(file, FileSystem),
                        MonoGame = new MonoGameParameters
                        {
                            AdditionType = AdditionType.Import,
                            Importer = instruction.Importer,
                            Processor = instruction.Processor,
                            ProcessorParams = instruction.ProcessorParams.ToList(),
                        },
                    };
                    break;

                case FileProcessType.Credits:
                    AddCreditsFile(index, file);
                    break;

                case FileProcessType.Ignore:
                    // Do nothing.
                    break;

                case FileProcessType.Copy:
                default:
                    index.OutputFiles[outputFileName] = new FileContent
                    {
                        Source = new FileAsFileSource(file, FileSystem),
                        MonoGame = new MonoGameParameters { AdditionType = AdditionType.Copy },
                    };
                    break;
            }
        }

        private void AddCreditsFile(ContentIndex index, string file)
        {
            try
            {
                index.Credits.Add(file);
            }
            catch (YamlException e)
            {
                log.LogError("Credits", "12", "Credits", file, e.Start.Line, e.Start.Column, e.End.Line, e.End.Column,
                    $"Parsing failure. Invalid YAML or JSON file. " + e.Message);
            }
            catch (InvalidCreditsException e)
            {
                log.LogError("Credits", "12", "Credits", file, 1, 1, 1, 1,
                    $"Invalid credits file. " + e.Message);
            }
        }

        private IFolderContext ReadFolderContext(string path, IFolderContext parentContext = null)
        {
            string filePath = Path.Combine(path, "content.index");

            try
            {
                Console.WriteLine(filePath);

                string text = File.ReadAllText(filePath);

                ContentIndexFile newContext = deserializer.Deserialize<ContentIndexFile>(text);

                return new FolderContext(newContext, parentContext, path, FileSystem);
            }
            catch (FileNotFoundException)
            {
                if (parentContext == null)
                {
                    log.LogError($"Missing root content.index file in {path}.");
                    throw new ContentException($"Missing root content.index file in {path}.");
                }

                return new InheritedFolderContext(parentContext, Path.GetFileName(path), FileSystem);
            }
            catch (YamlException e)
            {
                log.LogError("YamlDeserialization",
                             "12",
                             "YAML",
                             Path.GetFullPath(filePath),
                             e.Start.Line,
                             e.Start.Column,
                             e.End.Line,
                             e.End.Column,
                             e.Message);

                throw new ContentException("Failed to read content.index file.", e);
            }
        }

    }
}