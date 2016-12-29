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
using System.Text;
using System.Reflection;

namespace AgateLib.Serialization.Xle
{
	public class TypeBinder : ITypeBinder
	{
		public List<Assembly> SearchAssemblies = new List<Assembly>();

		public Type GetType(string typename)
		{
			if (typename == "string")
				return typeof(string);

			if (Type.GetType(typename) != null)
				return Type.GetType(typename);

			for (int i = 0; i < SearchAssemblies.Count; i++)
			{
				if (SearchAssemblies[i].GetType(typename) != null)
				{
					return SearchAssemblies[i].GetType(typename);
				}
			}

			return null;
		}

		public void AddAssemblies(IEnumerable<Assembly> assemblies)
		{
			foreach (var assembly in assemblies)
				AddAssembly(assembly);
		}

		public void AddAssembly(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly cannot be null.");
			if (SearchAssemblies.Contains(assembly))
				return;

			SearchAssemblies.Add(assembly);
		}
	}
}
