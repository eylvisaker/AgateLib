using System.Collections.Generic;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetThemeModel : WidgetStateModel
	{
		public int? MaxWidth { get; set; }
		public int? MaxHeight { get; set; }
		public int? MinWidth { get; set; }
		public int? MinHeight { get; set; }

		public WidgetLayoutModel Layout { get; set; } = new WidgetLayoutModel();

		public WidgetTransitionModel Transition { get; set; } = new WidgetTransitionModel();

		public Dictionary<string, WidgetStateModel> State { get; set; } = new Dictionary<string, WidgetStateModel>();

		public FontStyleProperties Font { get; set; } = new FontStyleProperties();
	}
}