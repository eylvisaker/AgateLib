using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib
{
	public static class GamepadMapDatabase
	{
		static Dictionary<Guid, string> mMapStrings = new Dictionary<Guid, string>();

		/// <summary>
		/// Finds the mapping from the joystick inputs to the gamepad inputs.
		/// Returns a default GamepadMap object if nothing is found.
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static GamepadMap FindMap(Guid guid)
		{
			return new GamepadMap();
		}
	}
}
