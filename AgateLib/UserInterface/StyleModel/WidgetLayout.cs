using AgateLib.UserInterface.DataModel;

namespace AgateLib.UserInterface.StyleModel
{
	public class WidgetLayout
	{
		public WidgetLayoutType PositionType { get; set; }
		public WidgetLayoutType SizeType { get; set; }
		
		public int? MinWidth { get; set; }
		public int? MaxWidth { get; set; }
		public int? MinHeight { get; set; }
		public int? MaxHeight { get; set; }

	}
}