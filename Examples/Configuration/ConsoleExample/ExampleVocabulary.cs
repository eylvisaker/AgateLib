using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.Mathematics.Geometry;

namespace Examples.Configuration.ConsoleExample
{
	class ExampleVocabulary : IVocabulary
	{
		private List<Point> points;

		public string Namespace => "";

		public ExampleVocabulary(List<Point> points)
		{
			this.points = points;
		}

		[ConsoleCommand("Say hello! You can also tell me your name with\n    hello myname")]
		void Hello(string name = null)
		{
			if (string.IsNullOrEmpty(name))
			{
				Log.WriteLine("Hello, anonymous user.");
			}
			else
			{
				Log.WriteLine($"Hello, {name}!");
			}
		}

		[ConsoleCommand("Adds several boxes to the screen. Try it and see!")]
		void Add(int count = 250)
		{
			Random rnd = new Random();

			for(int i = 0; i< count; i++)
				points.Add(new Point(
					rnd.Next(1280),
					rnd.Next(720)));

			Log.WriteLine($"Added {count} new boxes.");
		}

		[ConsoleCommand("Clears the boxes.")]
		void Clear()
		{
			points.Clear();
		}

	}
}
