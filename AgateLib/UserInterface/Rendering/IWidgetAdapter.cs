using AgateLib.Resources.DataModel;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{
	public interface IWidgetAdapter
	{
		FacetModelCollection FacetData { get; set; }
		ThemeModelCollection ThemeData { get; set; }

		WidgetStyle StyleOf(Widget widget);
		void SetFont(Widget widget);
		void InitializeStyleData(Gui gui);
	}
}