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
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// Base class for a shader.
	/// </summary>
	public class AgateShader
	{
		AgateShaderImpl impl;

		/// <summary>
		/// Sets the implementation.  If the implementation is already set, then
		/// an exception is thrown.
		/// </summary>
		/// <param name="impl"></param>
		protected internal void SetImpl(AgateShaderImpl impl)
		{
			if (this.impl != null)
				throw new InvalidOperationException("Cannot set impl on an object which already has one.");

			this.impl = impl;
		}

		/// <summary>
		/// Gets the implementation.
		/// </summary>
		public AgateShaderImpl Impl
		{
			get { return impl; }
		}
		/// <summary>
		/// Returns true if this shader has an implementation.
		/// </summary>
		public bool IsValid
		{
			get { return impl != null; }
		}

		internal void BeginInternal()
		{
			impl.Begin();
		}
		internal void EndInternal()
		{
			impl.End();
		}

		/// <summary>
		/// Returns true if this is the currently active shader.
		/// </summary>
		public bool IsActive
		{
			get { return Display.Shader == this; }
		}
		/// <summary>
		/// Activates this shader.
		/// </summary>
		public void Activate()
		{
			Display.Shader = this;
		}
		
	}
}
