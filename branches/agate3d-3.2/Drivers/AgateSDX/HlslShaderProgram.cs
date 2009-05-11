using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using Direct3D = SlimDX.Direct3D9;

namespace AgateSDX
{
	class HlslShaderProgram : ShaderProgram 
	{
		Direct3D.VertexShader mVertexShader;
		Direct3D.PixelShader mPixelShader;
		SDX_Display mDisplay;

		public HlslShaderProgram(Direct3D.VertexShader vert, Direct3D.PixelShader pix)
		{
			mDisplay = (SDX_Display)Display.Impl;

			mVertexShader = vert;
			mPixelShader = pix;
		}
		public override PixelShader PixelShader
		{
			get { throw new NotImplementedException(); }
		}

		public override void SetUniform(string name, AgateLib.Geometry.Matrix4 matrix)
		{
			
		}

		public override void SetUniform(string name, params int[] v)
		{
			
		}

		public override void SetUniform(string name, params float[] v)
		{
			
		}

		public override VertexShader VertexShader
		{
			get { throw new NotImplementedException(); }
		}

		public Direct3D.VertexShader HlslVertexShader
		{
			get { return mVertexShader; }
		}
		public Direct3D.PixelShader HlslPixelShader
		{
			get { return mPixelShader; }
		}

		public override void Render(RenderHandler handler, object obj)
		{
			mDisplay.D3D_Device.Device.VertexShader = mVertexShader;
			mDisplay.D3D_Device.Device.PixelShader = mPixelShader;

			handler(obj);
			//int passcount = mEffect.Begin(SlimDX.Direct3D9.FX.None);

			//for (int i = 0; i < passcount; i++)
			//{
			//    mEffect.BeginPass(i);
			//    handler(obj);
			//    mEffect.EndPass();
			//}

			//mEffect.End();
		}
	}

	class HlslPixelShader : PixelShader
	{
	}

	class HlslVertexShader : VertexShader
	{ }
}
