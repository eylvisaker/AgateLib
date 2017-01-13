using System;
using System.Drawing;

namespace Examples.Launcher
{
	public interface ILauncherView
	{
		event EventHandler<ExampleEventArgs> LaunchExample;
		event EventHandler<ExampleEventArgs> SelectedExampleChanged;

		ExampleCategories Categories { get; set; }

		Image Image { get; set; }
	}
}