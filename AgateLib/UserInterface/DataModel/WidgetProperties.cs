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

		public WidgetChildCollection Children { get; private set; } = new WidgetChildCollection();

		public WidgetThemeModel Style { get; set; }

		public Point? Position { get; set; }
		public Size? Size { get; set; }

		public WidgetDock? Dock { get; set; }

		public bool Enabled { get; set; } = true;
	}
}
