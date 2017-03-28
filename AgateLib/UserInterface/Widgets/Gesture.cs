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

namespace AgateLib.UserInterface.Widgets
{
	public class Gesture
	{
		public GestureType GestureType { get; internal set; }

		public Point StartPoint { get; set; }
		public Point CurrentPoint { get; set; }

		public double StartTime { get; set; }
		public double CurrentTime { get; set; }
		public Widget FocusWidget { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public AxisType Axis { get; set; }
		/// <summary>
		/// A value set by the focus widget to indicate to the rendering engine which widget is actually affected.
		/// </summary>
		public Widget TargetWidget { get; set; }

		/// <summary>
		/// A value set by the focus widget to indicate whether this gesture is valid for the target widget. If it is not
		/// valid, the target widget will not be drawn as cooperating with the motion.
		/// </summary>
		public bool IsValidForTarget { get; set; }

		internal void Initialize(GestureType gestureType, Point startPoint, Widget gestureWidget)
		{
			this.GestureType = gestureType;
			this.StartPoint = startPoint;
			this.FocusWidget = this.TargetWidget = gestureWidget;
			this.Axis = AxisType.Unknown;
			this.StartTime = AgateApp.GetTimeInMilliseconds();

			IsValidForTarget = false;
		}

		public Vector2f Velocity
		{
			get
			{
				Vector2f delta = AmountDragged;

				delta /= CurrentTime - StartTime;
				delta *= 1000;

				return delta;
			}
		}

		public GestureEffect Effect { get; set; }

		public Vector2f AmountDragged { get { return new Vector2f(CurrentPoint.X, CurrentPoint.Y) - new Vector2f(StartPoint.X, StartPoint.Y); } }
	}


	public enum GestureType
	{
		None,

		Touch,
		LongPress,
		/// <summary>
		/// Gesture where an object is touched and then immediately the finger moves.
		/// </summary>
		Swipe,
		/// <summary>
		/// Gesture where an object is moved after it has been touched and held.
		/// </summary>
		Drag,
		LongPressDrag,
	}
}
