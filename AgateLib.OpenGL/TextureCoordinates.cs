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

namespace AgateLib.OpenGL
{
	/// <summary>
	/// Structure to contain source texture coordinates for drawing quads.
	/// </summary>
	public struct TextureCoordinates
	{
		public TextureCoordinates(float left, float top, float right, float bottom)
		{
			Top = top;
			Left = left;
			Bottom = bottom;
			Right = right;
		}
		public float Top;
		public float Bottom;
		public float Left;
		public float Right;
	}
}
