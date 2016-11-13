using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform;

namespace AgateLib.UserInterface.Venus.LayoutModel
{
	public class WidgetLayoutModel
	{
		public WidgetLayoutModel(string @namespace, params WidgetProperties[] widgets)
		{
			Namespace = @namespace;
			Widgets = widgets.ToList();
		}

		public WidgetLayoutModel(string @namespace, ILayoutCondition condition, params WidgetProperties[] widgets)
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
		bool ApplyLayoutModel(LayoutEnvironment environment, WidgetLayoutModel widget);
	}

	public class LayoutEnvironment
	{
		public DeviceType DeviceType { get; set; }
	}
}
