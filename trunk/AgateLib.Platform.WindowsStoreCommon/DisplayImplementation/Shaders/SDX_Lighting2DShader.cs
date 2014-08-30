using AgateLib.DisplayLib.Shaders.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsStore.DisplayImplementation.Shaders
{
	class SDX_Lighting2DShader : AgateShaderImpl
	{
		public override void SetTexture(DisplayLib.Shaders.EffectTexture tex, string variableName)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params float[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params int[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, Geometry.Matrix4x4 matrix)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override int Passes
		{
			get { throw new NotImplementedException(); }
		}

		public override void Begin()
		{
			throw new NotImplementedException();
		}

		public override void BeginPass(int passIndex)
		{
			throw new NotImplementedException();
		}

		public override void EndPass()
		{
			throw new NotImplementedException();
		}

		public override void End()
		{
			throw new NotImplementedException();
		}
	}
}
