using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.Geometry;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout.LayoutAssemblers
{
	internal class RowAssembler : ILayoutAssembler
	{
		public bool CanDoLayoutFor(WidgetStyle containerStyle)
		{
			if (containerStyle.ContainerLayout.Direction != LayoutDirection.Row)
				return false;

			if (containerStyle.ContainerLayout.Wrap != LayoutWrap.None)
				return false;

			return true;
		}

		public bool ComputeNaturalSize(ILayoutBuilder layoutBuilder, WidgetStyle widget)
		{
			var container = widget.Widget as Container;
			Size size = new Size();

			foreach (var child in container.Children.Select(x => layoutBuilder.StyleOf(x)))
			{
				layoutBuilder.ComputeNaturalSize(child);

				size.Width += child.Metrics.NaturalBoxSize.Width;
				size.Height = Math.Max(size.Height, child.Metrics.NaturalBoxSize.Height);
			}

			size = new Size(
				size.Width + widget.BoxModel.Width,
				size.Height + widget.BoxModel.Height);

			if (size == widget.Metrics.NaturalBoxSize)
				return false;

			widget.Metrics.NaturalBoxSize = size;

			return true;
		}

		public void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren, int? maxWidth = null, int? maxHeight = null)
		{
			int x = 0;
			int _maxWidth = maxWidth ?? (int.MaxValue - container.BoxModel.Width);
			int naturalContentWidth = container.Metrics.NaturalBoxSize.Width - container.BoxModel.Width;

			double widthFraction = _maxWidth / (double)naturalContentWidth;
			Size contentSize = new Size();

			if (widthFraction >= 1)
			{
				foreach (var child in layoutChildren)
				{
					child.Widget.X = x + child.BoxModel.Left;
					child.Widget.Y = child.BoxModel.Top;

					x += child.Metrics.BoxSize.Width;

					contentSize.Width += child.Metrics.BoxSize.Width;
					contentSize.Height = Math.Max(contentSize.Height, child.Metrics.BoxSize.Height);
				}
			}
			else if (widthFraction >= 0)
			{
				foreach (var child in layoutChildren)
				{
					int childMinWidth = child.Metrics.MinTotalSize.Width;
					int childNaturalWidth = child.Metrics.NaturalBoxSize.Width - child.BoxModel.Width;
					int childTargetWidth = childMinWidth + (int)(widthFraction * (childNaturalWidth - childMinWidth));

					layoutBuilder.ComputeBoxSize(child, maxWidth: childTargetWidth);

					child.Widget.X = x + child.BoxModel.Left;
					child.Widget.Y = child.BoxModel.Top;
					
					x += child.Metrics.BoxSize.Width;

					contentSize.Height = Math.Max(contentSize.Height, child.Metrics.BoxSize.Height);
				}

				contentSize.Width = _maxWidth;
			}
			else
			{
				throw new NotImplementedException();
			}

			container.Metrics.BoxSize = new Size(
				contentSize.Width + container.BoxModel.Width,
				contentSize.Height + container.BoxModel.Height);
			container.Metrics.ContentSize = contentSize;
				
		}
	}
}