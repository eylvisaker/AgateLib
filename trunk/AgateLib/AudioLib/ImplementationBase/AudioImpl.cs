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
using System.Text;
using AgateLib.Drivers;
using AgateLib.IO;
using System.IO;

namespace AgateLib.AudioLib.ImplementationBase
{
	/// <summary>
	/// Implements Audio class factory.
	/// </summary>
	public abstract class AudioImpl : DriverImplBase
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
	}

}
