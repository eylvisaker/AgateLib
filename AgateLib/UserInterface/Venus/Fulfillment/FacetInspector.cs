using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Fulfillment
{
	public class FacetInspector : IFacetInspector
	{
		public FacetWidgetPropertyMap BuildPropertyMap(IUserInterfaceFacet facet)
		{
			FacetWidgetPropertyMap result = new FacetWidgetPropertyMap();

			var baseType = typeof(Widget).GetTypeInfo();

			var type = facet.GetType();
			var info = type.GetTypeInfo();

			var fields = info.DeclaredFields;

			foreach (var field in fields)
			{
				if (baseType.IsAssignableFrom(field.FieldType.GetTypeInfo()) == false)
					continue;

				var name = GetWidgetName(field);

				result.Add(name, new FacetWidgetPropertyMapValue
				{
					Assign = (value) => field.SetValue(facet, value)
				});

				//var widget = builder.WidgetOrDefault(name);

				//if (widget != null)
				//{
				//	field.SetValue(ui, widget);
				//}
			}

			var properties = info.DeclaredProperties;

			foreach (var property in properties)
			{
				if (baseType.IsAssignableFrom(property.PropertyType.GetTypeInfo()) == false)
					continue;

				var name = GetWidgetName(property);

				result.Add(name, new FacetWidgetPropertyMapValue
				{
					Assign = (value) => property.SetValue(facet, value)
				});
			}
			return result;
		}

		private string GetWidgetName(FieldInfo field)
		{
			var bindTo = field.GetCustomAttribute<BindToAttribute>();

			if (bindTo != null && string.IsNullOrWhiteSpace(bindTo.Name))
				throw new InvalidOperationException("BindTo name is empty.");

			return bindTo?.Name ?? field.Name;
		}

		private string GetWidgetName(PropertyInfo field)
		{
			var bindTo = field.GetCustomAttribute<BindToAttribute>();

			if (bindTo != null && string.IsNullOrWhiteSpace(bindTo.Name))
				throw new InvalidOperationException("BindTo name is empty.");

			return bindTo?.Name ?? field.Name;
		}
	}
}
