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

			gui.Desktop.Width = renderTargetSize.Width;
			gui.Desktop.Height = renderTargetSize.Height;

			SetDesktopAnimatorProperties(gui.Desktop);

			metricsComputer.ComputeMetrics(gui.Desktop);

			ComputeNaturalSize(gui.Desktop);

			LayoutChildren(gui.Desktop, totalRefresh);

			//if (totalRefresh || gui.Desktop.Descendants.Any(x => x.LayoutDirty))
			//{
			//	RedoFixedLayout(gui.Desktop);

			//	gui.Desktop.LayoutDirty = false;
			//	foreach (var w in gui.Desktop.Descendants)
			//		w.LayoutDirty = false;
			//}
		}

		public bool ComputeNaturalSize(Widget widget)
		{
			var container = widget as Container;

			if (container != null)
			{
				return ComputeNaturalSize(container);
			}
			else
			{
				var style = adapter.StyleOf(widget);

				return metricsComputer.ComputeNaturalSize(widget, style);
			}
		}

		private bool ComputeNaturalSize(Container container)
		{
			var containerStyle = adapter.StyleOf(container);

			ILayoutAssembler assembler = FindAssembler(containerStyle);

			return assembler.ComputeNaturalSize(this, containerStyle);
		}

		private void SetDesktopAnimatorProperties(Desktop desktop)
		{
			var style = adapter.StyleOf(desktop);

			style.Widget.ClientRect = new Rectangle(0, 0, desktop.Width, desktop.Height);
		}

		private void LayoutChildren(Container container, bool totalRefresh)
		{
			var containerStyle = adapter.StyleOf(container);

			ILayoutAssembler assembler = FindAssembler(containerStyle);
			
			var layoutChildren = (from item in container.Children
								  let style = adapter.StyleOf(item)
								  where style.WidgetLayout.Type == WidgetLayoutType.Flow
								  select style).ToList();

			assembler.DoLayout(this, containerStyle, layoutChildren);
		}

		private ILayoutAssembler FindAssembler(WidgetStyle containerStyle)
		{
			foreach(var assembler in layoutAssemblers)
			{
				if (assembler.CanDoLayoutFor(containerStyle))
					return assembler;
			}

			Log.WriteLine($"Could not find a layout assembler for {containerStyle.Widget.Name}");

			return layoutAssemblers.First();
		}

		public Size ComputeBoxSize(WidgetStyle widget, int? maxWidth = null, int? maxHeight = null)
		{
			if (widget.Widget is Container)
			{
				widget.Metrics.BoxSize = new Size(maxWidth ?? int.MaxValue, maxHeight ?? int.MaxValue);

				LayoutChildren((Container)widget.Widget, false);

				return widget.Metrics.BoxSize;
			}

			throw new NotImplementedException();
		}
	}
}
