using AgateLib.Drivers;
using AgateOTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Desktop.Forms
{
	public static class Setup
	{
		public static void Initialize()
		{
			TypeRegistry.DisplayDriver = typeof(GL_Display);
		}
	}
}
