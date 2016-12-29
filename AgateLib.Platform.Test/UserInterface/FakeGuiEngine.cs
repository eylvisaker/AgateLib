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
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.Platform.Test.UserInterface
{
	public class FakeRenderer : IGuiRenderer
	{
		public void Draw()
		{
		}
		public void Update(double deltaTime)
		{ }


		public Gesture ActiveGesture { get; set; }


		public bool InTransition
		{
			get { return false; }
		}

		public Gui MyGui { get; set; }

		public IWidgetAdapter Adapter { get; set; }
	}

	public class FakeLayoutEngine : IGuiLayoutEngine
	{
		public void UpdateLayout(Gui gui)
		{
		}
	}
}
