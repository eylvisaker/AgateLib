﻿//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class Window : Container
	{
		string mBackgroundStyle = "paper";
		string mFrameStyle = "paper";

		public Window()
		{
		}
		public Window(string name)
		{
			Name = name;
		}
		public Window(params Widget[] contents)
			: base(contents)
		{
		}

		public FontSurface FontSmall { get; set; }

		public string BackgroundStyle
		{
			get { return mBackgroundStyle; }
			set
			{
				mBackgroundStyle = value;
			}
		}
		public string FrameStyle
		{
			get { return mFrameStyle; }
			set
			{
				mFrameStyle = value;
			}
		}

		public override void DrawImpl()
		{
			// draw children after drawing frame
			base.DrawImpl();
		}
		public override void Update(double delta_t, ref bool processInput)
		{
			base.Update(delta_t, ref processInput);
		}

		public override void OnInputButtonDown(AgateLib.InputLib.KeyCode button, ref bool handled)
		{
			base.OnInputButtonDown(button, ref handled);
		}

	}
}
