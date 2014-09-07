using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets.Extensions
{
	public static class WidgetExtensions
	{
		public static bool ChildHasMouseIn(this Container c)
		{
			return c.Descendants.Any(x => x.MouseIn);
		}
	}
}
