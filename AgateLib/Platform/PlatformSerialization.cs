using AgateLib.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AgateLib.Platform.Common.PlatformImplementation
{
	public class PlatformSerialization : IPlatformSerialization
	{
		public object CreateInstance(Type t)
		{
			var typeinfo = t.GetTypeInfo();

			foreach(var constructor in typeinfo.DeclaredConstructors)
			{
				if (constructor.IsStatic) continue;
				if (constructor.IsAbstract) continue;
				if (constructor.GetParameters().Length == 0)
				{
					return constructor.Invoke(null);
				}
			}

			throw new InvalidOperationException("Could not find constructor for " + typeinfo.Name);
		}
	}
}
