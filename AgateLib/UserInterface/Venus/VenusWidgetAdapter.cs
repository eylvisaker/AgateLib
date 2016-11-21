using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface.DataModel;
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

		public FacetModelCollection FacetData { get; set; }

		public ThemeModelCollection ThemeData { get; set; }

		public LayoutEnvironment Environment
		{
			get { return environment; }
		}

		public void AddLayoutModel(WidgetLayoutModel widgetLayoutModel)
		{
			WidgetLayoutModels.Add(widgetLayoutModel);
		}

		public void InitializeStyleData(Gui gui)
		{
			var facetModel = FacetData[gui.FacetName];

			InitializeStyleData(gui.Desktop.Children, facetModel, null);
		}

		private void InitializeStyleData(IEnumerable<Widget> children, FacetModel facetModel, Widget parent)
		{
			foreach(var child in children)
			{
				InitializeStyleDataForWidget(child, facetModel[child.Name]);
			}
		}

		private void InitializeStyleDataForWidget(Widget widget, WidgetProperties widgetProperties)
		{
			var theme = ThemeOf(widget);
			var style = StyleOf(widget);

			ApplyStyleProperties(style, theme);
			ApplyStyleProperties(style, widgetProperties.Style);
		}
		
		private void ApplyStyleProperties(IWidgetStyle widget, WidgetThemeModel theme)
		{
			if (theme == null)
				return;

			var background = (BackgroundStyle)widget.Background;

			background.Image = theme.Background.Image;
		}

		private WidgetThemeModel ThemeOf(Widget widget)
		{
			ThemeModel theme = null;

			if (string.IsNullOrWhiteSpace(widget.Style))
				theme = ThemeData.First().Value;
			else
				theme = ThemeData[widget.Style];

			var widgetTypename = WidgetTypeNameOf(widget);

			if (theme.ContainsKey(widgetTypename))
				return theme[widgetTypename];

			return null;
		}

		private string WidgetTypeNameOf(Widget widget)
		{
			return widget.GetType().Name;
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

		public IWidgetStyle StyleOf(Widget widget)
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


		public void SetFont(Widget control)
		{
			var style = StyleOf(control);

			control.Font = DefaultAssets.Fonts.AgateSans;

			//control.Font = style.Font;
			//control.Font.Style = style.Data.Font.Weight;
		}
	}
}
