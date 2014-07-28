using System;
namespace AgateLib.Platform.WindowsForms.DisplayImplementation
{
	interface IPrimaryWindow
	{
		void RunApplication();

		void ReinitializeFramebuffer();
	}
}
