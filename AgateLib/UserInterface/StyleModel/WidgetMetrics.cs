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
using AgateLib.Geometry;

namespace AgateLib.UserInterface.StyleModel
{
	public class WidgetMetrics
	{
		/// <summary>
		/// The minimum dimensions of the widget, including borders, padding and margins.
		/// </summary>
		public Size MinTotalSize { get; set; }
		/// <summary>
		/// The maximum dimensions of the widget, including borders, padding and margins.
		/// </summary>
		public Size MaxTotalSize { get; set; }

		/// <summary>
		/// The actual size of the content area.
		/// </summary>
		public Size ContentSize { get; set; }
		
		/// <summary>
		/// The size of the control, including margins, borders and padding.
		/// </summary>
		public Size BoxSize { get; set; }

		/// <summary>
		/// The size of the control, including margins, borders and padding in 
		/// the absence of any constraints.
		/// </summary>
		public Size NaturalBoxSize { get; set; }
	}
}