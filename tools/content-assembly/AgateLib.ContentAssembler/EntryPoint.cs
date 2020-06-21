using CommandLine;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VermilionTower.ContentPipeline.Loggers;

namespace VermilionTower.ContentPipeline
{
    public class EntryPoint
    {
        public static int Main(string[] args)
        {
            int exitCode = 0;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    try
                    {
                        var builder = new ContentPipelineBuilder(options, new SystemIOFileSystem(), new ConsoleLogger());
                        builder.Run();
                    }
                    catch (ContentException)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.ToString());
                    }
                })
                .WithNotParsed(errors =>
                {
                    exitCode = 1;
                    errors.Output();
                });

            return exitCode;
        }
    }

    public class AgateLibContentAssembler : Task
    {
        public string Include { get; set; }

        public override bool Execute()
        {
            Log.LogMessage("AgateLib.ContentAssembler starting up...");

            var options = new Options();

            options.ContentBuild = Include;

            if (!File.Exists(Include))
            {
                Log.LogError($"Cannot find AgateLibContentAssembler file {Include} because it does not exist.");
                return false;
            }

            var builder = new ContentPipelineBuilder(
                options,
                new SystemIOFileSystem(),
                new TaskLogger(Log));

            try
            {
                builder.Run();
                return true;
            }
            catch (ContentException)
            {
                return false;
            }
            catch (Exception e)
            {
                Log.LogError("Unknown error." + e.ToString());
                return false;
            }
        }
    }
}
