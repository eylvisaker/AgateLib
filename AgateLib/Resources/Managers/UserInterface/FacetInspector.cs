using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Resources.Managers.UserInterface
{
	public class FacetInspector : IFacetInspector
	{
		public FacetWidgetPropertyMap BuildPropertyMap(IUserInterfaceFacet facet)
		{
			FacetWidgetPropertyMap result = new FacetWidgetPropertyMap();

			var baseType = typeof(Widget).GetTypeInfo();

			var type = facet.GetType();
			var info = type.GetTypeInfo();

			var properties = info.DeclaredProperties;

			foreach (var property in properties)
			{
				if (baseType.IsAssignableFrom(property.PropertyType.GetTypeInfo()) == false)
					continue;

				var name = GetWidgetName(property);

				if (result.ContainsKey(name))
					throw new InvalidOperationException($"Multiple items are attempting to bind to \"{name}\".");

				result.Add(name, new FacetWidgetPropertyMapValue
				{
					Assign = (value) => property.SetValue(facet, value)
				});
			}

			return result;
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
