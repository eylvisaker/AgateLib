using AgateLib.Diagnostics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AgateLib.Tests.Console
{
    public class BoxVocabulary : Vocabulary
    {
        private List<Point> points;

        public override string Path => "";

        public override bool IsValid => true;

        public BoxVocabulary(List<Point> points)
        {
            this.points = points;
        }

        [ConsoleCommand("Say hello! You can also tell me your name with\n    hello myname")]
        private void Hello(string name = null)
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
        private void Add(int count = 250)
        {
            Random rnd = new Random();

            for (int i = 0; i < count; i++)
                points.Add(new Point(
                    rnd.Next(1280),
                    rnd.Next(720)));

            Shell.WriteLine($"Added {count} new boxes.");
        }

        [ConsoleCommand("Clears the boxes.")]
        private void Clear()
        {
            points.Clear();
        }

    }
}
