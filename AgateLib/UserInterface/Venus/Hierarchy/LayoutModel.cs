using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform;

namespace AgateLib.UserInterface.Venus.Hierarchy
{
	public class LayoutModel
	{
		public LayoutModel(string @namespace, params WidgetProperties[] widgets)
		{
			Namespace = @namespace;
			Widgets = widgets.ToList();
		}

		public LayoutModel(string @namespace, ILayoutCondition condition, params WidgetProperties[] widgets)
		{
			Namespace = @namespace;
			Condition = condition;
			Widgets = widgets.ToList();
		}

		public string Namespace { get; set; }

		public ILayoutCondition Condition { get; set; }

		public IList<WidgetProperties> Widgets { get; set; }
	}


	public interface ILayoutCondition
	{
		bool ApplyLayoutModel(LayoutEnvironment environment, LayoutModel widget);
	}

	public class LayoutEnvironment
	{
		public DeviceType DeviceType { get; set; }
	}
}
