using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Venus.Fulfillment;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus
{
	public class VenusLayoutEngine : IGuiLayoutEngine
	{
		private VenusWidgetAdapter adapter;

		public VenusLayoutEngine(VenusWidgetAdapter adapter)
		{
			this.adapter = adapter;
		}

		public void UpdateLayout(Gui gui)
		{

		}
	}

}
