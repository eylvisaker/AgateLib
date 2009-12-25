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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui.Layout
{
	public class HorizontalBox : BoxLayoutBase
	{
		protected override void DoLayoutInternal()
		{
			DoBoxLayout(true);
		}

		protected override Size RecalcMinSizeInternal()
		{
			return RecalcMinSizeBox(true);
		}

		public override bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
		{
			switch (keyCode)
			{
				case AgateLib.InputLib.KeyCode.Right:
				case AgateLib.InputLib.KeyCode.Left:
					return true;

				default:
					return false;
			}
		}

		public override Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction)
		{
			if (direction == Direction.Up || direction == Direction.Down)
				return null;

			GuiRoot root = Root(container);
			int index = GetParentIndex(container, currentFocus);


			switch (direction)
			{
				case Direction.Left:
					return GetNextChild(container, index, -1);

				case Direction.Right:
					return GetNextChild(container, index, 1);
			}

			throw new InvalidOperationException();
		}


	}
}