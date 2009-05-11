using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.ImplementationBase;
using Direct3D = SlimDX.Direct3D9;

namespace AgateMDX
{
	class HlslCompiler : ShaderCompilerImpl 
	{
		SDX_Display mDisplay;

		public HlslCompiler(SDX_Display display)
		{
			mDisplay = display;
		}

		public override ShaderProgram CompileEffect(ShaderLanguage language, string effectSource)
		{
			throw new NotImplementedException();
		}
		//public override ShaderProgram CompileEffect(ShaderLanguage language, string effectSource)
		//{
		//    Direct3D.Effect effect = Direct3D.Effect.FromString(mDisplay.D3D_Device.Device,
		//        effectSource, null, null, Direct3D.ShaderFlags.None, null);

		//    return new HlslShaderProgram(effect);
		//}
		public override ShaderProgram CompileShader(ShaderLanguage language, string vertexShaderSource, string pixelShaderSource)
		{
			throw new NotImplementedException();

			//var vertexShaderStream = Direct3D.ShaderLoader.CompileShader(
			//    vertexShaderSource, "main", null, "vs_1_1", Direct3D.ShaderFlags.None);

			
			//var vertexShader = new Direct3D.VertexShader(mDisplay.D3D_Device.Device, vertexShaderStream);


			//var pixelShaderStream = Direct3D.ShaderLoader.CompileShader(
			//    pixelShaderSource, "main", null, "ps_1_1", Direct3D.ShaderFlags.None);

			//var pixelShader = new Direct3D.PixelShader(mDisplay.D3D_Device.Device, pixelShaderStream);

			//return new HlslShaderProgram(vertexShader, pixelShader);
		}
	}
}
