using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Testing.ConsoleTests
{
	public class BasicConsoleTest : ISerialModelTest
	{
		public string Category => "Console";

		public string Name => "Basic Console Test";

		public void EntryPoint()
		{
			AgateConsole.Initialize();

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
	}
}
