//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//

using System;
using System.Linq;
using System.Reflection;

namespace AgateLib.Resources.Managers
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
					Assign = (value) =>
					{
						try
						{
							property.SetValue(obj, value);
						}
						catch (Exception e)
						{
							throw new AgateUserInterfaceInitializationException(
								$"Widget named '{name}' is a {value.GetType().Name} but the container expects a {property.PropertyType.Name} instead.", e);
						}
					}
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
					Assign = (value) =>
					{
						try { field.SetValue(obj, value); }
						catch (Exception e)
						{
							throw new AgateUserInterfaceInitializationException(
								$"Widget named '{name}' is a {value.GetType().Name} but the container expects a {field.FieldType.Name} instead.", e);
						}
					}
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
