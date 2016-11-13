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

		public List<WidgetProperties> Children { get; private set; } = new List<WidgetProperties>();

		public StyleProperties Style { get; set; }

		public Point? Location { get; set; }
		public Size? Size { get; set; }

		public bool? Enabled { get; set; }
	}
}
