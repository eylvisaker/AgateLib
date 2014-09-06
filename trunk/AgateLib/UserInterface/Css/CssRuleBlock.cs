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
using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssRuleBlock : ICssCanSelect
	{
		CssPropertyMap map;

		public CssRuleBlock(CssPropertyMap map)
		{
			this.map = map;

			Properties = new Dictionary<string, string>();
			Bindings = new Dictionary<string, Binding>();
		}

		public Dictionary<string, Binding> Bindings { get; private set; }
		public Dictionary<string, string> Properties { get; private set; }

		public CssSelector Selector { get; set; }

		public override string ToString()
		{
			return Selector.ToString() + " { "
				+ string.Join(" ",
				Properties.Select(x => x.Key + ":" + x.Value + ";")) + "}";
		}

		public void AddProperty(string name, string value)
		{
			Properties.Add(name, value);

			Bindings.Add(name, new Binding(map, name, value));
		}


		public void ApplyProperties(CssStyleData cssStyleData)
		{
			foreach(var binding in Bindings.Values)
			{
				binding.Apply(cssStyleData);
			}
		}
	}
}