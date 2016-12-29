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

		public virtual void Update(double deltaTime)
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
