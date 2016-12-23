using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.FontTests.TextLayout
{
	class TextLayout : Scene, ISceneModelTest
	{
		Surface image;

		protected override void OnSceneStart()
		{
			image = new Surface("9ball.png");
			image.SetScale(0.5, 0.5);
		}

		public override void Draw()
		{
			Display.Clear(Color.White);

			IFont font = AgateLib.DefaultAssets.Fonts.AgateSans;
			font.Color = Color.Black;

			font.TextImageLayout = TextImageLayout.InlineTop;
			font.DrawText(0, 0, "Test InlineTop:\n{0}Test Layout {0} Text\nTest second line.",
				image);

			font.TextImageLayout = TextImageLayout.InlineCenter;
			font.DrawText(0, 150, "Test InlineCenter:\n{0}Test Layout {0} Text\nTest second line.",
				image);

			font.TextImageLayout = TextImageLayout.InlineBottom;
			font.DrawText(0, 300, "Test InlineBottom:\n{0}Test Layout {0} Text\nTest second line.",
				image);

			font.DrawText(0, 450, "This is a test of the {0}AlterText{1} stuff." +
				"The last word here should appear really {2}Large{3}.",
				LayoutCacheAlterFont.Color(Color.Green), LayoutCacheAlterFont.Color(Color.Black),
				LayoutCacheAlterFont.Scale(3.0, 3.0), LayoutCacheAlterFont.Scale(1.0, 1.0));

			font.DrawText(0, 530, "Test of escape sequences: {{}Escaped{}}");
		}


		public string Name
		{
			get { return "Text Layout Test"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}


		public override void Update(double deltaT)
		{
		}

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public Scene StartScene
		{
			get { return this; }
		}
	}
}
