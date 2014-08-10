using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
	public interface IGuiLayoutEngine
	{
		void RedoLayout(Gui gui);
	}
}
