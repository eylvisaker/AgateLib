namespace AgateLib.InputLib.GamepadModel
{
	public class GamepadButtonEventArgs
	{
		public GamepadButtonEventArgs(GamepadButton button)
		{
			this.Button = button;
		}

		public GamepadButton Button { get; }
	}
}