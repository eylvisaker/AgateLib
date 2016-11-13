using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Venus.LayoutModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus
{
	public class VenusWidgetAdapter : IWidgetAdapter
	{
		private LayoutEnvironment environment = new LayoutEnvironment();
		private Dictionary<Widget, WidgetStyle> styles = new Dictionary<Widget, WidgetStyle>();

		public VenusWidgetAdapter(IEnumerable<WidgetLayoutModel> models =null)
		{
			models = models ?? new List<WidgetLayoutModel>();
			WidgetLayoutModels = models.ToList();
		}

		public LayoutEnvironment Environment
		{
			get { return environment; }
		}

		public void AddLayoutModel(WidgetLayoutModel widgetLayoutModel)
		{
			WidgetLayoutModels.Add(widgetLayoutModel);
		}

		internal List<WidgetLayoutModel> WidgetLayoutModels { get; set; }

		internal IEnumerable<WidgetLayoutModel> SelectModels(string @namespace)
		{
			var result = from model in WidgetLayoutModels
						 where model.Namespace == @namespace &&
							   (model.Condition == null || model.Condition.ApplyLayoutModel(Environment, model))
						 select model;

			return result;
		}

		public IWidgetStyle GetStyle(Widget widget)
		{
			if (styles.ContainsKey(widget) == false)
				styles.Add(widget, new WidgetStyle(widget));

			var result = styles[widget];

			if (result.NeedRefresh)
			{
				BuildStyle(result);
			}

			return result;
		}

		private void BuildStyle(WidgetStyle result)
		{
			var widget = result.Widget;

			
		}

		public void SetFont(Widget widget)
		{
			throw new NotImplementedException();
		}
	}
}
