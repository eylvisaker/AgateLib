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
	internal class ColumnAssembler : ILayoutAssembler
	{
		public bool CanDoLayoutFor(WidgetStyle containerStyle)
		{
			if (containerStyle.ContainerLayout.Direction != LayoutDirection.Column)
				return false;

			if (containerStyle.ContainerLayout.Wrap != LayoutWrap.None)
				return false;

			return true;
		}

		public bool ComputeNaturalSize(ILayoutBuilder layoutBuilder, WidgetStyle widget)
		{
			Size size = new Size();

			foreach(var child in widget.Widget.LayoutChildren.Select(x => layoutBuilder.StyleOf(x)))
			{
				layoutBuilder.ComputeNaturalSize(child);

				size.Width = Math.Max(size.Width, child.Metrics.NaturalBoxSize.Width);
				size.Height += child.Metrics.NaturalBoxSize.Height;
			}

			size = new Size(
				size.Width + widget.BoxModel.Width,
				size.Height + widget.BoxModel.Height);

			if (size == widget.Metrics.NaturalBoxSize)
				return false;

			widget.Metrics.NaturalBoxSize = size;
			
			return true;
		}

		public void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren, 
			int? maxWidth = null, int? maxHeight = null)
		{
			int y = 0;
			int contentWidth = maxWidth ?? (container.Metrics.NaturalBoxSize.Width - container.BoxModel.Width);
			
			foreach (var child in layoutChildren)
			{
				int? childMaxHeight = maxHeight;

				if (childMaxHeight != null && child.Overflow == Overflow.Scroll)
				{
					childMaxHeight = childMaxHeight - y;
				}

				layoutBuilder.ComputeBoxSize(child, contentWidth, childMaxHeight);

				child.Widget.X = child.BoxModel.Left;
				child.Widget.Y = y + child.BoxModel.Top;
				
				y += child.Metrics.BoxSize.Height;
			}
			
			int height = y;

			if (container.Overflow == Overflow.Scroll && height > maxHeight)
				height = maxHeight.Value;

			container.Metrics.BoxSize = new Size(
				contentWidth + container.BoxModel.Width, 
				height + container.BoxModel.Height);
			container.Metrics.ContentSize = new Size(
				contentWidth,
				height);
		}
	}
}