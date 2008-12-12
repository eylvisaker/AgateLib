using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Resources;

namespace AgateLib.PackedSpriteCreator
{
	class SpriteCreator
	{
		#region --- Static methods ---

		static string exeName =
			Path.GetFileName(System.Reflection.Assembly.GetCallingAssembly()
				.Location);

		static void Main(string[] args)
		{
			try
			{
				new SpriteCreator().Run(args);
			}
			catch (InvalidUsageException e)
			{
                Usage(e.Message);
                System.Console.ReadKey(true);
			}
		}		

		private static void Usage(string message)
		{
			WriteLine("AgateLib Packed Sprite Creator");
			WriteLine("Usage:");
			WriteLine("    " + exeName + " [options] [input file list]");
			WriteLine();
			
			WriteLine("Options:");
            WriteLine("    -shell         Starts in shell mode.  If this option is specified, all");
            WriteLine("                   others are ignored.");
			WriteLine("    -o [filename]  Specifies an output resource file in XML format.");
			WriteLine("    -n [name]      Specifies the name of the sprite.");
			WriteLine("    -t [time]      Specifies the amount of time in milliseconds for each frame.");
            WriteLine("    -T [color]     Specifies a color in RRGGBB format to convert to transparent.");
            WriteLine();

			WriteLine("The default mode is to create a single packed sprite image from the input");
			WriteLine("files.  You must specify the -o and -s options when creating a packed sprite.");
			WriteLine();
			WriteLine("Sprite frames can be specified as a single number for square frames, or as");
			WriteLine("a width,height format.");
			WriteLine();
			WriteLine(message);
		}

		static void Write(string format, params object[] args)
		{
			Console.Write(format, args);
		}
		static void WriteLine(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}
		static void WriteLine()
		{
			Console.WriteLine();
		}

		#endregion

		List<string> images = new List<string>();
        string mOutputXmlFile;
        
        string OutputFile
        {
            get
            {
                const string extension = ".png";

                if (OutputXmlFile.ToLowerInvariant().EndsWith(".xml"))
                    return OutputXmlFile.Remove(OutputXmlFile.Length - 4, 4) + extension;
                else
                    return OutputXmlFile + extension;
            }
        }
        string OutputXmlFile
        {
            get { return mOutputXmlFile; }
            set
            {
                if (value.ToLowerInvariant().EndsWith(".xml") == false)
                    value += ".xml";

                mOutputXmlFile = value;
            }
        }
		string spriteName;
		double frameTime = 50;
		Size frameSize;
		Size outputSize = new Size(256, 256);
        bool doShell;

		private void Run(string[] args)
		{
			ParseCommandLine(args);

			CheckInputParameters();

            using (AgateSetup setup = new AgateSetup())
            {
                setup.InitializeDisplay(AgateLib.Drivers.DisplayTypeID.Reference);
                if (setup.WasCanceled)
                    return;

                if (doShell)
                {
                    Shell shell = new Shell();
                    shell.Run();
                }
                else
                {
                    ProcessImages();
                    WriteLine("Done.  Hit any key to exit.");
                    System.Console.ReadKey(true);
                }
            }
			
		}

		private void ProcessImages()
		{
			SpriteData sprite = new SpriteData();
			ConstructSpriteData(sprite);

			PixelBuffer imageData = PackImages(sprite);
			imageData.SaveTo(OutputFile, ImageFileFormat.Png);

			AgateResourceManager resources = new AgateResourceManager();
			AddSpriteData(resources, sprite);

			resources.Filename = OutputXmlFile;
			resources.Save();
		}

		private PixelBuffer PackImages(SpriteData sprite)
		{
			var p = new Utility.SurfacePacker.RectPacker<SpriteFrameData>(outputSize);

			for (int i = 0; i < sprite.Frames.Count; i++)
			{
				var frame = sprite.Frames[i];

				p.QueueObject(frame.ImageData.Size, frame);
			}

			p.AddQueue();

			PixelBuffer retval = new PixelBuffer(sprite.Frames[0].ImageData.PixelFormat, outputSize);

			foreach (var rect in p)
			{
				rect.Tag.PackedLocation = rect.Rect;

				SpriteFrameData data = rect.Tag;

				retval.CopyFrom(data.ImageData, new Rectangle(Point.Empty, data.ImageData.Size),
					rect.Tag.PackedLocation.Location, false);

			}

			return retval;
		}

