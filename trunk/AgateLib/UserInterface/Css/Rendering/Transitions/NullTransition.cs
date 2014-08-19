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
