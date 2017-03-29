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
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Layout.LayoutAssemblers
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
			Size size = new Size();

			foreach (var child in widget.Widget.LayoutChildren.Select(x => layoutBuilder.StyleOf(x)))
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