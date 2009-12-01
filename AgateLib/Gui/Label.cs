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

namespace AgateLib.Gui
{
	/// <summary>
	/// A label displays a text string for the user.  It has no decorations
	/// and does not interact with the user at all.
	/// </summary>
	public class Label : Widget
	{
		/// <summary>
		/// Constructs a label.
		/// </summary>
		public Label() { Name = "Label"; }
		/// <summary>
		/// Constructs a label.
		/// </summary>
		/// <param name="text">The initial text used in the label.</param>
		public Label(string text)
		{
			Name = text;
			Text = text;

			TextAlignment = AgateLib.DisplayLib.OriginAlignment.CenterLeft;
		}

		/// <summary>
		/// Gets or sets where the text should be aligned to.
		/// </summary>
		public DisplayLib.OriginAlignment TextAlignment { get; set; }
	}
}
