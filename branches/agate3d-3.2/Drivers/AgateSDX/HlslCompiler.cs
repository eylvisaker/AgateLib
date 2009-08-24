using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.ImplementationBase;
using Direct3D = SlimDX.Direct3D9;

namespace AgateSDX
{
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

			var effect = Direct3D.Effect.FromFile(mDisplay.D3D_Device.Device,
				tempFile, SlimDX.Direct3D9.ShaderFlags.Debug);

			return new HlslEffect(effect);
		}
	}
}
