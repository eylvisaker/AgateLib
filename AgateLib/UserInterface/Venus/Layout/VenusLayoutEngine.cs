using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Venus.Layout.LayoutAssemblers;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout
{
	public class VenusLayoutEngine : IGuiLayoutEngine, ILayoutBuilder
	{
		private VenusWidgetAdapter adapter;
		private VenusMetricsComputer metricsComputer;
		private List<ILayoutAssembler> layoutAssemblers = new List<ILayoutAssembler>();

		public VenusLayoutEngine(VenusWidgetAdapter adapter)
		{
			this.adapter = adapter;
			metricsComputer = new VenusMetricsComputer(adapter);

			layoutAssemblers.Add(new RowAssembler());
			layoutAssemblers.Add(new ColumnAssembler());
		}

		public void UpdateLayout(Gui gui)
		{
			UpdateLayout(gui, Display.Coordinates.Size);
		}
		public void UpdateLayout(Gui gui, Size renderTargetSize)
		{
			bool totalRefresh = false;

			totalRefresh |= gui.Desktop.Width != renderTargetSize.Width;
			totalRefresh |= gui.Desktop.Height != renderTargetSize.Height;
			totalRefresh |= gui.Desktop.LayoutDirty;

			SetDesktopStyleProperties(gui.Desktop, renderTargetSize);

			ComputeNaturalSize(gui.Desktop);

			LayoutChildren(gui.Desktop, totalRefresh, renderTargetSize.Width, renderTargetSize.Height);
		}

		public bool ComputeNaturalSize(WidgetStyle style)
		{
			return ComputeNaturalSize(style.Widget);
		}

		public bool ComputeNaturalSize(Widget widget)
		{
			var container = widget as Container;

			adapter.SetFont(widget);

			if (container != null)
			{
				return ComputeNaturalSize(container);
			}
			else
			{
				var style = adapter.StyleOf(widget);
				adapter.SetFont(widget);

				return metricsComputer.ComputeNaturalSize(widget, style);
			}
		}

		private bool ComputeNaturalSize(Container container)
		{
			var containerStyle = adapter.StyleOf(container);

			if (containerStyle.WidgetLayout.SizeType == WidgetLayoutType.Fixed)
			{
				var result = ComputeFixedNaturalSize(container, containerStyle);

				foreach (var child in container.Children)
					ComputeNaturalSize(child);

				return result;
			}
			else
			{
				ILayoutAssembler assembler = FindAssembler(containerStyle);

				return assembler.ComputeNaturalSize(this, containerStyle);
			}
		}

		private static bool ComputeFixedNaturalSize(Container container, WidgetStyle containerStyle)
		{
			var newNaturalBoxSize = new Size(
				container.Width + containerStyle.BoxModel.Width,
				container.Height + containerStyle.BoxModel.Height);

			if (containerStyle.Metrics.NaturalBoxSize == newNaturalBoxSize)
				return false;

			containerStyle.Metrics.NaturalBoxSize = newNaturalBoxSize;

			return true;
		}

		private void SetDesktopStyleProperties(Desktop desktop, Size renderTargetSize)
		{
			var style = adapter.StyleOf(desktop);

			style.Metrics.ContentSize = renderTargetSize;
		}

		/// <summary>
		/// Performs layout of the child widgets within a container's client area. Max width/height supplied are the constraints on the container's
		/// client area.
		/// </summary>
		/// <param name="container"></param>
		/// <param name="totalRefresh"></param>
		/// <param name="maxWidth">The maximum width of any child widget's box</param>
		/// <param name="maxHeight">The maximum height of any child widget's box</param>
		private void LayoutChildren(Container container, bool totalRefresh, int? maxWidth = null, int? maxHeight = null)
		{
			var containerStyle = adapter.StyleOf(container);

			ILayoutAssembler assembler = FindAssembler(containerStyle);

			var layoutChildren = (from item in container.Children
								  let style = adapter.StyleOf(item)
								  where style.WidgetLayout.PositionType == WidgetLayoutType.Flow
								  select style).ToList();

			var nonlayoutContainers = (from item in container.Children.OfType<Container>()
									   let style = adapter.StyleOf(item)
									   where style.WidgetLayout.PositionType == WidgetLayoutType.Fixed
									   select item).ToList();

			assembler.DoLayout(this, containerStyle, layoutChildren, maxWidth, maxHeight);

			foreach (var style in nonlayoutContainers.Select(x => adapter.StyleOf(x)))
			{
				SetDimensions(style, maxWidth, maxHeight);

				if (style.WidgetLayout.SizeType == WidgetLayoutType.Fixed)
				{
					LayoutChildren(style.Widget as Container, totalRefresh, style.Widget.Width, style.Widget.Height);
				}
				else
				{
					LayoutChildren(style.Widget as Container, totalRefresh);
				}
			}

			foreach (var style in from item in container.Children
								  let style = adapter.StyleOf(item)
								  where style.WidgetLayout.SizeType == WidgetLayoutType.Flow
								  select style)
			{
				SetDimensions(style, maxWidth, maxHeight);
			}
		}

		private void SetDimensions(WidgetStyle style, int? maxWidth, int? maxHeight)
		{
			if (style.WidgetLayout.PositionType == WidgetLayoutType.Flow)
			{
				var widgetBoxWidth = Math.Min(style.Metrics.BoxSize.Width, maxWidth ?? int.MaxValue);
				var widgetBoxHeight = Math.Min(style.Metrics.BoxSize.Height, maxHeight ?? int.MaxValue);

				var widgetWidth = widgetBoxWidth - style.BoxModel.Width;
				var widgetHeight = widgetBoxHeight - style.BoxModel.Height;

				style.Widget.Width = widgetWidth;
				style.Widget.Height = widgetHeight;

				style.Widget.WidgetSize = new Size(
					widgetBoxWidth - style.BoxModel.Margin.Left - style.BoxModel.Margin.Right,
					widgetBoxHeight - style.BoxModel.Margin.Top - style.BoxModel.Margin.Bottom);
			}
			else
			{
				style.Widget.WidgetSize = new Size(
					style.Widget.Width + style.BoxModel.WidgetWidth,
					style.Widget.Height + style.BoxModel.WidgetHeight);
			}

			style.Widget.ClientWidgetOffset = new Point(
				style.BoxModel.Border.Left + style.BoxModel.Padding.Left,
				style.BoxModel.Border.Top + style.BoxModel.Padding.Top);
		}

		private ILayoutAssembler FindAssembler(WidgetStyle containerStyle)
		{
			foreach (var assembler in layoutAssemblers)
			{
				if (assembler.CanDoLayoutFor(containerStyle))
					return assembler;
			}

			Log.WriteLine($"Could not find a layout assembler for {containerStyle.Widget.Name}");

			return layoutAssemblers.First();
		}

		public void ComputeBoxSize(WidgetStyle widget, int? maxWidth = null, int? maxHeight = null)
		{
			var container = widget.Widget as Container;

			if (container != null)
			{
				LayoutChildren(container, false, maxWidth - widget.BoxModel.Width, maxHeight - widget.BoxModel.Height);
			}
			else
			{
				metricsComputer.ComputeBoxSize(widget, maxWidth, maxHeight);
			}
		}

		public WidgetStyle StyleOf(Widget widget)
		{
			return adapter.StyleOf(widget);
		}
	}
}
