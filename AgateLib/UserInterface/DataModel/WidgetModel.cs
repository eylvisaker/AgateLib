using System.Collections.Generic;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetModel
	{
		public string Type { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public WidgetDock Dock { get; set; }

		public Dictionary<string, WidgetModel> Children { get; set; }
	}
}