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
using AgateLib.Mathematics;
using AgateLib.Platform;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering.Transitions
{
	class SlideTransition : NullTransition
	{
		bool mFirstTransition = true;

		public TransitionDirection TransitionType { get; private set; }

		Vector2f mOutsidePosition;
		Vector2f mTargetDestination;
		float mVelocityMag;
		float mAccelerationMag;

		Vector2f mDestination;
		Vector2f mPosition;
		Vector2f mVelocity;
		Vector2f mAcceleration;

		double mTime;
		double mFinalTime = 0.2;
		bool movingOut;


		public override void Initialize()
		{
			TransitionType = Style.Transition.Direction;
			mFinalTime = Style.Transition.Time;

			mTargetDestination = new Vector2f(Widget.ClientRect.X, Widget.ClientRect.Y);
			mOutsidePosition = mTargetDestination;
			mVelocity = Vector2f.Zero;

			const int leeway = 40;

			switch (TransitionType)
			{
				case TransitionDirection.Left:
					mOutsidePosition.X = -Widget.Width - leeway;
					break;

				case TransitionDirection.Top:
					mOutsidePosition.Y = -Widget.Height - leeway;
					break;

				case TransitionDirection.Right:
					mOutsidePosition.X = Widget.Parent.Width + leeway;
					break;

				case TransitionDirection.Bottom:
					mOutsidePosition.Y = Widget.Parent.Height + leeway;
					break;
			}

			mDestination = mTargetDestination;
			SetDynamicVariables();

			SetInitialAnimatorProperties();
		}

		private void SetDynamicVariables()
		{
			mPosition = new Vector2f(Animator.ClientRect.Location);

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
			mDestination = new Vector2f(Widget.ClientRect.Location);

			SetDynamicVariables();
			RecalculateVectors();
		}

		void TransitionOut()
		{
			Active = true;
			movingOut = true;
			mTime = 0;
			mVelocity = Vector2f.Zero;

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
			mVelocity = Vector2f.Zero;

			mPosition = mOutsidePosition;
			mDestination = new Vector2f(Widget.ClientRect.X, Widget.ClientRect.Y);
			
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

		public override void Update(ClockTimeSpan elapsed)
		{
			double delta_t = elapsed.TotalSeconds;

			if (delta_t > 0.1)
				delta_t = 0.1;

			mTime += delta_t;

			float dtf = (float)delta_t;
			Vector2f step = mVelocity * dtf + 0.5f * mAcceleration * dtf * dtf;

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

		void SetPosition(Vector2f pos)
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
