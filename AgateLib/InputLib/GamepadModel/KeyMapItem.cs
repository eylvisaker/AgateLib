namespace AgateLib.InputLib.GamepadModel
{
	public class KeyMapItem
	{
		public static KeyMapItem ToLeftStick(IGamepad pad, StickAxis axis, double value)
		{
			return new KeyMapItem(pad)
			{
				KeyMapType = KeyMapType.LeftStick,
				StickAxis = axis,
				StickValue = (float)value
			};
		}

		public static KeyMapItem ToButton(IGamepad gamepad, GamepadButton button)
		{
			return new KeyMapItem(gamepad)
			{
				KeyMapType = KeyMapType.Button,
				Button = button,
			};
		}

		public KeyMapItem(IGamepad pad)
		{
			this.Gamepad = pad;
		}

		public IGamepad Gamepad { get; private set; }

		public float StickValue { get; private set; }

		public StickAxis StickAxis { get; private set; }

		public KeyMapType KeyMapType { get; private set; }

		public GamepadButton Button { get; private set; }
	}
}