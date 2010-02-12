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
using AgateLib;

namespace AgateLib.Gui
{
	/// <summary>
	/// Exception which is thrown when there is an error in the AgateLib Gui.
	/// </summary>
	[global::System.Serializable]
	public class AgateGuiException : AgateException
	{
		/// <summary>
		/// Constructs a new AgateGuiException.
		/// </summary>
		public AgateGuiException() { }
		/// <summary>
		/// Constructs a new AgateGuiException.
		/// </summary>
		/// <param name="message"></param>
		public AgateGuiException(string message) : base(message) { }
		/// <summary>
		/// Constructs a new AgateGuiException.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public AgateGuiException(string message, Exception inner) : base(message, inner) { }
#if !XNA
		/// <summary>
		/// Constructs a new AgateGuiException.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected AgateGuiException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
#endif
	}

}
