//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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
