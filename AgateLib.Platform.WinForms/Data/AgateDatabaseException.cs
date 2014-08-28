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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	/// <summary>
	/// Exception which is thrown if there is an error when working with the database.
	/// </summary>
	public class AgateDatabaseException : AgateException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		public AgateDatabaseException()
		{
			ErrorCount = 1;
		}
		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		/// <param name="message"></param>
		public AgateDatabaseException(string message)
			: base(message)
		{
			ErrorCount = 1;
		}
		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public AgateDatabaseException(string message, Exception inner)
			: base(message, inner)
		{
			ErrorCount = 1;
		}
		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public AgateDatabaseException(string format, params object[] args)
			: base(format, args)
		{
			ErrorCount = 1;
		}
		internal AgateDatabaseException(int errorCount, string message)
			: base(message)
		{
			ErrorCount = errorCount;
		}
		internal AgateDatabaseException(int errorCount, string format, params object[] args)
			: base(format, args)
		{
			ErrorCount = errorCount;
		}

		internal int ErrorCount { get; set; }
	}
}
