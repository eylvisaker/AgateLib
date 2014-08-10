using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Rendering.Transitions
{
	class NullTransition : IWidgetTransition
	{
		bool mActive;

		public CssStyle Style {get;set;}
		public WidgetAnimator Animator { get;set;}
		protected Widget Widget { get { return Style.Widget; } }

		public virtual bool Active
		{
			get { return mActive; }
			protected set { mActive = value; }
		}


		public virtual void Initialize()
		{
			CopyLayoutDataToAnimator();
		}
		
		public virtual void Update(double deltaTime)
		{
			CopyLayoutDataToAnimator();

			mActive = false;
		}

		protected void CopyLayoutDataToAnimator()
		{
			Animator.Visible = Widget.Visible;
			Widget.ClientRect = Animator.ClientRect;

			Widget.ClientWidgetOffset = Animator.ClientWidgetOffset;
			Widget.WidgetSize = Animator.WidgetSize;
		}

		public virtual bool NeedTransition()
		{
			bool retval = false;

			if (Animator.Visible != Widget.Visible)
				retval = true;
			if (Animator.Visible == false && retval == false)
				return false;

			if (Animator.ClientRect != Widget.ClientRect)
				retval = true;

			if (retval != mActive)
			{
				mActive = retval;
				StartTransition();
			}

			return mActive;
		}

		protected virtual void StartTransition()
		{
		}


	}
}
