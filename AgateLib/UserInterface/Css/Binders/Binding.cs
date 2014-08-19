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
using AgateLib.UserInterface.Css.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Binders
{
	public class Binding
	{
		private string name;
		private string value;
		PropertyChain chain;
		object convertedValue;

		public Binding(CssPropertyMap map, string name, string value)
		{
			// TODO: Complete member initialization
			this.name = name;
			this.value = value;

			chain = map[name];

			var property = FinalProperty;

			if (typeof(ICssPropertyFromText).IsAssignableFrom(property.PropertyType))
			{
				convertedValue = value;
			}
			else
			{
				convertedValue = CssTypeConverter.ChangeType(property.PropertyType, value);
			}
		}

		PropertyInfo FinalProperty { get { return chain[chain.Count - 1]; } }

		public void Apply(CssStyleData cssStyleData)
		{
			object obj = cssStyleData;

			for (int i = 0; i < chain.Count - 1; i++)
			{
				obj = chain[i].GetValue(obj, null);
			}

			var property = chain[chain.Count - 1];

			if (typeof(ICssPropertyFromText).IsAssignableFrom(property.PropertyType))
			{
				var rfs = (ICssPropertyFromText)property.GetValue(obj, null);
				rfs.SetValueFromText(value);
			}
			else
			{
				property.SetValue(obj, convertedValue, null);
			}
		}
	}
}
