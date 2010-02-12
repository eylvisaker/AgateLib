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
using System.Text;

namespace AgateLib
{
	/// <summary>
	/// Base exception class for exceptions which are thrown by AgateLib.
	/// </summary>
	[global::System.Serializable]
	public class AgateException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// Constructs an AgateException.
		/// </summary>
		public AgateException() { }
		/// <summary>
		/// Constructs an AgateException.
		/// </summary>
		public AgateException(string message) : base(message) { }
		/// <summary>
		/// Constructs an AgateException.
		/// </summary>
		public AgateException(Exception inner, string message) : base(message, inner) { }
		/// <summary>
		/// Constructs an AgateException, calling string.Format on the arguments.
		/// </summary>
		public AgateException(string format, params object[] args)
			: base(string.Format(format, args)) { }
		/// <summary>
		/// Constructs an AgateException.
		/// </summary>
		public AgateException(Exception inner, string format, params object[] args)
			: base(string.Format(format, args), inner) { }

#if !XNA
		/// <summary>
		/// Deserializes an AgateException.
		/// </summary>
		protected AgateException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
#endif
	}

	/// <summary>
	/// Exception which is thrown when AgateLib detects that it is used in a way that
	/// may not be portable to different platforms, 
	/// and Core.CrossPlatformDebugLevel is set to Exception.
	/// </summary>
	[global::System.Serializable]
	public class AgateCrossPlatformException : AgateException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// Constructs a new AgateCrossPlatformException object.
		/// </summary>
		public AgateCrossPlatformException() { }
		/// <summary>
		/// Constructs a new AgateCrossPlatformException object.
		/// </summary>
		public AgateCrossPlatformException(string message) : base(message) { }
		/// <summary>
		/// Constructs a new AgateCrossPlatformException object.
		/// </summary>
		public AgateCrossPlatformException(string message, Exception inner) : base(message, inner) { }
#if !XNA
		/// <summary>
		/// Constructs a new AgateCrossPlatformException object from serialization data.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected AgateCrossPlatformException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
#endif
	}
}