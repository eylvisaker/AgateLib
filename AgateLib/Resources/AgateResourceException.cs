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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Resources
{
	/// <summary>
	/// AgateException derived exception class used when there is a problem reading
	/// from a resource file.
	/// </summary>
	[global::System.Serializable]
	public class AgateResourceException : AgateException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// 
		/// </summary>
		public AgateResourceException() { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public AgateResourceException(string message) : base(message) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public AgateResourceException(string format, params object[] args)
			: base(string.Format(format, args)) 
		{ }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public AgateResourceException(string message, Exception inner) : base(inner, message) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public AgateResourceException(Exception inner, string format, params object[] args)
			: base(inner, string.Format(format, args))
		{ }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected AgateResourceException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
