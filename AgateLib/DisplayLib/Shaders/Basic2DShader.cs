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
	/// The default 2D shader.  This shader supports no effects, and must be implemented
	/// by every AgateLib display driver.
	/// </summary>
	public class Basic2DShader : AgateInternalShader  
	{
		/// <summary>
		/// Constructs a 2D shader.
		/// </summary>
		public Basic2DShader()
		{

		}
		/// <summary>
		/// Returns the implementation.
		/// </summary>
		protected new Basic2DImpl Impl
		{
			get { return (Basic2DImpl)base.Impl; }
		}

		protected override BuiltInShader BuiltInShaderType
		{
			get { return BuiltInShader.Basic2DShader; }
		}

		/// <summary>
		/// Gets or sets the coordinate system used for drawing.
		/// The default for any render target is to use a one-to-one
		/// mapping for pixels.
		/// </summary>
		public Rectangle CoordinateSystem
		{
			get { return Impl.CoordinateSystem; }
			set { Impl.CoordinateSystem = value; }
		}
	}
}
