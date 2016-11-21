using System.Collections.Generic;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetThemeModel : WidgetStateModel
	{
		public BoxDataModel Box { get; set; }

		public WidgetTransitionModel Transition { get; set; } 

		public Dictionary<string, WidgetStateModel> State { get; set; } = new Dictionary<string, WidgetStateModel>();
	}
}