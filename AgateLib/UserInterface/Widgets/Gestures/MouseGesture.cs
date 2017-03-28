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

namespace AgateLib.UserInterface.Widgets.Gestures
{
	class MouseGesture : IGestureController
	{
		public Gesture GestureData { get; set; }

		public void Update()
		{
			if (GestureData.GestureType == GestureType.Touch || GestureData.GestureType == GestureType.LongPress)
			{
				var delta = GestureData.CurrentPoint;
				delta.X -= GestureData.StartPoint.X;
				delta.Y -= GestureData.StartPoint.Y;

				if (Math.Abs(delta.X) + Math.Abs(delta.Y) > 8)
				{
					if (GestureData.GestureType == GestureType.Touch)
						GestureData.GestureType = GestureType.Drag;
					else
						GestureData.GestureType = GestureType.LongPressDrag;
				}

				if (GestureData.FocusWidget.AnyDirectionGestures == false)
				{
					if (delta.X > delta.Y)
						GestureData.Axis = AxisType.Horizontal;
					else
						GestureData.Axis = AxisType.Vertical;
				}

				GestureData.FocusWidget.OnGestureChange(GestureData);
			}
		}

		public void OnBegin()
		{
			GestureData.FocusWidget.OnGestureBegin(GestureData);
		}

		public void OnComplete()
		{
			GestureData.CurrentTime = AgateApp.GetTimeInMilliseconds();

			if (GestureData.GestureType == GestureType.Drag && GestureData.Velocity.Magnitude > 1000)
				GestureData.GestureType = GestureType.Swipe;

			GestureData.FocusWidget.OnGestureComplete(GestureData);
		}

		public void OnTimePass()
		{
			if (GestureData.GestureType == GestureType.None)
				return;

			if (GestureData.GestureType == GestureType.Touch)
			{
				double delta = AgateApp.GetTimeInMilliseconds() - GestureData.StartTime;

				if (delta > 1000)
				{
					GestureData.GestureType = GestureType.LongPress;
					GestureData.FocusWidget.OnGestureChange(GestureData);
				}
			}
		}
	}
}
