using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.InputLib;

namespace AgateLib.Diagnostics
{
	public interface IAgateConsole : IInputHandler
	{
		ICommandProcessor CommandProcessor { get; set; }

		bool IsVisible { get; set; }

		void Draw();
		void WriteLine(string v);
		void WriteMessage(ConsoleMessage message);
	}
}
