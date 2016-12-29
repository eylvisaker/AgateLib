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

namespace AgateLib.Quality
{
	/// <summary>
	/// Exception thrown when an assert from AgateLib.Quality fails.
	/// </summary>
	public class AssertFailedException : Exception
	{
		/// <summary>
		/// Constructs the exception.
		/// </summary>
		public AssertFailedException()
		{
		}

		/// <summary>
		/// Constructs the exception.
		/// </summary>
		/// <param name="message"></param>
		public AssertFailedException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructs the exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public AssertFailedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}