using System;
namespace AgateLib.Platform.WindowsForms.DisplayImplementation
{
	interface IPrimaryWindow
	{
		void RunApplication();

		void ExitMessageLoop();

		void CreateContextForThread();
	}
}
