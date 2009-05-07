using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace AgateMDX
{
	class HlslShaderProgram : ShaderProgram 
	{
		Direct3D.Effect mEffect;

		public HlslShaderProgram(Direct3D.Effect effect)
		{
			mEffect = effect;
		}
		public override PixelShader PixelShader
		{
			get { throw new NotImplementedException(); }
		}

		public override void SetUniform(string name, AgateLib.Geometry.Matrix4 matrix)
		{
			throw new NotImplementedException();
		}

		public override void SetUniform(string name, params int[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetUniform(string name, params float[] v)
		{
			throw new NotImplementedException();
		}

		public override VertexShader VertexShader
		{
			get { throw new NotImplementedException(); }
		}

		public override void Render(RenderHandler handler, object obj)
		{
			int passcount = mEffect.Begin(Microsoft.DirectX.Direct3D.FX.None);

			for (int i = 0; i < passcount; i++)
			{
				mEffect.BeginPass(i);
				handler(obj);
				mEffect.EndPass();
			}

			mEffect.End();
		}
	}

	class HlslPixelShader : PixelShader
	{
	}

	class HlslVertexShader : VertexShader
	{ }
}
