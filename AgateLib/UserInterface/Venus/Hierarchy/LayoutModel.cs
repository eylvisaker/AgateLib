using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Venus.Hierarchy
{
	public class LayoutModel
	{
		public LayoutModel(string @namespace, params WidgetProperties[] widgets)
		{
			Namespace = @namespace;
			Widgets = widgets.ToList();
		}

		public string Namespace { get; set; }

		public ILayoutCondition Condition { get; set; }

		public IList<WidgetProperties> Widgets { get; set; }
	}


	public interface ILayoutCondition
	{
		bool ApplyWidgetProperties(LayoutEnvironment environment, WidgetProperties widget);
	}

	public class LayoutEnvironment
	{
		public LayoutModel LayoutModel { get; set; }

	}
}
