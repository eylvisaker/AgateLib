//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Layout.LayoutAssemblers;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Layout
{
	public class AgateLayoutEngine : IFacetLayoutEngine, ILayoutBuilder
	{
		private AgateWidgetAdapter adapter;
		private MetricsComputer metricsComputer;
		private List<ILayoutAssembler> layoutAssemblers = new List<ILayoutAssembler>();

		public AgateLayoutEngine(AgateWidgetAdapter adapter)
		{
			this.adapter = adapter;
			metricsComputer = new MetricsComputer(adapter);

			layoutAssemblers.Add(new RowAssembler());
			layoutAssemblers.Add(new ColumnAssembler());
		}

		public void UpdateLayout(FacetScene facetScene)
		{
			UpdateLayout(facetScene, Display.Coordinates.Size);
		}
		public void UpdateLayout(FacetScene facetScene, Size renderTargetSize)
		{
			bool totalRefresh = false;

			totalRefresh |= facetScene.Desktop.Width != renderTargetSize.Width;
			totalRefresh |= facetScene.Desktop.Height != renderTargetSize.Height;
			totalRefresh |= facetScene.Desktop.LayoutDirty;

			SetDesktopStyleProperties(facetScene.Desktop, renderTargetSize);

			ComputeNaturalSize(facetScene.Desktop);

			LayoutChildren(facetScene.Desktop, totalRefresh, renderTargetSize.Width, renderTargetSize.Height);
		}

		public bool ComputeNaturalSize(WidgetStyle style)
		{
			return ComputeNaturalSize(style.Widget);
		}

		public bool ComputeNaturalSize(Widget widget)
		{
			adapter.SetFont(widget);

			if (widget.LayoutChildren.Any())
			{
				var containerStyle = adapter.StyleOf(widget);

				if (containerStyle.WidgetLayout.SizeType == WidgetLayoutType.Fixed)
				{
					var result = ComputeFixedNaturalSize(widget, containerStyle);

					foreach (var child in widget.LayoutChildren)
						ComputeNaturalSize(child);

					return result;
				}
				else
				{
					ILayoutAssembler assembler = FindAssembler(containerStyle);

					return assembler.ComputeNaturalSize(this, containerStyle);
				}
			}
			else
			{
				var style = adapter.StyleOf(widget);
				adapter.SetFont(widget);

				return metricsComputer.ComputeNaturalSize(widget, style);
			}
		}
		
		private static bool ComputeFixedNaturalSize(Widget widget, WidgetStyle containerStyle)
		{
			var newNaturalBoxSize = new Size(
				widget.Width + containerStyle.BoxModel.Width,
				widget.Height + containerStyle.BoxModel.Height);

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
		private void LayoutChildren(Widget container, bool totalRefresh, int? maxWidth = null, int? maxHeight = null)
		{
			var containerStyle = adapter.StyleOf(container);

			ILayoutAssembler assembler = FindAssembler(containerStyle);

			var layoutChildren = (from item in container.LayoutChildren
								  let style = adapter.StyleOf(item)
								  where style.WidgetLayout.PositionType == WidgetLayoutType.Flow
								  select style).ToList();

			var nonlayoutChildren = (from item in container.LayoutChildren
									   let style = adapter.StyleOf(item)
									   where style.WidgetLayout.PositionType == WidgetLayoutType.Fixed
									   select item).ToList();

			assembler.DoLayout(this, containerStyle, layoutChildren, maxWidth, maxHeight);

			foreach (var style in nonlayoutChildren.Select(x => adapter.StyleOf(x)))
			{
				SetDimensions(style, maxWidth, maxHeight);

				if (style.WidgetLayout.SizeType == WidgetLayoutType.Fixed)
				{
					LayoutChildren(style.Widget, totalRefresh, style.Widget.Width, style.Widget.Height);
				}
				else
				{
					LayoutChildren(style.Widget, totalRefresh);
				}
			}

			foreach (var style in from item in container.LayoutChildren
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

		public void ComputeBoxSize(WidgetStyle style, int? maxWidth = null, int? maxHeight = null)
		{
			if (style.Widget.LayoutChildren.Any())
			{
				LayoutChildren(style.Widget, false, maxWidth - style.BoxModel.Width, maxHeight - style.BoxModel.Height);
			}
			else
			{
				metricsComputer.ComputeBoxSize(style, maxWidth, maxHeight);
			}
		}

		public WidgetStyle StyleOf(Widget widget)
		{
			return adapter.StyleOf(widget);
		}
	}
}
