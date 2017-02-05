using System;
using AgateLib.Geometry;

namespace AgateLib.InputLib.GamepadModel
{
	public class Gamepad : IGamepad
	{
		private readonly IJoystick joystick;
		private bool[] buttons = new bool[Enum.GetValues(typeof(GamepadButton)).Length];
		private Vector2 leftStick;
		private Vector2 rightStick;
		private Point directionPad;

		public Gamepad(IJoystick joystick)
		{
			this.joystick = joystick;
		}

		public event EventHandler<GamepadButtonEventArgs> ButtonPressed;
		public event EventHandler<GamepadButtonEventArgs> ButtonReleased;
		public event EventHandler LeftStickChanged;
		public event EventHandler RightStickChanged;
		public event EventHandler DirectionPadChanged;

		public Vector2 LeftStick
		{
			get { return leftStick; }
			set
			{
				leftStick = value;
				LeftStickChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public Vector2 RightStick
		{
			get { return rightStick; }
			set
			{
				rightStick = value;
				RightStickChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public Point DirectionPad
		{
			get { return directionPad; }
			set
			{
				directionPad = value;
				DirectionPadChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public bool A
		{
			get { return GetButton(GamepadButton.A); }
			set { SetButton(GamepadButton.A, value); }
		}

		public bool B
		{
			get { return GetButton(GamepadButton.B); }
			set { SetButton(GamepadButton.B, value); }
		}

		public bool X
		{
			get { return GetButton(GamepadButton.X); }
			set { SetButton(GamepadButton.X, value); }
		}

		public bool Y
		{
			get { return GetButton(GamepadButton.Y); }
			set { SetButton(GamepadButton.Y, value); }
		}

		public bool LeftBumper
		{
			get { return GetButton(GamepadButton.LeftBumper); }
			set { SetButton(GamepadButton.LeftBumper, value); }
		}

		public bool RightBumper
		{
			get { return GetButton(GamepadButton.RightBumper); }
			set { SetButton(GamepadButton.RightBumper, value); }
		}

		public bool Back
		{
			get { return GetButton(GamepadButton.Back); }
			set { SetButton(GamepadButton.Back, value); }
		}

		public bool Start
		{
			get { return GetButton(GamepadButton.Start); }
			set { SetButton(GamepadButton.Start, value); }
		}

		public bool LeftStickButton
		{
			get { return GetButton(GamepadButton.LeftStickButton); }
			set { SetButton(GamepadButton.LeftStickButton, value); }
		}

		public bool RightStickButton
		{
			get { return GetButton(GamepadButton.RightStickButton); }
			set { SetButton(GamepadButton.RightStickButton, value); }
		}

		public double LeftTrigger { get; set; }
		public double RightTrigger { get; set; }

		internal void ReadFromJoystick()
		{
			if (joystick == null)
				return;

			LeftStick = ReadStickFromJoystick(0);
			RightStick = ReadStickFromJoystick(2);

			DirectionPad = ReadDirectionPadFromJoystick();
		}

		internal Point ReadDirectionPadFromJoystick()
		{
			if (joystick.HatCount == 0)
				return Point.Empty;

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


		private bool GetButton(GamepadButton index)
		{
			return buttons[(int)index];
		}

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

	public interface IGamepad
	{
		event EventHandler<GamepadButtonEventArgs> ButtonPressed;
		event EventHandler<GamepadButtonEventArgs> ButtonReleased;
		event EventHandler LeftStickChanged;
		event EventHandler RightStickChanged;
		event EventHandler DirectionPadChanged;
		Vector2 LeftStick { get; set; }
		Vector2 RightStick { get; set; }
		Point DirectionPad { get; set; }
		bool A { get; set; }
		bool B { get; set; }
		bool X { get; set; }
		bool Y { get; set; }
		bool LeftBumper { get; set; }
		bool RightBumper { get; set; }
		bool Back { get; set; }
		bool Start { get; set; }
		bool LeftStickButton { get; set; }
		bool RightStickButton { get; set; }
		double LeftTrigger { get; set; }
		double RightTrigger { get; set; }
		void SetButton(GamepadButton button, bool value);
	}

}