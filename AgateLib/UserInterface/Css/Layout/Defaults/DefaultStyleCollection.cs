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
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout.Defaults
{
	public class DefaultStyleCollection
	{
		Dictionary<Type, IDefaultStyleSetter> mSetters = new Dictionary<Type, IDefaultStyleSetter>();
		IDefaultStyleSetter mDefaultSetter = new BlankDefaultStyle();

		public DefaultStyleCollection()
		{
			mSetters[typeof(Window)] = new WindowDefaultStyle();
			mSetters[typeof(Menu)] = new MenuDefaultStyle();
		}

		public void Add(IDefaultStyleSetter styleSetter, params Type[] types)
		{
			if (types.Length == 0)
				throw new ArgumentException("You must supply at least one type to use.");

			foreach (var t in types)
				mSetters[t] = styleSetter;
		}

		public IDefaultStyleSetter this[Type type]
		{
			get
			{
				if (mSetters.ContainsKey(type))
					return mSetters[type];
				else
					return mDefaultSetter;
			}
		}
	}
}
