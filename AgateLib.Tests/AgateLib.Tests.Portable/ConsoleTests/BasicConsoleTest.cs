using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;
using AgateLib.Diagnostics;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Tests.ConsoleTests
{
	public class BasicConsoleTest : IAgateTest, ICommandVocabulary
	{
		public string Category => "Console";

		public string Name => "Basic Console Test";

		public AgateConfig Configuration { get; set; }

		public void ModifySetup(IAgateSetup setup)
		{
			setup.DesiredDisplayWindowResolution = new Size(800, 600);
		}

		public void Run()
		{
			AgateConsole.Initialize();
			AgateConsole.CommandLibraries.Add(new LibraryVocabulary(this));

			while (Display.CurrentWindow.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.LightBlue);

				Font.AgateSans.Color = Color.Black;
				Font.AgateSans.DrawText("Press ~ key to open console.");

				Display.EndFrame();
				Core.KeepAlive();
			}
		}

		[ConsoleCommand("This method throws an exception.")]
		public int Throw()
		{
			return int.Parse("abc");
		}

		[ConsoleCommand("Type 'remove library' to remove the command library.\nThis method will completely remove the throw/remove commands and there is no way to get them back. So be careful if you're going to use it. That is obviously the point of the test but this is a lot of explanatory text to demonstrate the text wrapping algorithm.")]
		public void Remove(string library)
		{
			if (library != "library")
			{
				AgateConsole.Execute("help remove");
			}
			else
			{
				AgateConsole.CommandLibraries.Clear();
			}
		}
	}
}
