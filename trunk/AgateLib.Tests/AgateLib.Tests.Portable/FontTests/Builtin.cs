using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.FontTests
{
	class Builtin : Scene, ISceneModelTest
	{
		List<FontSurface> fonts = new List<FontSurface>();
		string nonenglish;

		public string Name
		{
			get { return "Built-in font tests"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}

		protected override void OnSceneStart()
		{
			//fonts.Add(BuiltinResources.AgateSans10);
			//fonts.Add(BuiltinResources.AgateSans14);
			//fonts.Add(BuiltinResources.AgateSerif10);
			//fonts.Add(BuiltinResources.AgateSerif14);
			//fonts.Add(BuiltinResources.AgateMono10);

			for (char i = (char)128; i < 255; i++)
			{
				nonenglish += i;
			}
		}

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public Scene StartScene
		{
			get { return this; }
		}

		public override void Update(double delta_t)
		{
		}

		public override void Draw()
		{
			Display.Clear(Color.Black);

			int y = 0;
			foreach (var font in fonts)
			{
				font.Color = Color.White;

				font.DrawText(0, y, font.FontName);
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
	}
}
