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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Rendering.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Rendering
{
	public class WidgetAnimator
	{
		CssStyle mStyle;

		public Point ClientWidgetOffset { get; set; }
		public Size WidgetSize
		{
			get
			{
				var widgetSize = ClientRect.Size;
				var box = mStyle.BoxModel;

				widgetSize.Width += box.Padding.Left + box.Padding.Right + box.Border.Left + box.Border.Right;
				widgetSize.Height += box.Padding.Top + box.Padding.Bottom + box.Border.Bottom + box.Border.Top;

				return widgetSize;
			}
		}
		public Rectangle ClientRect;

		public bool Active { get; private set; }
		public bool Visible { get; set; }

		CssTransitionType mTransitionType;
		public IWidgetTransition Transition { get; private set; }

		public WidgetAnimator(CssStyle style)
		{
			mStyle = style;
		}

		public void Update(double deltaTime)
		{ 
			if (Transition == null || mTransitionType != mStyle.Data.Transition.Type)
			{
				mTransitionType = mStyle.Data.Transition.Type;
				Transition = TransitionFactory.CreateTransition(mTransitionType);
				Transition.Animator = this;
				Transition.Style = mStyle;

				Transition.Initialize();
			}

			if (Transition.Active || Transition.NeedTransition())
			{
				Transition.Update(deltaTime);
			}
		}

		public Widgets.Widget ParentCoordinateSystem { get; set; }
	}
}
