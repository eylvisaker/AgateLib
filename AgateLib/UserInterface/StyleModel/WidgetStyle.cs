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
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.Quality;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.StyleModel
{
	public class WidgetStyle
	{
		public WidgetStyle(Widget widget)
		{
			Widget = widget;
			NeedRefresh = true;
		}

		public Widget Widget { get; private set; }

		public WidgetProperties WidgetProperties { get; internal set; }

		public WidgetMetrics Metrics { get; private set; } = new WidgetMetrics();

		public ContainerLayout ContainerLayout { get; private set; } = new ContainerLayout();

		public WidgetLayout WidgetLayout { get; private set; } = new WidgetLayout();

		public BackgroundStyle Background { get; set; } = new BackgroundStyle();

		public BorderStyle Border { get; set; } = new BorderStyle();

		public TransitionStyle Transition { get; set; } = new TransitionStyle();

		public WidgetFontStyle Font { get; set; } = new WidgetFontStyle();

		public BoxModel BoxModel { get; set; } = new BoxModel();

		public WidgetViewStyle View { get; set; } = new WidgetViewStyle();

		public Overflow Overflow { get; set; }
		
		public TextAlign TextAlign { get; set; }

		internal bool NeedRefresh { get; set; }

		public void Clear()
		{
			Metrics = new WidgetMetrics();
			ContainerLayout = new ContainerLayout();
			WidgetLayout = new WidgetLayout();
			Background = new BackgroundStyle();
			Border = new BorderStyle();
			Transition = new TransitionStyle();
			BoxModel.Clear();
		}

		public override string ToString()
		{
			return $"WidgetStyle: {Widget.Name} ({Widget.GetType().Name})";
		}

		public void ScrollToWidget(Widget widget)
		{
			//Condition.Requires(Widget.IsDescendant(widget));

			Point location = Widget.ClientLocationOf(widget);
			var newOffset = View.ScrollOffset;

			if ((View.AllowScroll & ScrollAxes.Vertical) != 0)
			{
				int bottom = widget.WidgetRect.Bottom;

				if (bottom - newOffset.Y > Widget.ClientRect.Height)
				{
					newOffset.Y = bottom - Widget.ClientRect.Height;
				}
				if (widget.WidgetRect.Top < newOffset.Y)
				{
					newOffset.Y = widget.WidgetRect.Top;
				}
			}
			if ((View.AllowScroll & ScrollAxes.Horizontal) != 0)
			{
				int right = widget.WidgetRect.Right;

				if (right - newOffset.X > Widget.ClientRect.Width)
				{
					int diff = right - Widget.ClientRect.Width;
					newOffset.X = diff;
				}

				if (widget.WidgetRect.Left < newOffset.X)
				{
					newOffset.X = widget.WidgetRect.Left;
				}
			}

			View.ScrollOffset = newOffset;
		}
	}
}
