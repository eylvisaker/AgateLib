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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.ImplementationBase;

namespace AgateLib.DisplayLib.Shaders
{
	[Obsolete]
	public static class ShaderCompiler
	{
		static ShaderCompilerImpl impl;

		internal static void Initialize(ShaderCompilerImpl shaderCompilerImpl)
		{
			impl = shaderCompilerImpl;

			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Display_DisposeDisplay);
		}

		static void Display_DisposeDisplay()
		{
			impl = null;
		}

		internal static void Disable()
		{
			impl = null;
		}

		public static Effect CompileEffect(ShaderLanguage language, string effectSource)
		{
			return impl.CompileEffect(language, effectSource);
		}
	}
}
