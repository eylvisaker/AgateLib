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
using System.Text;
using AgateLib.Drivers;
using AgateLib.IO;
using System.IO;

namespace AgateLib.AudioLib.ImplementationBase
{
	/// <summary>
	/// Implements Audio class factory.
	/// </summary>
	public abstract class AudioImpl : IDriverCore
	{

		/// <summary>
		/// This function is called once a frame to allow the Audio driver to update
		/// information.  There is no need to call base.Update() if overriding this
		/// function.
		/// </summary>
		public virtual void Update()
		{
		}


		/// <summary>
		/// This function is called when a Caps property is inspected.
		/// It should return false for any unknown value.
		/// </summary>
		/// <param name="audioBoolCaps"></param>
		/// <returns></returns>
		protected internal abstract bool CapsBool(AudioBoolCaps audioBoolCaps);

		/// <summary>
		/// Destroys the AudioImpl.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Override to dispose of local resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{ }

		/// <summary>
		/// Override to provide initialization after the connection to the Audio class is made.
		/// </summary>
		public abstract void Initialize();
	}

}
