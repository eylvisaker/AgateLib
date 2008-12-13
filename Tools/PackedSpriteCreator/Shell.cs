using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using AgateLib.Resources;

namespace AgateLib.PackedSpriteCreator
{

    public class Shell
    {
        #region --- Console Wrapper Methods ---

        void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }
        void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
        void WriteLine()
        {
            Console.WriteLine();
        }

        string ReadLine()
        {
            return Console.ReadLine();
        }

        #endregion
        
        public delegate void CommandMethod(string[] args);

        public class CommandInfo
        {
            public string name;
            public CommandMethod method;
            public CommandAttribute attribute;
        }

        List<CommandInfo> mCommands = new List<CommandInfo>();

        public Shell()
        {
            Type mytype = this.GetType();
            var methods = mytype.GetMethods( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = (CommandAttribute[]) method.GetCustomAttributes(typeof(CommandAttribute), true);

                if (attributes.Length == 0) continue;

                CommandInfo info = new CommandInfo
                {
                    name = method.Name.ToLowerInvariant(),
                    // use method.Name here.  for some reason I get cannot convert from MethodInfo to MethodInfo error otherwise?
                    method = (CommandMethod)Delegate.CreateDelegate(typeof(CommandMethod), this, method.Name),
                    attribute = attributes[0],
                };

                mCommands.Add(info);
            }
        }

