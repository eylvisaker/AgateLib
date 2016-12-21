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
		bool IsVisible { get; set; }

		IList<ICommandLibrary> CommandLibraries { get; set; }

		void Draw();
		void WriteLine(string text);
		void WriteMessage(ConsoleMessage message);

		/// <summary>
		/// Executes the command as if the user had typed it in.
		/// </summary>
		/// <param name="command"></param>
		void Execute(string command);
	}
}
