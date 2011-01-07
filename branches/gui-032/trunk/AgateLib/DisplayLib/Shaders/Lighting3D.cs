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
	/// Lighting3D is the basic 3D shader.  Any driver with support for 3D must implement
	/// this shader.
	/// </summary>
	public class Lighting3D : Implementation.AgateInternalShader
	{
		protected override BuiltInShader BuiltInShaderType
		{
			get { return BuiltInShader.Lighting3D; }
		}
		/// <summary>
		/// Returns the implementation.
		/// </summary>
		protected new Lighting3DImpl Impl { get { return (Lighting3DImpl)base.Impl;}}

		/// <summary>
		/// Projection matrix for 3D view.  Best obtained by Matrix4x4.Projection.
		/// </summary>
		public Matrix4x4 Projection { get { return Impl.Projection; } set { Impl.Projection = value; } }
		/// <summary>
		/// View matrix for 3D view.  Best obtained by Matrix4x4.ViewLookAt.
		/// </summary>
		public Matrix4x4 View { get { return Impl.View; } set { Impl.View = value; } }
		/// <summary>
		/// World matrix for 3D view.
		/// </summary>
		public Matrix4x4 World { get { return Impl.World; } set { Impl.World= value; } }

		/// <summary>
		/// Set to true to enable lighting effects.
		/// </summary>
		public bool EnableLighting { get { return Impl.EnableLighting; } set { Impl.EnableLighting = value; } }
		public Light[] Lights { get { return Impl.Lights; } }
		public Color AmbientLight { get { return Impl.AmbientLight; } set { Impl.AmbientLight = value; } }
	}
}
