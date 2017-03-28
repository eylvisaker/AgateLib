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
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform;
using AgateLib.UserInterface.Rendering.Transitions;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering.Animators
{
	public class WidgetAnimator : IWidgetAnimator
	{
		WidgetStyle mStyle;
		Rectangle mClientRect;
		IWidgetAnimator mParentCoordinates;

		public IList<IWidgetAnimator> Children { get; private set; }
		public IWidgetAnimator Parent { get; set; }
		public IWidgetAnimator ParentCoordinateSystem
		{
			get
			{
				return mParentCoordinates ?? Parent;
			}
			set { mParentCoordinates = value; }
		}

		public Point ClientWidgetOffset { get { return Widget.ClientWidgetOffset; } }
		public Size WidgetSize
		{
			get
			{
				return Widget.WidgetSize;
				//var widgetSize = ClientRect.Size;
				//var box = mStyle.BoxModel;

				//widgetSize.Width += box.Padding.Left + box.Padding.Right + box.Border.Left + box.Border.Right;
				//widgetSize.Height += box.Padding.Top + box.Padding.Bottom + box.Border.Bottom + box.Border.Top;

				//return widgetSize;
			}
		}
		public Rectangle ClientRect
		{
			get { return mClientRect; }
			set { mClientRect = value; }
		}
		public Rectangle WidgetRect
		{
			get
			{
				return new Rectangle(
					mClientRect.X - ClientWidgetOffset.X,
					mClientRect.Y - ClientWidgetOffset.Y,
					WidgetSize.Width,
					WidgetSize.Height);
			}
		}

		public int X
		{
			get { return mClientRect.X; }
			set { mClientRect.X = value; }
		}
		public int Y
		{
			get { return mClientRect.Y; }
			set { mClientRect.Y = value; }
		}
		public int Width
		{
			get { return mClientRect.Width; }
			set { mClientRect.Width = value; }
		}
		public int Height
		{
			get { return mClientRect.Height; }
			set { mClientRect.Height = value; }
		}

		public Rectangle ClientToScreen(Rectangle value, bool translateForScroll = true)
		{
			Rectangle translated = value;

			translated.Location = ClientToScreen(value.Location, translateForScroll);

			return translated;
		}
		public Point ClientToScreen(Point clientPoint, bool translateForScroll = true)
		{
			if (Parent == null)
				return clientPoint;

			Point translated = ClientToParent(clientPoint);

			if (translateForScroll)
			{
				translated.X -= ScrollOffset.X;
				translated.Y -= ScrollOffset.Y;
			}

			return ParentCoordinateSystem.ClientToScreen(translated);
		}
		public Point ScreenToClient(Point screenPoint)
		{
			if (Parent == null)
				return screenPoint;

			Point translated = ParentToClient(screenPoint);

			return Parent.ScreenToClient(translated);
		}
		public Point ClientToParent(Point clientPoint)
		{
			Point translated = clientPoint;

			translated.X += X;
			translated.Y += Y;

			return translated;
		}
		public Point ParentToClient(Point parentClientPoint)
		{
			Point translated = parentClientPoint;

			translated.X -= X;
			translated.Y -= Y;

			return translated;
		}

		public bool IsDead { get; set; }

		public bool Active { get; private set; }
		public bool Visible { get; set; }

		public Widget Widget => mStyle.Widget;
		public WidgetStyle Style => mStyle; 

		WindowTransitionType mTransitionType;
		public IWidgetTransition Transition { get; private set; }

		public Point ScrollOffset => Style.View.ScrollOffset;

		public bool InTransition
		{
			get
			{
				if (IsDead) return false;
				if (Transition == null) return false;

				return (Transition != null && (Transition.Active || Transition.NeedTransition));
			}
		}

		public WidgetAnimator(WidgetStyle style)
		{
			mStyle = style;
			Children = new List<IWidgetAnimator>();
		}

		public void Update(ClockTimeSpan elapsed)
		{
			if (Transition == null || mTransitionType != mStyle.Transition.Type)
			{
				mTransitionType = mStyle.Transition.Type;
				Transition = TransitionFactory.CreateTransition(mTransitionType);
				Transition.Animator = this;
				Transition.Style = mStyle;

				Transition.Initialize();
			}

			if (Transition.NeedTransition && Transition.Active == false)
			{
				Transition.ActivateTransition();
			}

			if (Transition.Active)
			{
				Transition.Update(elapsed);
			}


			if (Transition.AnimationDead)
				IsDead = true;

			if (Gesture != null)
			{
				AnimateForGesture();
			}
		}

		private void AnimateForGesture()
		{
			switch (Gesture.GestureType)
			{
				case GestureType.Drag:
				case GestureType.Swipe:
				case GestureType.LongPressDrag:
					AnimateDrag();
					break;
			}

		}

		private void AnimateDrag()
		{
			if (Gesture.IsValidForTarget == false)
				return;

			Vector2f delta = new Vector2f(Gesture.CurrentPoint);
			delta -= new Vector2f(Gesture.StartPoint);

			if (Gesture.Axis == AxisType.Horizontal)
				delta.Y = 0;
			else if (Gesture.Axis == AxisType.Vertical)
				delta.X = 0;

			ClientRect = new Rectangle(
				Widget.ClientRect.X + (int)delta.X,
				Widget.ClientRect.Y + (int)delta.Y,
				Widget.ClientRect.Width,
				Widget.ClientRect.Height);
		}

		public override string ToString()
		{
			return "Animator: " + mStyle.Widget.ToString();
		}

		public Gesture Gesture { get; set; }
	}
}
