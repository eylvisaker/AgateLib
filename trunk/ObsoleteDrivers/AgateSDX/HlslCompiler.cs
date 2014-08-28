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
