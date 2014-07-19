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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.DisplayLib.Shaders
{
	/*
	public abstract class ShaderProgram
	{
		public abstract PixelShader PixelShader { get; }
		public abstract VertexShader VertexShader { get; }

		public abstract void SetUniform(string name, params float[] v);
		public abstract void SetUniform(string name, params int[] v);
		public abstract void SetUniform(string name, Matrix4 matrix);

		public void SetUniform(string name, Vector2 v)
		{
			SetUniform(name, v.X, v.Y);
		}
		public void SetUniform(string name, Vector3 v)
		{
			SetUniform(name, v.X, v.Y, v.Z);
		}
		public void SetUniform(string name, Vector4 v)
		{
			SetUniform(name, v.X, v.Y, v.Z, v.W);
		}
		public void SetUniform(string name, Color color)
		{
			SetUniform(name, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		public VertexLayout VertexDefinition { get; set; }

		public abstract void Render(RenderHandler handler, object obj);
	}

	public delegate void RenderHandler(object obj);
	 * */
}
