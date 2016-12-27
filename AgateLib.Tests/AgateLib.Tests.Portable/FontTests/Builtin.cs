using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;

namespace AgateLib.Tests.FontTests
{
	class Builtin : Scene, IAgateTest
	{
		List<IFont> fonts = new List<IFont>();
		string nonenglish;

		public string Name
		{
			get { return "Built-in font tests"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}

		public AgateConfig Configuration { get; set; }

		protected override void OnSceneStart()
		{
			fonts.Add(Font.AgateMono);
			fonts.Add(Font.AgateSans);
			fonts.Add(Font.AgateSerif);

			for (char i = (char)128; i < 255; i++)
			{
				nonenglish += i;
			}
		}

		public override void Update(double deltaT)
		{
		}

		public override void Draw()
		{
			Display.Clear(Color.Black);

			int y = 0;
			foreach (var font in fonts)
			{
				font.Color = Color.White;

				font.DrawText(0, y, font.Name);
				int x = 20;
				y += font.FontHeight;

				font.DrawText(x, y, "01234567890!@#$%^&*()[]{}\\|/?'\";:.>,<`~-_=+");
				y += font.FontHeight;

				font.DrawText(x, y, "The quick brown fox jumped over the lazy dogs.");
				y += font.FontHeight;

				font.DrawText(x, y, "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS.");
				y += font.FontHeight;

				font.DrawText(x, y, nonenglish);
				y += font.FontHeight;

				y += font.FontHeight;
			}
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.DesiredDisplayWindowResolution = new Size(800, 600);
		}

		public void Run()
		{
			SceneStack.Start(this);
		}
	}
}
