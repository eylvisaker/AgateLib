using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;

namespace AgateLib.Tests.ConsoleTests
{
	public class BasicConsoleTest : IAgateTest, IVocabulary
	{
		string IVocabulary.Namespace => "";

		public string Category => "Console";

		public string Name => "Basic Console Test";

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				AgateConsole.CommandLibraries.Add(new LibraryVocabulary(this));

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.LightBlue);

					Font.AgateSans.Color = Color.Black;
					Font.AgateSans.DrawText("Press ~ key to open console.");

					Display.EndFrame();
					AgateApp.KeepAlive();
				}
			}
		}

		[ConsoleCommand("This method throws an exception.")]
		public int Throw()
		{
			return int.Parse("abc");
		}

		[ConsoleCommand("Type 'remove library' to remove the command library.\nThis method will completely remove the throw/remove commands and there is no way to get them back. So be careful if you're going to use it. That is obviously the point of the test but this is a lot of explanatory text to demonstrate the text wrapping algorithm.")]
		public void Remove(string library = null)
		{
			if (library != "library")
			{
				AgateConsole.Execute("help remove");
			}
			else
			{
				AgateConsole.CommandLibraries.Clear();
				AgateConsole.WriteLine("Removed the throw and remove commands.");
			}
		}
	}
}
