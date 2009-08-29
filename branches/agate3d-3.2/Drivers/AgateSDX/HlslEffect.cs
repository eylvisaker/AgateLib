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
	class HlslEffect : Effect 
	{
		SDX_Display mDisplay;
		Direct3D.Effect mEffect;
		Dictionary<string, EffectParameter> mParams = new Dictionary<string, EffectParameter>();

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
			mEffect.SetValue(name, mDisplay.TransformAgateMatrix(matrix));
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
			mEffect.Begin();
		}

		public override void BeginPass(int passIndex)
		{
			mEffect.BeginPass(passIndex);
		}

		public override void EndPass()
		{
			mEffect.EndPass();
		}

		public override void End()
		{
			mEffect.End();
		}
	}
}
