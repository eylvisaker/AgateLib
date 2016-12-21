using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace AgateLib.Configuration.State
{
	class ConsoleState
	{
		internal IAgateConsole Instance;
		internal Color BackgroundColor;
		internal Color EntryColor;
		internal Color TextColor;
		internal KeyCode VisibleToggleKey;
		internal IFont Font;
	}
}
