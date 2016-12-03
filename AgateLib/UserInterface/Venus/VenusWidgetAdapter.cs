using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus
{
	public class VenusWidgetAdapter : IWidgetAdapter
	{
		private static ThemeModel DefaultTheme { get; set; }

		static VenusWidgetAdapter()
		{
			DefaultTheme = new ThemeModel();

			var windowTheme = new WidgetThemeModel();
			windowTheme.TextColor = Color.Black;
			DefaultTheme["window"] = windowTheme;
		}

		private Dictionary<Widget, WidgetStyle> styles = new Dictionary<Widget, WidgetStyle>();

		public FacetModelCollection FacetData { get; set; }

		public ThemeModelCollection ThemeData { get; set; }

		public void InitializeStyleData(Gui gui)
		{
			var facetModel = FacetData[gui.FacetName];

			InitializeStyleData(gui.Desktop.Children, facetModel, null);
		}

		private void InitializeStyleData(IEnumerable<Widget> children, FacetModel facetModel, Widget parent)
		{
			foreach (var child in children)
			{
				InitializeStyleDataForWidget(child, facetModel[child.Name]);
			}

		}

		private void InitializeStyleDataForWidget(Widget widget, WidgetProperties widgetProperties)
		{
			var theme = ThemeOf(widget);
			var style = StyleOf(widget);

			style.BoxModel.Clear();

			ApplyStyleProperties(style, DefaultThemeOf(widget));
			ApplyStyleProperties(style, theme);
			ApplyStyleProperties(style, widgetProperties.Style);
			ApplyWidgetProperties(style, widgetProperties);

			var container = widget as Container;
			if (container != null)
			{
				foreach (var child in container.Children)
				{
					InitializeStyleDataForWidget(child, widgetProperties.Children[child.Name]);
				}
			}
		}

		private void ApplyWidgetProperties(WidgetStyle style, WidgetProperties widgetProperties)
		{
			if (widgetProperties.Position != null)
				style.WidgetLayout.PositionType = WidgetLayoutType.Fixed;
			if (widgetProperties.Size != null)
				style.WidgetLayout.SizeType = WidgetLayoutType.Fixed;
		}

		private void ApplyStyleProperties(WidgetStyle widget, WidgetThemeModel theme)
		{
			if (theme == null)
				return;

			widget.Font.Color = theme.TextColor ?? widget.Font.Color;

			if (theme.Box != null)
			{
				widget.BoxModel.Margin = theme.Box.Margin ?? widget.BoxModel.Margin;
				widget.BoxModel.Padding = theme.Box.Padding ?? widget.BoxModel.Padding;
			}

			if (theme.Background != null)
			{
				var background = widget.Background;

				background.Image = theme.Background.Image ?? background.Image;
				background.Color = theme.Background.Color ?? background.Color;
				background.Repeat = theme.Background.Repeat ?? background.Repeat;
				background.Clip = theme.Background.Clip ?? background.Clip;
				background.Position = theme.Background.Position ?? background.Position;
			}

			if (theme.Border != null)
			{
				var border = widget.Border;

				border.Image = theme.Border.Image ?? border.Image;
				border.ImageSlice = theme.Border.Size ?? border.ImageSlice;

				widget.BoxModel.Border = border.ImageSlice;
			}

			if (theme.Font != null)
			{
				var font = widget.Font;

				font.Family = theme.Font.Family ?? font.Family;
				font.Size = theme.Font.Size ?? font.Size;
				font.Style = theme.Font.Style ?? font.Style;
			}
		}

		private WidgetThemeModel DefaultThemeOf(Widget widget)
		{
			return WidgetThemeFromTheme(widget, DefaultTheme);
		}

		private WidgetThemeModel ThemeOf(Widget widget)
		{
			ThemeModel theme;

			if (string.IsNullOrWhiteSpace(widget.Style))
				theme = ThemeData.First().Value;
			else
				theme = ThemeData[widget.Style];

			return WidgetThemeFromTheme(widget, theme);
		}

		private WidgetThemeModel WidgetThemeFromTheme(Widget widget, ThemeModel theme)
		{
			var widgetTypename = WidgetTypeNameOf(widget);

			if (theme.ContainsKey(widgetTypename))
			{
				return theme[widgetTypename];
			}

			return null;
		}

		private string WidgetTypeNameOf(Widget widget)
		{
			return widget.GetType().Name;
		}

		public WidgetStyle StyleOf(Widget widget)
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

		IWidgetStyle IWidgetAdapter.StyleOf(Widget widget)
		{
			return StyleOf(widget);
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
