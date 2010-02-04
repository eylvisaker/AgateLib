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
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	/// <summary>
	/// Base class for implementing the Lighting3D shader.
	/// </summary>
	public abstract class Lighting3DImpl : AgateShaderImpl  
	{
		/// <summary>
		/// Constructs a Lighting3DImpl object.
		/// </summary>
		public Lighting3DImpl()
		{
			AmbientLight = Color.White;
			EnableLighting = true;
		}

		/// <summary>
		/// Gets or sets the projection matrix.
		/// </summary>
		public abstract Matrix4x4 Projection { get; set; }
		/// <summary>
		/// Gets or sets the projection matrix.
		/// </summary>
		public abstract Matrix4x4 View { get; set; }
		/// <summary>
		/// Gets or sets the projection matrix.
		/// </summary>
		public abstract Matrix4x4 World { get; set; }

		/// <summary>
		/// Gets or sets the lights.
		/// </summary>
		public abstract Light[] Lights { get; }
		/// <summary>
		/// Gets or sets the ambient color.
		/// </summary>
		public abstract Color AmbientLight { get; set; }
		/// <summary>
		/// Gets or sets whether or not lighting should be enabled.
		/// </summary>
		public virtual bool EnableLighting { get; set; }

	}
}
