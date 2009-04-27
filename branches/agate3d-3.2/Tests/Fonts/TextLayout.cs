using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace Tests.Fonts.TextLayout
{
	class TextLayout : AgateApplication, IAgateTest 
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateFileProvider.Assemblies.AddPath("../Drivers");
			AgateFileProvider.Images.AddPath("Images");

			new TextLayout().Run(args);
		}

		FontSurface font;
		Surface image;

		protected override void Initialize()
		{
			font = new FontSurface("Arial", 10);
			font.Color = Color.Black;

			image = new Surface("9ball.png");
			image.SetScale(0.5, 0.5);
		}

		protected override void  Render()
		{
			Display.Clear(Color.White);

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
				AlterFont.Color(Color.Green), AlterFont.Color(Color.Black),
				AlterFont.Scale(3.0, 3.0), AlterFont.Scale(1.0, 1.0));

			font.DrawText(0, 530, "Test of escape sequences: {{}Escaped{}}");
		}

		#region IAgateTest Members

		public string Name
		{
			get { return "Text Layout Test"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}

		void IAgateTest.Main(string[] args)
		{
			Run(args);
		}

		#endregion
	}
}
