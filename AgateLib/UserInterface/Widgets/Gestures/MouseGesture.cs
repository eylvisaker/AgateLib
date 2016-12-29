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
