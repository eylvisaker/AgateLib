using System;
using System.Drawing;

namespace Examples.Launcher
{
	public interface ILauncherView
	{
		event EventHandler<ExampleEventArgs> LaunchExample;
		event EventHandler<ExampleEventArgs> SelectedExampleChanged;

		string Arguments { get; }

		ExampleCategories Categories { get; set; }

		Image Image { get; set; }

		void Show();
		void Hide();
	}
}