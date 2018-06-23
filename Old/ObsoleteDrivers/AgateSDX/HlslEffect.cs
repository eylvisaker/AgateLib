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
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using Direct3D = SlimDX.Direct3D9;

namespace AgateSDX
{
	[Obsolete]
	class HlslEffect : Effect 
	{
		SDX_Display mDisplay;
		Direct3D.Effect mEffect;
		Dictionary<string, EffectParameter> mParams = new Dictionary<string, EffectParameter>();
		Dictionary<EffectTexture, string> mTextures = new Dictionary<EffectTexture, string>();

		public HlslEffect(Direct3D.Effect effect)
		{
			mDisplay = (SDX_Display)Display.Impl;
			mEffect = effect;
		}

		private EffectParameter GetParameter(string name)
		{
			if (mParams.ContainsKey(name))
				return mParams[name];

			var param = mEffect.GetParameter(null, name);
			EffectParameter p = new EffectParameter();
			p.Name = name;
			p.Handle = param;

			mParams[name] = p;

			return p;
		}

		public override void SetVariable(string name, Matrix4x4 matrix)
		{
			mEffect.SetValue(name, GeoHelper.TransformAgateMatrix(matrix));
		}
		public override void SetVariable(string name, params int[] v)
		{
			mEffect.SetValue(name, v);
		}
		public override void SetVariable(string name, params float[] v)
		{
			mEffect.SetValue(name, v);
		}

		public override void Render<T>(RenderHandler<T> handler, T obj)
		{
			Display.FlushDrawBuffer();

			mDisplay.Effect = this;

			int passcount = mEffect.Begin(SlimDX.Direct3D9.FX.None);

			for (int i = 0; i < passcount; i++)
			{
				mEffect.BeginPass(i);
				handler(obj);

				Display.FlushDrawBuffer();
				mEffect.EndPass();
			}

			mEffect.End();
		}
		public void InternalRender<T>(RenderHandler<T> handler, T obj)
		{
			mDisplay.Effect = this;

			int passcount = mEffect.Begin(SlimDX.Direct3D9.FX.None);

			for (int i = 0; i < passcount; i++)
			{
				mEffect.BeginPass(i);
				handler(obj);
				mEffect.EndPass();
			}

			mEffect.End();
		}
		public override string Technique
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				mEffect.Technique = value;
			}
		}

		public override int Passes
		{
			get { throw new NotImplementedException(); }
		}

		public override void Begin()
		{
			mDisplay.Effect = this;

			mEffect.Begin();
		}

		public override void BeginPass(int passIndex)
		{
			mEffect.BeginPass(passIndex);
		}

		public override void EndPass()
		{
			Display.FlushDrawBuffer();
			mEffect.EndPass();
		}

		public override void End()
		{
			mEffect.End();

			mDisplay.Effect = null;
		}

		public override void SetTexture(EffectTexture tex, string variableName)
		{
			if (string.IsNullOrEmpty(variableName))
			{
				mTextures.Remove(tex);
			}
			else
				mTextures[tex] = variableName;
		}
		public void SetTexture(EffectTexture tex, Direct3D.Texture surf)
		{
			string varName = mTextures[tex];

			mEffect.SetTexture(varName, surf);
		}
	}
}
