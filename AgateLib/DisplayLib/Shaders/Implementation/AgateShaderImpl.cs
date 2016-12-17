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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
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
	/// Base class for implementing an AgateShader object.
	/// </summary>
	public abstract class AgateShaderImpl
	{
		public abstract void SetVariable(string name, params float[] v);
		public abstract void SetVariable(string name, params int[] v);
		public abstract void SetVariable(string name, Matrix4x4 matrix);
		public abstract void SetVariable(string name, Color color);

		public abstract int Passes { get; }

		public abstract void Begin();
		public abstract void BeginPass(int passIndex);
		public abstract void EndPass();
		public abstract void End();
		
	}
}
