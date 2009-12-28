using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.ImplementationBase;
using Direct3D = SlimDX.Direct3D9;

namespace AgateSDX
{
	[Obsolete]
	class HlslCompiler : ShaderCompilerImpl 
	{
		SDX_Display mDisplay;

		public HlslCompiler(SDX_Display display)
		{
			mDisplay = display;
		}

		public override Effect CompileEffect(ShaderLanguage language, string effectSource)
		{
			string tempFile = Path.GetTempFileName();

			using (var stream = new StreamWriter(tempFile))
			{
				stream.WriteLine(effectSource);
			}

			string compilationErrors = "";

			try
			{
				var effect = Direct3D.Effect.FromFile(mDisplay.D3D_Device.Device,
					tempFile, null, null, null, SlimDX.Direct3D9.ShaderFlags.Debug, null, out compilationErrors);
				
				return new HlslEffect(effect);
			}
			catch (Direct3D.Direct3D9Exception e)
			{
				throw new AgateShaderCompilerException(compilationErrors, e);
			}

		}
	}
}
