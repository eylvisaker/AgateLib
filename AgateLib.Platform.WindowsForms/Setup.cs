using AgateLib.Drivers;
using AgateLib.Platform.WindowsForms.DisplayImplementation;
using AgateLib.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms
{
	public static class Setup
	{
		public static void Initialize()
		{
			TypeRegistry.DisplayDriver = typeof(GL_Display);
		}
	}
}
