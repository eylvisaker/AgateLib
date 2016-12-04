using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.DataModel;
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

			metricsComputer.ComputeMetrics(gui.Desktop);

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

			ILayoutAssembler assembler = FindAssembler(containerStyle);

			return assembler.ComputeNaturalSize(this, containerStyle);
		}

		private void SetDesktopStyleProperties(Desktop desktop, Size renderTargetSize)
		{
			var style = adapter.StyleOf(desktop);

			style.Metrics.ContentSize = renderTargetSize;
		}

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

			assembler.DoLayout(this, containerStyle, layoutChildren);

			container.ClientWidgetOffset = new Point(
				containerStyle.BoxModel.Border.Left + containerStyle.BoxModel.Padding.Left, 
				containerStyle.BoxModel.Border.Top + containerStyle.BoxModel.Padding.Top);

			foreach(var subContainer in nonlayoutContainers)
			{
				LayoutChildren(subContainer, totalRefresh);
			}

			foreach(var style in from item in container.Children
								let style = adapter.StyleOf(item)
								where style.WidgetLayout.SizeType == WidgetLayoutType.Flow
								select style)
			{
				style.Widget.Width = style.Metrics.BoxSize.Width - style.BoxModel.Margin.Left - style.BoxModel.Margin.Right;
				style.Widget.Height = style.Metrics.BoxSize.Height - style.BoxModel.Margin.Top - style.BoxModel.Margin.Right;
			}
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
				LayoutChildren(container, false, maxWidth, maxHeight);
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
