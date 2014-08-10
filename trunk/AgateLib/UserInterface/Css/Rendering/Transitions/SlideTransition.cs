using AgateLib.Geometry;
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

		Vector2 mStartPosition;
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

			mTargetDestination = new Vector2(Animator.ClientRect.X, Animator.ClientRect.Y);
			mStartPosition = mTargetDestination;
			mVelocity = Vector2.Empty;

			switch (TransitionType)
			{
				case CssTransitionDirection.Left:
					mStartPosition.X = -Widget.Width - 20;
					break;

				case CssTransitionDirection.Top:
					mStartPosition.Y = -Widget.Height - 20;
					break;

				case CssTransitionDirection.Right:
					mStartPosition.X = Widget.Parent.Width + 20;
					break;

				case CssTransitionDirection.Bottom:
					mStartPosition.Y = Widget.Parent.Height + 20;
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

			SetInitialWidgetProperties();

		}

		protected override void StartTransition()
		{
			if (mFirstTransition)
			{
				TransitionIn();
				mFirstTransition = false;
			}
			else if (Widget.Visible)
				TransitionIn();
			else
				TransitionOut();
		}

		void TransitionOut()
		{
			Initialize();

			Active = true;
			movingOut = true;
			mTime = 0;
			mVelocity = Vector2.Empty;

			mPosition = mTargetDestination;
			mDestination = mStartPosition;

			RecalculateVectors();

			mVelocity += (float)mFinalTime * mAcceleration;
			mVelocity *= -1;
			mAcceleration *= -1;
		}
		void TransitionIn()
		{
			Initialize();

			Active = true;
			movingOut = false;

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

				Animator.Visible = Widget.Visible;

				if (movingOut == false)
				{
					Widget.ClientRect = Animator.ClientRect;
				}
			}
			else
			{
				SetWidgetPosition(mPosition);
			}
		}

		void SetWidgetPosition(Vector2 pos)
		{
			Widget.X = (int)(pos.X + 0.5);
			Widget.Y = (int)(pos.Y + 0.5);

			Widget.ClientRect = new Rectangle(
				Widget.ClientRect.Location, Animator.ClientRect.Size);

			Widget.ClientWidgetOffset = Animator.ClientWidgetOffset;
			Widget.WidgetSize = Animator.WidgetSize;
		}
		private void SetInitialWidgetProperties()
		{
			Animator.Visible = true;

			Widget.ClientRect = Animator.ClientRect;

			SetWidgetPosition(mStartPosition);

		}
	}
}
