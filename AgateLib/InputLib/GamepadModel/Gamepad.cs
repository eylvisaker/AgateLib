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
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.InputLib.GamepadModel
{
	/// <summary>
	/// Class which represents a gamepad.
	/// </summary>
	public class Gamepad : IGamepad
	{
		private readonly IJoystick joystick;
		private bool[] buttons = new bool[Enum.GetValues(typeof(GamepadButton)).Length];
		private Vector2 leftStick;
		private Vector2 rightStick;
		private Point directionPad;

		/// <summary>
		/// Constructs a Gamepad object and sets it to track a low-level joystick.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="joystick"></param>
		public Gamepad(int index, IJoystick joystick)
		{
			this.Index = index;
			this.joystick = joystick;
		}

		/// <summary>
		/// Event raised when a button is pressed.
		/// </summary>
		public event EventHandler<GamepadButtonEventArgs> ButtonPressed;

		/// <summary>
		/// Event raised when a button is released.
		/// </summary>
		public event EventHandler<GamepadButtonEventArgs> ButtonReleased;

		/// <summary>
		/// Event raised when the left stick is moved.
		/// </summary>
		public event EventHandler LeftStickChanged;

		/// <summary>
		/// Event raised when the right stick is moved.
		/// </summary>
		public event EventHandler RightStickChanged;

		/// <summary>
		/// Event raised when the direction pad is changed.
		/// </summary>
		public event EventHandler DirectionPadChanged;

		/// <summary>
		/// The index of this gamepad.
		/// </summary>
		public int Index { get; }

		/// <summary>
		/// The current value of the left stick.
		/// </summary>
		public Vector2 LeftStick
		{
			get { return leftStick; }
			set
			{
				leftStick = value;
				LeftStickChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// The current value of the right stick.
		/// </summary>
		public Vector2 RightStick
		{
			get { return rightStick; }
			set
			{
				rightStick = value;
				RightStickChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// The current value of the direction pad.
		/// </summary>
		public Point DirectionPad
		{
			get { return directionPad; }
			set
			{
				directionPad = value;
				DirectionPadChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// The current state of the A button.
		/// </summary>
		public bool A
		{
			get { return GetButton(GamepadButton.A); }
			set { SetButton(GamepadButton.A, value); }
		}

		/// <summary>
		/// The current state of the B button.
		/// </summary>
		public bool B
		{
			get { return GetButton(GamepadButton.B); }
			set { SetButton(GamepadButton.B, value); }
		}

		/// <summary>
		/// The current state of the X button.
		/// </summary>
		public bool X
		{
			get { return GetButton(GamepadButton.X); }
			set { SetButton(GamepadButton.X, value); }
		}

		/// <summary>
		/// The current state of the Y button.
		/// </summary>
		public bool Y
		{
			get { return GetButton(GamepadButton.Y); }
			set { SetButton(GamepadButton.Y, value); }
		}

		/// <summary>
		/// The current state of the left bumper.
		/// </summary>
		public bool LeftBumper
		{
			get { return GetButton(GamepadButton.LeftBumper); }
			set { SetButton(GamepadButton.LeftBumper, value); }
		}

		/// <summary>
		/// The current state of the right bumper.
		/// </summary>
		public bool RightBumper
		{
			get { return GetButton(GamepadButton.RightBumper); }
			set { SetButton(GamepadButton.RightBumper, value); }
		}

		/// <summary>
		/// The current state of the back button.
		/// </summary>
		public bool Back
		{
			get { return GetButton(GamepadButton.Back); }
			set { SetButton(GamepadButton.Back, value); }
		}

		/// <summary>
		/// The current state of the start button.
		/// </summary>
		public bool Start
		{
			get { return GetButton(GamepadButton.Start); }
			set { SetButton(GamepadButton.Start, value); }
		}

		/// <summary>
		/// The current state of the left stick button.
		/// </summary>
		public bool LeftStickButton
		{
			get { return GetButton(GamepadButton.LeftStickButton); }
			set { SetButton(GamepadButton.LeftStickButton, value); }
		}

		/// <summary>
		/// The current state of the right stick button.
		/// </summary>
		public bool RightStickButton
		{
			get { return GetButton(GamepadButton.RightStickButton); }
			set { SetButton(GamepadButton.RightStickButton, value); }
		}

		/// <summary>
		/// The current state of the left trigger.
		/// </summary>
		public double LeftTrigger { get; set; }

		/// <summary>
		/// The current state of the right trigger.
		/// </summary>
		public double RightTrigger { get; set; }
		
		/// <summary>
		/// Gets a button state by index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool GetButton(GamepadButton index)
		{
			return buttons[(int)index];
		}

		/// <summary>
		/// Sets a button state by index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void SetButton(GamepadButton index, bool value)
		{
			var oldValue = GetButton(index);

			if (oldValue == value)
				return;

			buttons[(int)index] = value;

			if (value)
				OnButtonPressed(index);
			else
				OnButtonReleased(index);
		}

		internal Point ReadDirectionPadFromJoystick()
		{
			if (joystick.HatCount == 0)
				return Point.Zero;

			var state = joystick.HatState(0);

			Point result = new Point();

			if ((state & HatState.Up) != 0) result.Y = -1;
			if ((state & HatState.Down) != 0) result.Y = 1;
			if ((state & HatState.Left) != 0) result.X = -1;
			if ((state & HatState.Right) != 0) result.X = 1;

			return result;
		}

		internal Vector2 ReadStickFromJoystick(int stickIndex)
		{
			return new Vector2(
				joystick.AxisState(stickIndex * 2),
				joystick.AxisState(stickIndex * 2 + 1));
		}

		private void OnButtonPressed(GamepadButton button)
		{
			ButtonPressed?.Invoke(this,
				new GamepadButtonEventArgs(button));
		}

		private void OnButtonReleased(GamepadButton button)
		{
			ButtonReleased?.Invoke(this,
				new GamepadButtonEventArgs(button));
		}

	}

	/// <summary>
	/// Interface for a gamepad
	/// </summary>
	public interface IGamepad
	{
		/// <summary>
		/// Event raised when a button is pressed.
		/// </summary>
		event EventHandler<GamepadButtonEventArgs> ButtonPressed;

		/// <summary>
		/// Event raised when a button is released.
		/// </summary>
		event EventHandler<GamepadButtonEventArgs> ButtonReleased;

		/// <summary>
		/// Event raised when the left stick is moved.
		/// </summary>
		event EventHandler LeftStickChanged;

		/// <summary>
		/// Event raised when the right stick is moved.
		/// </summary>
		event EventHandler RightStickChanged;

		/// <summary>
		/// Event raised when the direction pad is changed.
		/// </summary>
		event EventHandler DirectionPadChanged;

		/// <summary>
		/// The current value of the left stick.
		/// </summary>
		Vector2 LeftStick { get; set; }

		/// <summary>
		/// The current value of the right stick.
		/// </summary>
		Vector2 RightStick { get; set; }

		/// <summary>
		/// The current value of the direction pad.
		/// </summary>
		Point DirectionPad { get; set; }
		
		/// <summary>
		/// The current state of the A button.
		/// </summary>
		bool A { get; set; }

		/// <summary>
		/// The current state of the B button.
		/// </summary>
		bool B { get; set; }

		/// <summary>
		/// The current state of the X button.
		/// </summary>
		bool X { get; set; }

		/// <summary>
		/// The current state of the Y button.
		/// </summary>
		bool Y { get; set; }

		/// <summary>
		/// The current state of the left bumper.
		/// </summary>
		bool LeftBumper { get; set; }

		/// <summary>
		/// The current state of the right bumper.
		/// </summary>
		bool RightBumper { get; set; }

		/// <summary>
		/// The current state of the back button.
		/// </summary>
		bool Back { get; set; }

		/// <summary>
		/// The current state of the start button.
		/// </summary>
		bool Start { get; set; }

		/// <summary>
		/// The current state of the left stick button.
		/// </summary>
		bool LeftStickButton { get; set; }

		/// <summary>
		/// The current state of the right stick button.
		/// </summary>
		bool RightStickButton { get; set; }

		/// <summary>
		/// The current state of the left trigger.
		/// </summary>
		double LeftTrigger { get; set; }

		/// <summary>
		/// The current state of the right trigger.
		/// </summary>
		double RightTrigger { get; set; }

		/// <summary>
		/// The index of this gamepad.
		/// </summary>
		int Index { get; }

		/// <summary>
		/// Sets a button state by index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetButton(GamepadButton button, bool value);

		/// <summary>
		/// Gets a button state by index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool GetButton(GamepadButton button);
	}

}