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
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	/// <summary>
	/// Base class for the implementation of the Lighting2D shader.
	/// </summary>
	public abstract class Lighting2DImpl : AgateShaderImpl 
	{
		/// <summary>
		/// Constructs a Lighting2DImpl object.
		/// </summary>
		protected Lighting2DImpl()
		{
			Lights = new List<Light>();
		}

		/// <summary>
		/// Gets the maximum number of lights.
		/// </summary>
		public abstract int MaxActiveLights { get; }
		/// <summary>
		/// Gets the list of lights.
		/// </summary>
		public List<Light> Lights { get; private set; }
		/// <summary>
		/// Sets the ambient light color.
		/// </summary>
		public abstract Color AmbientLight { get; set; }
		
		/// <summary>
		/// Gets or sets the coordinate system used (orthogonal projection).
		/// </summary>
		public abstract Rectangle CoordinateSystem { get; set; }
	}
}
