using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Venus.LayoutModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Fulfillment
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