        bool done = false;
        internal void Run()
        {
			WriteLine("AgateLib Sprite Packer");
            WriteLine("Version 0.3.0");
            WriteLine();
                
            while (done == false)
            {
                Write("> ");
                string commandLine = ReadLine();
                string[] command = null;
                try
                {
                    command = SplitCommand(commandLine);
                    if (command.Length == 0)
                        continue;
                }
                catch (Exception e)
                {
                    WriteLine(e.Message);
                    continue;
                }

                CommandInfo method = FindCommand(command[0]);

                if (method == null)
                {
                    WriteLine("Unrecognized command " + command[0] + ".");
                    WriteLine("Type 'help' for a list of commands."); 
                    WriteLine();
                
                    continue;
                }

                try
                {
                    method.method(command.Skip(1).ToArray());
                }
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }

                WriteLine();
            }
        }

        private CommandInfo FindCommand(string command)
        {
            CommandInfo method = mCommands.Find(x => x.name == command.ToLowerInvariant());
            return method;
        }

        private string[] SplitCommand(string commandLine)
        {
            Regex r = new Regex(@"""[^""]*""|[^ ]*");

            var matches = r.Matches(commandLine);

            List<string> retval = new List<string>();
            foreach (var match in matches)
            {
                string text = match.ToString();

                if (string.IsNullOrEmpty(text))
                    continue;

                if (text.StartsWith("\"") && text.EndsWith("\""))
                    text = text.Substring(1, text.Length - 2);

                retval.Add(text);
            }

            return retval.ToArray();
        }

        private string CurrentDirectory
        {
            get
            {
                string currentDir = Directory.GetCurrentDirectory();

                if (currentDir.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
                    currentDir += Path.DirectorySeparatorChar;

                return currentDir;
            }
        }

        AgateResourceManager resources = new AgateResourceManager();
        SpriteResource sprite = null;

        #region --- Commands ---

        [Command(HelpText="Exits the shell.")]
        void Exit(string[] args)
        {
            done = true;
        }

        [Command]
        void Help(string[] args)
        {
            if (args.Length == 0)
            {
                WriteLine("Valid commands are: ");
                foreach (CommandInfo info in mCommands)
                {
                    WriteLine("    " + info.name);
                }
            }
            else
            {
                var cmd = FindCommand(args[0]);
                if (cmd == null)
                {
                    WriteLine("Unrecognized command " + cmd + ".");
                    return;
                }

                WriteLine();
                WriteLine("Help on command {0}:", cmd.name);
                WriteLine();
                WriteLine("Syntax: {0} {1}", cmd.name, cmd.attribute.ArgText);
                WriteLine();
                WriteLine(cmd.attribute.HelpText);
               
            }
        }

        [Command(ArgText="directory_name",HelpText="Changes directory")]
        void Cd(string[] args)
        {
            string path = string.Join(" ", args);

            try
            {
                Directory.SetCurrentDirectory(path);
                WriteLine(CurrentDirectory);

            }
            catch (DirectoryNotFoundException e)
            {
                WriteLine(e.Message);
            }
        }

        [Command(HelpText = "Prints the current directory.")]
        void Pwd(string[] args)
        {
            WriteLine(CurrentDirectory);
        }

        [Command(ArgText="[patterns]",HelpText="Lists files in the current directory")]
        void Ls(string[] args)
        {
            
            ListDirectories(args);
            ListFiles(args);
        }

        [Command(HelpText="Lists all supported formats in the current directory.")]
        void LsImg(string[] args)
        {
            Ls(new string[] { "*.png", "*.bmp", "*.jpg", "*.gif", "*.jpeg" });
        }

        [Command(ArgText = "resource_file", HelpText = "Loads resources in a resource file, combining them with the current resources.")]
        void load(string[] args)
        {
            if (args.Length == 0)
            {
                WriteLine("Please specify a resource file.");
                return;
            }
            
            resources.Load(args[0]);
            WriteLine("Loaded resources from " + args[0] + ".");
        }

        [Command(HelpText="Clears all resources in memory.")]
        void clear(string[] args)
        {
            resources = new AgateResourceManager();
        }

        [Command(ArgText="[-y] resource_file", HelpText="Saves resources to a resource file.  Specify -y before filename to force\noverwriting a destination file.")]
        void save(string[] args)
        {
            if (args.Length == 0)
                throw new Exception("Please specify a resource file.");
            if (args[0] == "-y" && args.Length == 1)
                throw new Exception("Please specify a resource file.");

            string filename = args[0];

            if (args[0] == "-y")
                filename = args[1];
            else if (File.Exists(filename))
            {
                string answer;
                int count = 0;

                do
                {
                    if (count > 0)
                        Write("Please enter yes or no.");
                    count++;

                    Write("Overwrite file " + filename + "? ");
                    
                    answer = ReadLine().ToLowerInvariant();

                    if (answer.StartsWith("n"))
                        return;

                } while (answer.StartsWith("y") == false);

            }

            resources.Filename = filename;
            resources.Save();
            WriteLine("Saved resource to " + filename + ".");
        }
        
        [Command(ArgText="sprite_name size", HelpText="Creates a new sprite in the resource file and sets it to the current sprite.")]
        void newsprite(string[] args)
        {
            if (args.Length == 0)
                throw new Exception("Please specify a name for the sprite.");
            if (args.Length == 1)
                throw new Exception("Please specify a size.");

            sprite = new SpriteResource(args[0]);
            resources.CurrentLanguage.Add(sprite);

            sprite.Size = AgateLib.Geometry.Size.FromString(args[1]);

            WriteLine("Created sprite {0} of size {1}.", args[0], args[1]);
        }

        [Command(ArgText="[sprite_name]",HelpText="Lists info on the current sprite, or on another sprite if specified.")]
        void spriteinfo(string[] args)
        {
            SpriteResource res = sprite;

            if (args.Length > 0)
            {
                AgateResource r = resources.CurrentLanguage[args[0]];
                res = r as SpriteResource;

                if (res == null)
                    throw new Exception("Resource " + args[0] + " is a " + r.GetType().Name + ".");
            }

            WriteLine();
            WriteLine("Name {0}.", res.Name);
            WriteLine("Size: {0}", res.Size);
            WriteLine("Frame Count: {0}", res.Frames.Count);

        }

        private void ListDirectories(string[] args)
        {
            List<string> dirs = new List<string>();

            if (args.Length != 0)
            {
                foreach (var arg in args)
                {
                    var thisPatternFiles = Directory.GetDirectories(CurrentDirectory, arg, SearchOption.TopDirectoryOnly);

                    for (int i = 0; i < thisPatternFiles.Length; i++)
                    {
                        // skip files already in the list
                        if (dirs.Contains(thisPatternFiles[i]))
                            continue;

                        dirs.Add(thisPatternFiles[i].Substring(CurrentDirectory.Length));
                    }
                }
            }
            else
            {
                dirs.AddRange(Directory.GetDirectories(CurrentDirectory));
            }

            dirs.Sort();

            for (int i = 0; i < dirs.Count; i++)
            {
                WriteLine(dirs[i].Substring(CurrentDirectory.Length) + Path.DirectorySeparatorChar);
            }

        }
        private void ListFiles(string[] args)
        {
            List<string> files = new List<string>();

            if (args.Length != 0)
            {
                foreach (var arg in args)
                {
                    var thisPatternFiles = Directory.GetFiles(CurrentDirectory, arg, SearchOption.TopDirectoryOnly);

                    for (int i = 0; i < thisPatternFiles.Length; i++)
                    {
                        // skip files already in the list
                        if (files.Contains(thisPatternFiles[i]))
                            continue;

                        files.Add(thisPatternFiles[i]);
                    }
                }
            }
            else
            {
                files.AddRange(Directory.GetFiles(Directory.GetCurrentDirectory()));
            }

            files.Sort();

            for (int i = 0; i < files.Count; i++)
            {
                WriteLine(files[i].Substring(CurrentDirectory.Length));
            }
        }

        #endregion
    }
}
