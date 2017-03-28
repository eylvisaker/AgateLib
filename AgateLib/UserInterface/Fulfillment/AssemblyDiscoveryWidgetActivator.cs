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
