using System;
using System.Collections.Generic;
using AgateLib.Diagnostics;
using Microsoft.Xna.Framework;

namespace AgateLib.Tests.Console
{
	class BoxVocabulary : IVocabulary
	{
		private List<Point> points;

		public string Namespace => "";

		public IConsoleShell Shell { get; set; }

		public BoxVocabulary(List<Point> points)
		{
			this.points = points;
		}

		[ConsoleCommand("Say hello! You can also tell me your name with\n    hello myname")]
		void Hello(string name = null)
		{
			if (string.IsNullOrEmpty(name))
			{
				Shell.WriteLine("Hello, anonymous user.");
			}
			else
			{
				Shell.WriteLine($"Hello, {name}!");
			}
		}

		[ConsoleCommand("Adds several boxes to the screen. Try it and see!")]
		void Add(int count = 250)
		{
			Random rnd = new Random();

			for (int i = 0; i < count; i++)
				points.Add(new Point(
					rnd.Next(1280),
					rnd.Next(720)));

			Shell.WriteLine($"Added {count} new boxes.");
		}

		[ConsoleCommand("Clears the boxes.")]
		void Clear()
		{
			points.Clear();
		}

	}
}
