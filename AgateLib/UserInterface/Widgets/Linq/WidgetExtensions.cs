using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets.Linq
{
	public static class WidgetExtensions
	{
		public static IEnumerable<Widget> Descendants(this Container container)
		{
			foreach (var w in container.Children)
			{
				yield return w;
			}

			foreach (var w in container.Children.OfType<Container>())
			{
				foreach (var ww in ((Container)w).Descendants())
					yield return ww;
			}
		}
	}
}
