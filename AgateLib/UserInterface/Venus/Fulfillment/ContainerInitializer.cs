using System.Collections.Generic;
using System.Reflection;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Fulfillment
{
	public class ContainerInitializer
	{
		public void Initialize(IUserInterfaceContainer ui, WidgetBuilder builder)
		{
			var type = ui.GetType();

			var fields = type.GetRuntimeFields();
			var baseType = typeof(Widget).GetTypeInfo();

			foreach (var field in fields)
			{
				if (baseType.IsAssignableFrom(field.FieldType.GetTypeInfo()) == false)
					continue;

				var name = GetWidgetName(field);

				var widget = builder.WidgetOrDefault(name);

				if (widget != null)
				{
					field.SetValue(ui, widget);
				}
			}
		}

		private string GetWidgetName(FieldInfo field)
		{
			return field.Name;
		}
	}
}
