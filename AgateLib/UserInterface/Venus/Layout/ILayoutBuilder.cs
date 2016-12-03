using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Venus.Layout
{
	public interface ILayoutBuilder
	{
		/// <summary>
		/// Calculates the box size of the widget, given the passed constraints.
		/// The box size includes with margin, padding and border.
		/// </summary>
		/// <param name="child"></param>
		/// <param name="maxWidth"></param>
		/// <returns></returns>
		Size ComputeBoxSize(WidgetStyle widget, int? maxWidth = null, int? maxHeight = null);
	}
}
