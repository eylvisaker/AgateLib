using System;
namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	interface IPrimaryWindow
	{
		void RunApplication();

		void ExitMessageLoop();

		void CreateContextForThread();
	}
}
