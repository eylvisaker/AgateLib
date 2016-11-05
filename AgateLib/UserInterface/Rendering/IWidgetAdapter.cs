using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{
	public interface IWidgetAdapter
	{
		IWidgetStyle GetStyle(Widget widget);
		void SetFont(Widget widget);
	}
}