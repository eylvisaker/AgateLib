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
	public class FacetInspector<T> : IFacetInspector<T>
	{
		public PropertyMap<T> BuildPropertyMap(IUserInterfaceFacet facet)
		{
			var result = new PropertyMap<T>();

			var baseType = typeof(T).GetTypeInfo();

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

				result.Add(name, new PropertyMapValue<T>
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
