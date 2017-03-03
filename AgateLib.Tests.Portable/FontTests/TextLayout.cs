using System;
using AgateLib.DisplayLib;
using AgateLib.Platform;

namespace AgateLib.Tests.FontTests
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

		protected override void OnSceneStart()
		{
			image = new Surface("9ball.png");
			image.SetScale(0.5, 0.5);
		}

		protected override void OnRedraw()
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

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
		}

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}
	}
}
