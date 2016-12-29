//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.Geometry;
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