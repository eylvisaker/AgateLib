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
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// The effect class encapsulates all of the runtime details of a set of 
	/// vertex/pixel/geometry shaders.
	/// </summary>
	public abstract class Effect
	{
		/// <summary>
		/// Which technique should be used
		/// </summary>
		public abstract string Technique { get; set; }

		/// <summary>
		/// The number of passes in the effect.
		/// </summary>
		public abstract int Passes { get; }

		/// <summary>
		/// Begin rendering with this effect.
		/// </summary>
		public abstract void Begin();
		/// <summary>
		/// Begins the specified pass.
		/// </summary>
		/// <param name="passIndex"></param>
		public abstract void BeginPass(int passIndex);
		/// <summary>
		/// Ends the current pass.
		/// </summary>
		public abstract void EndPass();
		/// <summary>
		/// End rendering with this effect.
		/// </summary>
		public abstract void End();

		/// <summary>
		/// Sets a texture variable.
		/// </summary>
		/// <param name="tex"></param>
		/// <param name="variableName"></param>
		public abstract void SetTexture(EffectTexture tex, string variableName);
		/// <summary>
		/// Sets a uniform variable for the shader.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="v"></param>
		public abstract void SetVariable(string name, params float[] v);
		/// <summary>
		/// Sets a uniform variable for the shader.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="v"></param>
		public abstract void SetVariable(string name, params int[] v);
		/// <summary>
		/// Sets a uniform variable for the shader.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="v"></param>
		public abstract void SetVariable(string name, Matrix4x4 matrix);

		/// <summary>
		/// Sets a uniform variable for the shader.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="v"></param>
		public void SetVariable(string name, Vector2 v)
		{
			SetVariable(name, v.X, v.Y);
		}
		/// <summary>
		/// Sets a uniform variable for the shader.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="v"></param>
		public void SetVariable(string name, Vector3 v)
		{
			SetVariable(name, v.X, v.Y, v.Z);
		}
		/// <summary>
		/// Sets a uniform variable for the shader.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="v"></param>
		public void SetVariable(string name, Vector4 v)
		{
			SetVariable(name, v.X, v.Y, v.Z, v.W);
		}
		/// <summary>
		/// Sets a uniform variable for the shader.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="v"></param>
		public void SetVariable(string name, Color color)
		{
			SetVariable(name, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		//public VertexLayout VertexDefinition { get; set; }

		/// <summary>
		/// Renders the geometry using this effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="handler">A callback function which is called to actually render the geometry.</param>
		/// <param name="obj">An object which is passed to the callback function.</param>
		public abstract void Render<T>(RenderHandler<T> handler, T obj);
	}

	/// <summary>
	/// Which texture variable to set.
	/// </summary>
	public enum EffectTexture
	{
		Texture0,
		Texture1,
		Texture2,
		Texture3,
	}

	public delegate void RenderHandler<T>(T obj);
}
