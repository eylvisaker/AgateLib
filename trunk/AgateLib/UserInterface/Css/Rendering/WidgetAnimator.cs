using AgateLib.Geometry;
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
		public Size WidgetSize { get; set; }
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
