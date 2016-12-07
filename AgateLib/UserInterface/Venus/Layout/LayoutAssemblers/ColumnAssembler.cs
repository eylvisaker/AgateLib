﻿using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.Geometry;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout.LayoutAssemblers
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
			var container = widget.Widget as Container;
			Size size = new Size();

			foreach(var child in container.Children.Select(x => layoutBuilder.StyleOf(x)))
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

		public void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren, int? maxWidth = null, int? maxHeight = null)
		{
			int y = 0;
			int contentWidth = maxWidth ?? (container.Metrics.NaturalBoxSize.Width - container.BoxModel.Width);
			
			foreach (var child in layoutChildren)
			{
				layoutBuilder.ComputeBoxSize(child, maxWidth: contentWidth);

				child.Widget.X = child.BoxModel.Left;
				child.Widget.Y = y + child.BoxModel.Top;
				
				y += child.Metrics.BoxSize.Height;
			}
			
			int height = y;

			container.Metrics.BoxSize = new Size(
				contentWidth + container.BoxModel.Width, 
				height + container.BoxModel.Height);
			container.Metrics.ContentSize = new Size(
				contentWidth,
				height);
		}
	}
}