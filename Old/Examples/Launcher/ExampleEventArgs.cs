using System;

namespace Examples.Launcher
{
	public class ExampleEventArgs : EventArgs
	{
		private ExampleItem example;

		public ExampleEventArgs(ExampleItem example)
		{
			this.example = example;
		}

		public ExampleItem Example => example;
	}
}