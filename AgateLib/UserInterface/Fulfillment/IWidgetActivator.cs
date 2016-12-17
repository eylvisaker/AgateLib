using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Fulfillment
{
	public interface IWidgetActivator
	{
		bool CanCreate(string typename);
		Widget Create(string typename);
	}
}