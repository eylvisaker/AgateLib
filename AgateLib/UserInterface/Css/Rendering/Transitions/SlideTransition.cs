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
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Rendering.Transitions
{
	class SlideTransition : NullTransition
	{
		bool mFirstTransition = true;

		public CssTransitionDirection TransitionType { get; private set; }

		Vector2 mOutsidePosition;
		Vector2 mTargetDestination;
		float mVelocityMag;
		float mAccelerationMag;

		Vector2 mDestination;
		Vector2 mPosition;
		Vector2 mVelocity;
		Vector2 mAcceleration;
		Point mClientWidgetDiff;

		double mTime;
		double mFinalTime = 0.2;
		bool movingOut;


		public override void Initialize()
		{
			TransitionType = Style.Data.Transition.Direction;
			mFinalTime = Style.Data.Transition.Time;

			mTargetDestination = new Vector2(Widget.ClientRect.X, Widget.ClientRect.Y);
			mOutsidePosition = mTargetDestination;
			mVelocity = Vector2.Empty;

			const int leeway = 40;

			switch (TransitionType)
			{
				case CssTransitionDirection.Left:
					mOutsidePosition.X = -Widget.Width - leeway;
					break;

				case CssTransitionDirection.Top:
					mOutsidePosition.Y = -Widget.Height - leeway;
					break;

				case CssTransitionDirection.Right:
					mOutsidePosition.X = Widget.Parent.Width + leeway;
					break;

				case CssTransitionDirection.Bottom:
					mOutsidePosition.Y = Widget.Parent.Height + leeway;
					break;
			}

			mDestination = mTargetDestination;
			SetDynamicVariables();

			SetInitialAnimatorProperties();
		}

		private void SetDynamicVariables()
		{
			mPosition = new Vector2(Animator.ClientRect.Location);

			// we are using a parabola to slow down as the window reaches its destination.
			// r controls the initial speed and amount of slowdown required. at the time
			// finalTime/r the window has traveled half-way. r must be larger than one.
			// the value chosen 2+sqrt(2) gives a zero velocity as the windows come to rest.
			const double r = 3.414213562;

			double distance = (mDestination - mPosition).Magnitude;

			mVelocityMag = -(float)(distance / (mFinalTime * (r - 1)) * (1 - r * r * 0.5));
			mAccelerationMag = (float)(distance / (mFinalTime * mFinalTime * (r - 1)) * (r * r - 2 * r));
		}

		public override void ActivateTransition()
		{
			if (mFirstTransition)
			{
				TransitionIn();
				mFirstTransition = false;
			}
			else if (Widget.Visible != Animator.Visible)
			{
				if (Widget.Visible)
					TransitionIn();
				else
					TransitionOut();
			}
			else if (Widget.Parent == null)
			{
				TransitionOut();
			}
			else
			{
				VisibleTransition();
			}
			Active = true;
		}

		public override bool NeedTransition
		{
			get
			{
				if (Widget.Visible == false && Animator.Visible == false)
					return false;
				if (Widget.Parent == null)
				{
					if (Animator.Visible)
						return true;
					else
						return false;
				}

				return base.NeedTransition;
			}
		}

		void VisibleTransition()
		{
			Active = true;
			movingOut = false;
			mTime = 0;
			mDestination = new Vector2(Widget.ClientRect.Location);

			SetDynamicVariables();
			RecalculateVectors();
		}

		void TransitionOut()
		{
			Active = true;
			movingOut = true;
			mTime = 0;
			mVelocity = Vector2.Empty;

			mDestination = mOutsidePosition;

			SetDynamicVariables();
			RecalculateVectors();

			//mVelocity += (float)mFinalTime * mAcceleration;
			//mVelocity *= -1;
			//mAcceleration *= -1;
		}
		void TransitionIn()
		{
			Active = true;
			movingOut = false;
			Animator.Visible = true;

			mTime = 0;
			mVelocity = Vector2.Empty;

			mPosition = mOutsidePosition;
			mDestination = new Vector2(Widget.ClientRect.X, Widget.ClientRect.Y);
			
			SetDynamicVariables();
			RecalculateVectors();
		}

		private void RecalculateVectors()
		{
			mVelocity = mDestination - mPosition;
			mVelocity /= mVelocity.Magnitude;

			mAcceleration = -mVelocity;

			mVelocity *= mVelocityMag;
			mAcceleration *= mAccelerationMag;
		}

		public override void Update(double delta_t)
		{
			if (delta_t > 0.1)
				delta_t = 0.1;

			mTime += delta_t;

			float dtf = (float)delta_t;
			Vector2 step = mVelocity * dtf + 0.5f * mAcceleration * dtf * dtf;

			mPosition += step;
			mVelocity += mAcceleration * dtf;

			if (mTime >= mFinalTime)
			{
				Active = false;

				SetPosition(mDestination);

				if (movingOut)
					Animator.Visible = false;
			}
			else
			{
				SetPosition(mPosition);
			}
		}

		void SetPosition(Vector2 pos)
		{
			Animator.X = (int)(pos.X + 0.5);
			Animator.Y = (int)(pos.Y + 0.5);
			Animator.Width = Widget.Width;
			Animator.Height = Widget.Height;
		}
		private void SetInitialAnimatorProperties()
		{
			Animator.Visible = Widget.Visible;

			Animator.ClientRect = Widget.ClientRect;

			SetPosition(mOutsidePosition);
		}
	}
}
