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
	/// A class which implements the ILayoutPerformer interface.
	/// This class allows GUI elements to be positioned manually.
	/// </summary>
	public class Grid : ILayoutPerformer
	{
		/// <summary>
		/// Always returns false.
		/// </summary>
		public bool DoingLayout
		{
			get { return false; }
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="container"></param>
		public void DoLayout(Container container)
		{
			return;
		}

		/// <summary>
		/// Returns the size of the container.
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		public Size RecalcMinSize(Container container)
		{
			return container.Size;
		}

		/// <summary>
		/// The grid layout does not accept any input keys, so this always
		/// returns false.
		/// </summary>
		/// <param name="keyCode"></param>
		/// <returns></returns>
		public bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
		{
			return false;
		}

		/// <summary>
		/// Does not perform directional passing of focus to other controls.
		/// </summary>
		/// <param name="container"></param>
		/// <param name="currentFocus"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction)
		{
			return null;
		}

	}
}
