using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Venus.LayoutModel
{
	public class WidgetProperties
	{
		public string Name { get; set; }
		public string Type { get; set; }

		public string Text { get; set; }

		public WidgetChildCollection Children { get; private set; } = new WidgetChildCollection();

		public StyleProperties Style { get; set; }

		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public WidgetDock? Dock { get; set; }

		public bool Enabled { get; set; } = true;
	}
}
