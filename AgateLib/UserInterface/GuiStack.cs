using AgateLib.InputLib;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
	public static class GuiStack
	{
		static List<Gui> mStack = new List<Gui>();

		internal static void Add(Gui gui)
		{
			mStack.Add(gui);
		}
		internal static bool Remove(Gui gui)
		{
			return mStack.Remove(gui);
		}

		public static IEnumerable<Gui> Items { get { return mStack; } }

		internal static void ListenEvent(object sender, AgateInputEventArgs args)
		{
			if (GuiEvent != null)
				GuiEvent(sender, args);
		}

		public static event EventHandler<AgateInputEventArgs> GuiEvent;
	}
}