		private void AddSpriteData(AgateResourceManager resources, SpriteData sprite)
		{
			SpriteResource res = new SpriteResource(spriteName);

			foreach (SpriteFrameData frame in sprite.Frames)
			{
				SpriteResource.SpriteFrameResource frameRes = new SpriteResource.SpriteFrameResource();

				frameRes.Bounds = frame.PackedLocation;
				frameRes.Offset = frame.Offset;
				frameRes.Filename = OutputFile;

				res.Frames.Add(frameRes);
			}

			res.Name = spriteName;
			res.Filename = OutputFile;
			res.Size = frameSize;
			res.TimePerFrame = frameTime;
			res.Packed = true;

			resources.CurrentLanguage.Add(res);
		}
		private void ConstructSpriteData(SpriteData sprite)
		{
			for (int i = 0; i < images.Count; i++)
			{
				Surface surf;

				try
				{
					surf = new Surface(images[i]);
				}
				catch (FileNotFoundException)
				{
					WriteLine("WARNING: Could not find file " + images[i]);
					continue;
				}

				sprite.Frames.AddRange(GetSpriteData(surf, frameSize).ToArray());
			}
		}

		private IEnumerable<SpriteFrameData> GetSpriteData(Surface surf, Size frameSize)
		{
			PixelBuffer pixels = surf.ReadPixels();

			Point location = Point.Empty;
			bool done = false;

			while (done == false)
			{
				SpriteFrameData frame = new SpriteFrameData()
				{
					SourceData = pixels,
					SourceRect = new Rectangle(location, frameSize)
				};

				frame.Trim();

				if (frame.IsBlank == false)
					yield return frame;

				// update the location sprite frames are drawn from
				location.X += frameSize.Width;

				if (location.X + frameSize.Width > pixels.Width)
				{
					location.Y += frameSize.Height;
					location.X = 0;
				}

				if (location.Y + frameSize.Height > pixels.Height)
					done = true;
			}

		}

		private void CheckInputParameters()
		{
            // ignore all other parameters if we are going to shell mode.
            if (doShell)
                return;

			if (images.Count == 0)
				throw new InvalidUsageException();
			if (string.IsNullOrEmpty(OutputFile))
				throw new InvalidUsageException("You did not specify an output file.");
			if (frameSize == Size.Empty)
				throw new InvalidUsageException("You did not specify a frame size.");
            
			if (string.IsNullOrEmpty(spriteName))
				spriteName = Path.GetFileNameWithoutExtension(OutputFile);
		}
        private void ParseCommandLine(string[] args)
		{
			int i;

			for (i = 0; i < args.Length; i++)
			{
				if (args[i] == "--")
					break;

				if (args[i].StartsWith("-"))
				{
					try
					{
						ProcessArgument(args, ref i);
					}
					catch (Exception e)
					{
						throw new InvalidUsageException("An exception was thrown in parsing argument " + args[i], e);
					}
					continue;
				}

				break;
			}

			for (; i < args.Length; i++)
				images.Add(args[i]);
		}
		private void ProcessArgument(string[] args, ref int i)
		{
            if (args[i].Equals("-o"))
                OutputXmlFile = args[++i];
            else if (args[i].Equals("-s"))
            {
                int result;
                i++;
                if (int.TryParse(args[i], out result))
                {
                    frameSize = new Size(result, result);
                }
                else
                {
                    string[] array = args[i].Split(',', 'x', ':');

                    if (array.Length != 2) throw new InvalidUsageException("Could not parse size.");

                    try
                    {
                        frameSize = new Size(int.Parse(array[0]), int.Parse(array[1]));

                        if (frameSize.Width <= 0 || frameSize.Height <= 0)
                            throw new InvalidUsageException("Frame size was negative.");
                    }
                    catch (FormatException)
                    {
                        throw new InvalidUsageException("Could not parse size " + args[i]);
                    }
                }
            }
            else if (args[i].Equals("-n"))
            {
                spriteName = args[++i];
            }
            else if (args[i].Equals("-t"))
            {
                if (double.TryParse(args[++i], out frameTime) == false)
                    throw new InvalidUsageException("Could not parse frametime: " + args[i]);
            }
            else if (args[i].Equals("-shell"))
            {
                doShell = true;
            }
            else
                throw new InvalidUsageException("Unrecognized argument: " + args[i]);
		}

	}
}
