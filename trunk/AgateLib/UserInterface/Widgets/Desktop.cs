using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets
{
	public class Desktop : Container
	{
		private Gui gui;

		internal Desktop(Gui gui)
		{
			this.gui = gui;
		}
		
		public Window TopWindow
		{
			get { return (Window)Children[Children.Count - 1]; }
		}

		public IEnumerable<Window> Windows { get { return Children.OfType<Window>(); } }

		protected override Gui MyGui { get { return gui; } }
	}
}
