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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib
{
	public class GamepadMap
	{
		public GamepadMap()
		{
			AxisMap = new GamepadMapTarget[6];
			ButtonMap = new GamepadMapTarget[16];

			InitializeMapTarget(AxisMap);
			InitializeMapTarget(ButtonMap);

			InitializeXboxControllerMap();
		}

		private void InitializeXboxControllerMap()
		{
			ButtonMap[0] = GamepadMapTarget.DPadUp;
			ButtonMap[1] = GamepadMapTarget.DPadDown;
			ButtonMap[2] = GamepadMapTarget.DPadLeft;
			ButtonMap[3] = GamepadMapTarget.DPadRight;
			ButtonMap[4] = GamepadMapTarget.Start;
			ButtonMap[5] = GamepadMapTarget.Select;
			ButtonMap[6] = GamepadMapTarget.LeftStickButton;
			ButtonMap[7] = GamepadMapTarget.RightStickButton;
			ButtonMap[8] = GamepadMapTarget.LeftBumper;
			ButtonMap[9] = GamepadMapTarget.RightBumper;
			ButtonMap[10] = GamepadMapTarget.A;
			ButtonMap[11] = GamepadMapTarget.B;
			ButtonMap[12] = GamepadMapTarget.X;
			ButtonMap[13] = GamepadMapTarget.Y;
			ButtonMap[14] = GamepadMapTarget.Home;
		}

		private void InitializeMapTarget(GamepadMapTarget[] map)
		{
			for (int i = 0; i < map.Length; i++)
				map[i] = (GamepadMapTarget)i;
		}

		public GamepadMapTarget[] AxisMap { get; set; }
		public GamepadMapTarget[] ButtonMap { get; set; }
	}


	public enum GamepadMapTarget
	{
		None = -1,
		LeftX,
		LeftY,
		RightX,
		RightY,
		LeftTrigger,
		RightTrigger,
		A,
		B,
		X,
		Y,
		LeftBumper,
		RightBumper,
		Select,
		Start,
		LeftStickButton,
		RightStickButton,
		DPadUp,
		DPadDown,
		DPadLeft,
		DPadRight,
		Home,
	}

}
