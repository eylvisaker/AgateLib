using System.Collections.Generic;

namespace AgateLib.InputLib.GamepadModel
{
	public class KeyboardGamepadMap
	{
		Dictionary<KeyCode, KeyMapItem> keyMap = new Dictionary<KeyCode, KeyMapItem>();

		public void Add(KeyCode keyCode, KeyMapItem keyMapItem)
		{
			keyMap.Add(keyCode, keyMapItem);
		}

		public bool ContainsKey(KeyCode key)
		{
			return keyMap.ContainsKey(key);
		}

		public KeyMapItem this[KeyCode key] => keyMap[key];
	}
}