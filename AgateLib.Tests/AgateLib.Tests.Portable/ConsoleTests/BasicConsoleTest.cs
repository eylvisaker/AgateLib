using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.Diagnostics;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Testing.ConsoleTests
{
	public class BasicConsoleTest : ISerialModelTest, ICommandVocabulary
	{
		public string Category => "Console";

		public string Name => "Basic Console Test";

		public void EntryPoint()
		{
			AgateConsole.Initialize();
			AgateConsole.CommandProcessors.Add(new LibraryVocabulary(this));

			while (Display.CurrentWindow.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.White);

				Font.AgateSans.Color = Color.Black;
				Font.AgateSans.DrawText("Press ~ key to open console.");

				Display.EndFrame();
				Core.KeepAlive();
			}
		}

		public void ModifyModelParameters(SerialModelParameters parameters)
		{
		}

		[ConsoleCommand]
		[Description("This is the apple() method.")]
		public void Apple()
		{
			AgateConsole.WriteLine("Apple() called");
		}


		[ConsoleCommand]
		[Description("Type 'remove library' to remove the command library.")]
		public void Remove(string library)
		{
			if (library != "library")
			{
				AgateConsole.Execute("help remove");
			}
			else
			{
				AgateConsole.CommandProcessors.Clear();
			}
		}
	}
}
