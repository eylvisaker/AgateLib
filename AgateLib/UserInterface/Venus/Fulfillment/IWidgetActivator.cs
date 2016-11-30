using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Fulfillment
{
	public interface IWidgetActivator
	{
		bool CanCreate(string typename);
		Widget Create(string typename);
	}
}