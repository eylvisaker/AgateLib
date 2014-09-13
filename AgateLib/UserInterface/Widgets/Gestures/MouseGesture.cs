using AgateLib.Geometry;
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
			GestureData.CurrentTime = Core.GetTime();

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
				double delta = Core.GetTime() - GestureData.StartTime;

				if (delta > 1000)
				{
					GestureData.GestureType = GestureType.LongPress;
					GestureData.FocusWidget.OnGestureChange(GestureData);
				}
			}
		}
	}
}
