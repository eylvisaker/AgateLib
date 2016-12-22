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

		public void ModifyModelParameters(SerialModelParameters parameters)
		{
		}

		[ConsoleCommand("This method throws an exception.")]
		public int Throw()
		{
			return int.Parse("abc");
		}


		[ConsoleCommand("Type 'remove library' to remove the command library.")]
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
