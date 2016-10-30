using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Css.Rendering
{
	public interface IWidgetAdapter
	{
		IWidgetStyle GetStyle(Widget widget);
		void SetFont(Widget widget);
	}
}