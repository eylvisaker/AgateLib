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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Binders
{
	public class CssBindingMapper
	{
		Dictionary<Type, ReflectionPropertyBinder> mPropertyBinders = new Dictionary<Type, ReflectionPropertyBinder>();
		private CssPropertyMap map;

		public CssBindingMapper(CssPropertyMap destMap)
		{
			map = destMap;
		}
		public ReflectionPropertyBinder GetPropertyBinder(Type type)
		{
			if (mPropertyBinders.ContainsKey(type) == false)
				mPropertyBinders.Add(type, CreatePropertyBinder(type));

			return mPropertyBinders[type];
		}

		private ReflectionPropertyBinder CreatePropertyBinder(Type type)
		{
			return new ReflectionPropertyBinder(type, this);
		}

		public PropertyChain GetCssPropertyChain(Type objType, string name)
		{
			var binder = GetPropertyBinder(objType);

			return binder.GetCssPropertyChain(objType, name);
		}

		public bool FindPropertyChain(string property)
		{
			if (map.ContainsKey(property))
				return true;

			var result = GetCssPropertyChain(typeof(CssStyleData), property);

			if (result == null)
				return false;

			if (result.Count > 0)
			{
				map.AddChain(property, result);
				return true;
			}

			return false;
		}
	}
}
