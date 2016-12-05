using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetProperties
	{
		public string Name { get; set; }
		public string Type { get; set; }

		public string Text { get; set; }

		public WidgetLayoutModel Layout { get; set; } = new WidgetLayoutModel();

		public WidgetChildCollection Children { get; private set; } = new WidgetChildCollection();

		public WidgetThemeModel Style { get; set; }

		/// <summary>
		/// Indicates the upper-left point of the widget's client area.
		/// If specified, the widget will not participate in the flow layout of controls. Instead it
		/// will be fixed at this position within its parent. 
		/// </summary><remarks>
		/// Specifying this value will prevent
		/// the UI system from touching the position of the widget, so supplying a value
		/// is a good for controls that will be programmatically moved.
		/// </remarks>
		public Point? Position { get; set; }

		/// <summary>
		/// Indicates the size of the client area for this widget. 
		/// If specified, the layout engine will not alter the size
		/// of the widget.
		/// </summary>
		public Size? Size { get; set; }


		public bool Enabled { get; set; } = true;

		public bool Visible { get; set; } = true;
	}
}
