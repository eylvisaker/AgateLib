using System;
using System.Collections.Generic;
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
			throw new NotImplementedException();
		}

		public void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren)
		{
			int y = 0;

			foreach (var child in layoutChildren)
			{
				child.Metrics.BoxSize = layoutBuilder.ComputeBoxSize(child, maxWidth: container.Metrics.ContentSize.Width);

				child.Widget.X = 0;
				child.Widget.Y = y;

				y += child.Metrics.BoxSize.Height;
			}
		}
	}
}