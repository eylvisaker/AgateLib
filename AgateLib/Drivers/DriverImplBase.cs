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
using AgateLib.Diagnostics;
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
		/// Disposes of owned resources.
		/// </summary>
		public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of owned resources. If disposing is fals, only unmanaged resources
        /// should be disposed.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }

		/// <summary>
		/// Called by drivers in their Initialize routine to report
		/// which driver was instantiated.
		/// </summary>
		/// <param name="text"></param>
		protected void Report(string text)
		{
			Log.WriteLine(text);
		}
	}
}
