using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets
{
	public class InputMap
	{
		Dictionary<KeyCode, GuiInput> mKeyMap = new Dictionary<KeyCode, GuiInput>();
		Dictionary<int, GuiInput> mJoystickButtonMap = new Dictionary<int, GuiInput>();

		public void Clear()
		{
			mKeyMap.Clear();
			mJoystickButtonMap.Clear();
		}

		public InputMap()
		{ }
		public static InputMap CreateDefaultMapping()
		{
			InputMap retval = new InputMap();

			retval.AddKey(KeyCode.Up, GuiInput.Up);
			retval.AddKey(KeyCode.Down, GuiInput.Down);
			retval.AddKey(KeyCode.Left, GuiInput.Left);
			retval.AddKey(KeyCode.Right, GuiInput.Right);
			retval.AddKey(KeyCode.Enter, GuiInput.Accept);
			retval.AddKey(KeyCode.Escape, GuiInput.Cancel);
			retval.AddKey(KeyCode.PageUp, GuiInput.PageUp);
			retval.AddKey(KeyCode.PageDown, GuiInput.PageDown);

			retval.AddKey(KeyCode.NumPad8, GuiInput.Up);
			retval.AddKey(KeyCode.NumPad4, GuiInput.Left);
			retval.AddKey(KeyCode.NumPad6, GuiInput.Right);
			retval.AddKey(KeyCode.NumPad2, GuiInput.Down);
			retval.AddKey(KeyCode.NumPad0, GuiInput.Cancel);
			retval.AddKey(KeyCode.NumPadPlus, GuiInput.Menu);
			retval.AddKey(KeyCode.NumPadMinus, GuiInput.Switch);
			retval.AddKey(KeyCode.NumPad9, GuiInput.PageUp);
			retval.AddKey(KeyCode.NumPad3, GuiInput.PageDown);

			return retval;
		}

		public void AddKey(KeyCode keyCode, GuiInput guiInput)
		{
			mKeyMap[keyCode] = guiInput;
		}

		public GuiInput MapKey(KeyCode keyCode)
		{
			if (mKeyMap.ContainsKey(keyCode) == false)
				return GuiInput.None;

			return mKeyMap[keyCode];
		}
	}
}
