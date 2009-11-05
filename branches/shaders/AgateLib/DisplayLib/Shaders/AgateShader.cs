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
	public class AgateShader
	{
		AgateShaderImpl impl;

		protected internal void SetImpl(AgateShaderImpl impl)
		{
			if (this.impl != null)
				throw new InvalidOperationException("Cannot set impl on an object which already has one.");

			this.impl = impl;
		}

		public int Passes
		{
			get { return impl.Passes; }
		}

		public void Begin()
		{
			impl.Begin();
		}
		public void BeginPass(int passIndex)
		{
			impl.BeginPass(passIndex);
		}
		public void EndPass()
		{
			impl.EndPass();
		}
		public void End()
		{
			impl.End();
		}

		public bool IsValid
		{
			get { return impl != null; }
		}

		public bool IsActive
		{
			get { return Display.Shader == this; }
		}

		public void Activate()
		{
			Display.Shader = this;
		}
		public void SetTexture(EffectTexture tex, string variableName)
		{
			impl.SetTexture(tex, variableName);
		}
		public void SetVariable(string name, params float[] v)
		{
			impl.SetVariable(name, v);
		}
		public void SetVariable(string name, params int[] v)
		{
			impl.SetVariable(name, v);
		}
		public void SetVariable(string name, Matrix4x4 matrix)
		{
			impl.SetVariable(name, matrix);
		}

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
			impl.SetVariable(name, color);
		}

	}

	public static class AgateBuiltInShaders
	{
		internal static void InitializeShaders()
		{
			if (Basic2DShader != null)
				throw new InvalidOperationException();

			Basic2DShader = new Basic2DShader();


			Basic2DShader.Activate();
		}
		internal static void DisposeShaders()
		{
			Basic2DShader = null;
		}

		public static Basic2DShader Basic2DShader { get; private set; }

	}
}
