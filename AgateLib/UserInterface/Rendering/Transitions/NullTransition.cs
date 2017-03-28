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
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering.Transitions
{
	class NullTransition : IWidgetTransition
	{
		bool mActive;

		public WidgetStyle Style { get; set; }
		public IWidgetAnimator Animator { get; set; }
		public Widget Widget { get { return Style.Widget; } }

		public virtual bool Active
		{
			get { return mActive; }
			protected set { mActive = value; }
		}


		public virtual void Initialize()
		{
			CopyLayoutDataToAnimator();
		}

		public virtual void Update(ClockTimeSpan elapsed)
		{
			CopyLayoutDataToAnimator();

			mActive = false;
		}

		protected void CopyLayoutDataToAnimator()
		{
			Animator.Visible = Widget.Visible;
			Animator.ClientRect = Widget.ClientRect;
			//Animator.WidgetSize = Widget.WidgetSize;
		}

		public virtual bool NeedTransition
		{
			get
			{
				if (Widget is Desktop)
					return false;

				if (Animator.Visible != Widget.Visible)
					return true;
				if (Animator.Visible == false)
					return false;
				if (Animator.IsDead == false && Animator.Widget.Parent == null)
					return true;

				if (Animator.ClientRect != Widget.ClientRect)
					return true;

				return false;
			}
		}

		public virtual void ActivateTransition()
		{
			CopyLayoutDataToAnimator();

			mActive = false;

			if (Animator.Widget.Parent == null)
				AnimationDead = true;
		}


		public bool AnimationDead { get; protected set; }
	}
}
