using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateDrawing
{
	class DrawingBasic2DShader : AgateShaderImpl 
	{
		public override void SetTexture(AgateLib.DisplayLib.Shaders.EffectTexture tex, string variableName)
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

		public override void SetVariable(string name, AgateLib.Geometry.Matrix4x4 matrix)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override int Passes
		{
			get { return 1; }
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
