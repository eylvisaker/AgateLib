using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateOTK.Shaders.FixedFunction
{
	class OTK_FF_Basic2DShader : AgateShaderImpl 
	{
		public override void Begin()
		{
		}

		public override void BeginPass(int passIndex)
		{
		}

		public override void End()
		{
		}

		public override void EndPass()
		{
		}

		public override int Passes
		{
			get { return 1; }
		}

		public override void SetTexture(EffectTexture tex, string variableName)
		{
		}

		public override void SetVariable(string name, AgateLib.Geometry.Color color)
		{
		}

		public override void SetVariable(string name, AgateLib.Geometry.Matrix4x4 matrix)
		{
		}

		public override void SetVariable(string name, params int[] v)
		{
		}

		public override void SetVariable(string name, params float[] v)
		{
		}
	}
}
