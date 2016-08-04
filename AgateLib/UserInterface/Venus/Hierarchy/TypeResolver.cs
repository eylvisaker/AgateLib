using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Hierarchy
{
	public interface ITypeResolver
	{
		Type Resolve(string name);
	}

	public class TypeResolver : ITypeResolver
	{
		private Dictionary<string, Type> widgetTypeMap;

		public TypeResolver()
		{
			widgetTypeMap = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

			widgetTypeMap.Add("Label", typeof(Label));
			widgetTypeMap.Add("Panel", typeof(Panel));
			widgetTypeMap.Add("Window", typeof(Window));
		}

		public Type Resolve(string name)
		{
			if (widgetTypeMap.ContainsKey(name))
				return widgetTypeMap[name];

			return null;
		}
	}
}
