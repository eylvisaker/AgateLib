using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;

namespace AgateLib.Tests.FontTests.TextLayout
{
	class TextLayout : Scene, IAgateTest
	{
		Surface image;
		public string Name
		{
			get { return "Text Layout Test"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}

		public AgateConfig Configuration { get; set; }

		protected override void OnSceneStart()
		{
			image = new Surface("9ball.png");
			image.SetScale(0.5, 0.5);
		}

		public override void Draw()
		{
			Display.Clear(Color.White);

			IFont font = Font.AgateSans;
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

		public override void Update(double deltaT)
		{
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
