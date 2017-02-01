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