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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Fulfillment
{
	public class AssemblyDiscoveryWidgetActivator : IWidgetActivator
	{
		Dictionary<string, Type> typeMap = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

		public static AssemblyDiscoveryWidgetActivator ForAgateLib()
		{
			return new AssemblyDiscoveryWidgetActivator(typeof(Widget).GetTypeInfo().Assembly);
		}

		public AssemblyDiscoveryWidgetActivator(Assembly assembly)
		{
			foreach(var type in assembly.DefinedTypes.Where(x => x.IsAbstract == false && typeof(Widget).GetTypeInfo().IsAssignableFrom(x)))
			{
				if (typeMap.ContainsKey(type.Name))
				{
					throw new InvalidOperationException("Multiple types with the same name were found. AssemblyDiscoverWidgetActivator cannot be used. Either rename the types or use an alternate widget activator for this assembly.");
				}

				typeMap.Add(type.Name, type.AsType());
			}
		}

		public bool CanCreate(string typename)
		{
			return typeMap.ContainsKey(typename);
		}

		public Widget Create(string typename)
		{
			return (Widget)Activator.CreateInstance(typeMap[typename]);
		}
	}
}
