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
	/// <summary>
	/// Class which lays out GUI widgets from top to bottom.
	/// </summary>
	public class VerticalBox : BoxLayoutBase
	{
		/// <summary>
		/// Performs the layout.
		/// </summary>
		protected override void DoLayoutInternal()
		{
			DoBoxLayout(false);
		}
		/// <summary>
		/// Recalculates the minimum size.
		/// </summary>
		/// <returns></returns>
		protected override Size RecalcMinSizeInternal()
		{
			return RecalcMinSizeBox(false);
		}
		/// <summary>
		/// Returns true for key up or key down.
		/// </summary>
		/// <param name="keyCode"></param>
		/// <returns></returns>
		public override bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
		{
			switch (keyCode)
			{
				case AgateLib.InputLib.KeyCode.Up:
				case AgateLib.InputLib.KeyCode.Down:
					return true;

				default:
					return false;
			}
		}
		/// <summary>
		/// Tests whether or not focus can be moved in the specified direction.
		/// </summary>
		/// <param name="container"></param>
		/// <param name="currentFocus"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public override Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction)
		{
			if (direction == Direction.Right || direction == Direction.Left)
				return null;

			GuiRoot root = Root(container);
			int index = GetParentIndex(container, root.FocusControl);

			switch (direction)
			{
				case Direction.Up:
					return GetNextChild(container, index, -1);

				case Direction.Down:
					return GetNextChild(container, index, 1);

			}

			throw new InvalidOperationException();
		}
	}
}