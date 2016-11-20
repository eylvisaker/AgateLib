using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Fulfillment
{
	public class FacetWidgetPropertyMapValue
	{
		public string Name { get; set; }
		public Action<Widget> Assign { get; set; }
	}
}
