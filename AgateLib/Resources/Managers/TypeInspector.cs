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
	/// <summary>
	/// Inspects an object's type and builds a map of property setters for that object 
	/// for properties that derive from T.
	/// </summary>
	public class TypeInspector<T> : ITypeInspector<T>
	{
		public PropertyMap<T> BuildPropertyMap(object obj)
		{
			var result = new PropertyMap<T>();

			var baseType = typeof(T).GetTypeInfo();

			var type = obj.GetType();
			var info = type.GetTypeInfo();

			var properties = info.DeclaredProperties;

			foreach (var property in properties.Where(property => baseType.IsAssignableFrom(property.PropertyType.GetTypeInfo())))
			{
				var name = GetWidgetName(property);

				if (result.ContainsKey(name))
					throw new InvalidOperationException($"Multiple items are attempting to bind to '{name}'.");

				result.Add(name, new PropertyMapValue<T>
				{
					Assign = (value) => property.SetValue(obj, value)
				});
			}

			var fields = info.DeclaredFields
				.Where(x => x.IsPublic)
				.Where(field => baseType.IsAssignableFrom(field.FieldType.GetTypeInfo()));

			foreach (var field in fields)
			{
				var name = GetWidgetName(field);

				if (result.ContainsKey(name))
					throw new InvalidOperationException($"Multiple items are attempting to bind to '{name}'.");

				result.Add(name, new PropertyMapValue<T>
				{
					Assign = (value) => field.SetValue(obj, value)
				});
			}

			return result;
		}

		private string GetWidgetName(MemberInfo field)
		{
			var bindTo = field.GetCustomAttribute<BindToAttribute>();

			if (bindTo != null && string.IsNullOrWhiteSpace(bindTo.Name))
				throw new InvalidOperationException("BindTo name is empty.");

			return bindTo?.Name ?? field.Name;
		}
	}
}
