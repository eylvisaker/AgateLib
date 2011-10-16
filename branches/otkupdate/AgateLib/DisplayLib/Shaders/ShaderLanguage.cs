﻿//     The contents of this file are subject to the Mozilla Public License
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

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// Indicates a shader language.
	/// </summary>
	public enum ShaderLanguage
	{
		/// <summary>
		/// No shading language.
		/// </summary>
		None,

		/// <summary>
		/// Microsoft's high level shading language.
		/// </summary>
		Hlsl,
		/// <summary>
		/// The OpenGL shading language.
		/// </summary>
		Glsl,
		// Cg,

	}
}