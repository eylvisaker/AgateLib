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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Widgets;

namespace Zodiac.UserInterface.GuiSystem
{
	public class WindowTransition
	{
		Window mOwner;

		public WindowTransition(Window wind, TransitionType transitionType)
		{
			OffScreen = true;
			mOwner = wind;
			this.TransitionType = transitionType;
		}

		bool mInitialized;
		public bool InTransition { get; private set; }
		public bool HasStarted { get; private set; }
		public TransitionType TransitionType { get; private set; }

		Vector2 mStartPosition;
		Vector2 mTargetDestination;
		float mVelocityMag;
		float mAccelerationMag;

		Vector2 mDestination;
		Vector2 mPosition;
		Vector2 mVelocity;
		Vector2 mAcceleration;

		double mTime;
		const double mFinalTime = 0.2;

		internal void Update(double delta_t, ref bool processInput)
		{
			if (mInitialized == false)
				Initialize();

			if (HasStarted == false)
			{
				TransitionIn();
				HasStarted = true;
			}
			else if (InTransition == false)
			{
				return;
			}
			else
			{
				if (delta_t > 0.1)
					delta_t = 0.1;

				mTime += delta_t;

				processInput = false;

				float dtf = (float)delta_t;
				Vector2 step = mVelocity * dtf + 0.5f * mAcceleration * dtf * dtf;

				mPosition += step;
				mVelocity += mAcceleration * dtf;

				if (mTime >= mFinalTime)
				{
					CancelTransition();
				}
			}

			mOwner.X = (int)(mPosition.X + 0.5);
			mOwner.Y = (int)(mPosition.Y + 0.5);
		}
		/// <summary>
		/// The initialize routine assume that the position of the owning window is 
		/// its destination.
		/// </summary>
		private void Initialize()
		{
			mInitialized = true;

			mTargetDestination = new Vector2(mOwner.X, mOwner.Y);
			mStartPosition = mTargetDestination;
			mVelocity = Vector2.Empty;

			switch (TransitionType)
			{
				case TransitionType.FromLeft:
					mStartPosition.X = -mOwner.Width - 20;
					break;

				case TransitionType.FromTop:
					mStartPosition.Y = -mOwner.Height - 20;
					break;

				case TransitionType.FromRight:
					mStartPosition.X = Display.CurrentWindow.Width + 20;
					break;

				case TransitionType.FromBottom:
					mStartPosition.Y = Display.CurrentWindow.Height + 20;
					break;
			}

			// we are using a parabola to slow down as the window reaches its destination.
			// r controls the initial speed and amount of slowdown required. at the time
			// finalTime/r the window has traveled half-way. r must be larger than one.
			// the value chosen 2+sqrt(2) gives a zero velocity as the windows come to rest.
			const double r = 3.414213562;

			double distance = (mStartPosition - mTargetDestination).Magnitude;

			mVelocityMag = -(float)(distance / (mFinalTime * (r - 1)) * (1 - r * r * 0.5));
			mAccelerationMag = (float)(distance / (mFinalTime * mFinalTime * (r - 1)) * (r * r - 2 * r));

		}

		bool MovingOut { get; set; }

		public void TransitionOut()
		{
			if (MovingOut)
				return;

			OffScreen = true;
			MovingOut = true;

			InTransition = true;
			mTime = 0;
			mVelocity = Vector2.Empty;

			mPosition = mTargetDestination;
			mDestination = mStartPosition;

			RecalculateVectors();

			mVelocity += (float)mFinalTime * mAcceleration;
			mVelocity *= -1;
			mAcceleration *= -1;
		}
		public void TransitionIn()
		{
			if (!OffScreen)
				return;

			OffScreen = true;
			MovingOut = false;

			InTransition = true;
			mTime = 0;
			mVelocity = Vector2.Empty;

			mPosition = mStartPosition;
			mDestination = mTargetDestination;

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

		private static void Swap<T>(ref T a, ref T b)
		{
			T c = a;
			a = b;
			b = c;
		}

		public bool OffScreen { get; private set; }

		public void CancelTransition()
		{
			InTransition = false;
			mPosition = mDestination;

			if (MovingOut == false)
				OffScreen = false;
		}

	}

	public enum TransitionType
	{
		None,

		FromTop,
		FromLeft,
		FromRight,
		FromBottom,
	}
}
