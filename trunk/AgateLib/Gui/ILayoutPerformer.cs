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

namespace AgateLib.Gui
{
	/// <summary>
	/// Interface for an object which performs layout of GUI components.
	/// </summary>
	public interface ILayoutPerformer
	{
		/// <summary>
		/// Called when the GUI components must be layed out.
		/// </summary>
		/// <param name="container">The container in which the layout occurs.</param>
		void DoLayout(Container container);
		/// <summary>
		/// Calculates the minimum size of the container.
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		Size RecalcMinSize(Container container);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyCode"></param>
		/// <returns></returns>
		bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		/// <param name="currentFocus"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction);
	}
}
