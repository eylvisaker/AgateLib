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

namespace AgateLib.Drivers
{
	/// <summary>
	/// Base class all driver classes should implement.
	/// </summary>
	public abstract class DriverImplBase : IDisposable
	{
		/// <summary>
		/// Initialization beyond what the constructor does.
		/// </summary>
		public abstract void Initialize();
		/// <summary>
		/// Disposes of unmanaged resources.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Called by drivers in their Initialize routine to report
		/// which driver was instantiated.
		/// </summary>
		/// <param name="text"></param>
		protected void Report(string text)
		{
			//Console.WriteLine(text);
			System.Diagnostics.Trace.WriteLine(text);
		}
	}
}
