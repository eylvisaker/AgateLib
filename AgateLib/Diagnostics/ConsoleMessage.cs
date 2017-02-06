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
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Represents a message in the console window.
	/// </summary>
	public class ConsoleMessage
	{
		string text;

		/// <summary>
		/// Gets or sets the text of a console message.
		/// </summary>
		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				Layout = null;
			}
		}

		/// <summary>
		/// Gets or sets the time the console message was logged.
		/// </summary>
		public long Time { get; set; }

		/// <summary>
		/// Gets or sets the type of console message. This is used to determine how the console message is displayed.
		/// </summary>
		public ConsoleMessageType MessageType { get; set; }

		internal ContentLayout Layout { get; set; }

		internal void ClearCache()
		{
			Layout = null;
		}
	}
}
