using System;
using System.Collections.Generic;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout.LayoutAssemblers
{
	internal class RowAssembler : ILayoutAssembler
	{
		public bool CanDoLayoutFor(WidgetStyle containerStyle)
		{
			if (containerStyle.ContainerLayout.Direction == LayoutDirection.Row)
				return true;


			return false;
		}

		public bool ComputeNaturalSize(ILayoutBuilder layoutBuilder, WidgetStyle widget)
		{
			throw new NotImplementedException();
		}

		public void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren, int? maxWidth = null, int? maxHeight = null)
		{
			throw new NotImplementedException();
		}
	}
}