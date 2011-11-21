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
	[Obsolete]
	public abstract class Effect
	{
		public abstract string Technique { get; set; }

		public abstract int Passes { get; }

		public abstract void Begin();
		public abstract void BeginPass(int passIndex);
		public abstract void EndPass();
		public abstract void End();

		public abstract void SetTexture(EffectTexture tex, string variableName);
		public abstract void SetVariable(string name, params float[] v);
		public abstract void SetVariable(string name, params int[] v);
		public abstract void SetVariable(string name, Matrix4x4 matrix);

		public void SetVariable(string name, Vector2 v)
		{
			SetVariable(name, v.X, v.Y);
		}
		public void SetVariable(string name, Vector3 v)
		{
			SetVariable(name, v.X, v.Y, v.Z);
		}
		public void SetVariable(string name, Vector4 v)
		{
			SetVariable(name, v.X, v.Y, v.Z, v.W);
		}
		public void SetVariable(string name, Color color)
		{
			SetVariable(name, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		//public VertexLayout VertexDefinition { get; set; }

		public abstract void Render<T>(RenderHandler<T> handler, T obj);
	}

	public enum EffectTexture
	{
		Texture0,
		Texture1,
		Texture2,
		Texture3,
	}

	public delegate void RenderHandler<in T>(T obj);
}
