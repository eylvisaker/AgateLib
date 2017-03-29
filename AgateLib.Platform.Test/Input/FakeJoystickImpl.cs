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
using AgateLib.InputLib;
using AgateLib.InputLib.ImplementationBase;

namespace AgateLib.Platform.Test.Input
{
	public class FakeJoystickImpl : IJoystickImpl
	{
		private static int NextId = 1;

		private List<double> axes = new List<double>();
		private List<bool> buttons = new List<bool>();
		private List<HatState> hats = new List<HatState>();
		private int id;

		public FakeJoystickImpl()
		{
			id = NextId++;
			Name = "Joystick " + id;
			Guid = Guid.NewGuid();

			AxisCount = 6;
			ButtonCount = 10;
			HatCount = 1;
		}

		public int AxisCount
		{
			get { return axes.Count; }
			set { SetListSize(axes, value); }
		}

		public int ButtonCount
		{
			get { return buttons.Count; }
			set { SetListSize(buttons, value); }
		}

		public int HatCount
		{
			get { return hats.Count; }
			set { SetListSize(hats, value); }
		}

		public string Name { get; set; }

		public Guid Guid { get; set; }

		public bool GetButtonState(int buttonIndex)
		{
			return buttons[buttonIndex];
		}

		public double GetAxisValue(int axisIndex)
		{
			return axes[axisIndex];
		}

		public HatState GetHatState(int hatIndex)
		{
			return hats[hatIndex];
		}

		public void SetButtonState(int buttonIndex, bool value)
		{
			buttons[buttonIndex] = value;
		}

		public void SetAxisValue(int axisIndex, double value)
		{
			axes[axisIndex] = value;
		}

		public void SetHatState(int hatIndex, HatState value)
		{
			hats[hatIndex] = value;
		}

		public void Recalibrate()
		{
		}

		public double AxisThreshold { get; set; }

		public bool PluggedIn { get; set; }

		public void Poll()
		{
		}

		private void SetListSize<T>(List<T> list, int newSize)
		{
			while (list.Count < newSize)
				list.Add(default(T));
			while (list.Count > newSize)
				list.RemoveAt(list.Count - 1);
		}
	}
}