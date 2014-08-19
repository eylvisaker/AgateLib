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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Binders
{
	public class ReflectionPropertyBinder
	{
		Type theType;
		PropertyInfo[] properties;
		private CssBindingMapper mMap;

		public ReflectionPropertyBinder(Type type, CssBindingMapper map)
		{
			mMap = map;
			theType = type;
			properties = theType.GetProperties();
		}

		public PropertyChain GetCssPropertyChain(Type type, string name)
		{
			if (type != theType) throw new ArgumentException("Wrong type of object passed!");
			PropertyChain retval = new PropertyChain();

			var property = properties.FirstOrDefault(
				p =>
				{
					var a = p.GetCustomAttribute<CssAliasAttribute>(true);
					if (a != null)
						return a.Alias.Equals(name, StringComparison.OrdinalIgnoreCase);
					else
						return p.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
				});

			if (property != null)
			{
				retval.Add(property);
				return retval;
			}

			foreach (var prop in properties)
			{
				var attr = prop.GetCustomAttribute<CssPromotePropertiesAttribute>(true);

				if (attr != null)
				{
					if (string.IsNullOrEmpty(attr.Prefix) == false)
					{
						if (name.StartsWith(attr.Prefix + "-") == false)
							continue;

						name = name.Substring(attr.Prefix.Length + 1);
					}

					var newprops = mMap.GetCssPropertyChain(prop.PropertyType, name);

					if (newprops != null)
					{
						retval.Add(prop);
						retval.AddRange(newprops);
						return retval;
					}
				}
			}

			return null;
		}

	}
}
