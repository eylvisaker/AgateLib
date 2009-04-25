using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace TextLayout
{
	class TextLayout : AgateApplication
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

		protected override void Initialize()
		{
			font = new FontSurface("Arial", 10);
			font.Color = Color.Black;
		}

		protected override void Render(double time_ms)
		{
			Display.Clear(Color.White);

			font.TextImageLayout = TextImageLayout.InlineTop;
			font.DrawText(0, 0, "Test InlineTop:\n{0}Test Layout {0} Text\nTest second line.",
				AgateLib.InternalResources.Data.PoweredBy);

			font.TextImageLayout = TextImageLayout.InlineCenter;
			font.DrawText(0, 150, "Test InlineCenter:\n{0}Test Layout {0} Text\nTest second line.",
				AgateLib.InternalResources.Data.PoweredBy);

			font.TextImageLayout = TextImageLayout.InlineBottom;
			font.DrawText(0, 300, "Test InlineBottom:\n{0}Test Layout {0} Text\nTest second line.",
				AgateLib.InternalResources.Data.PoweredBy);

			font.DrawText(0, 450, "This is a test of the {0}AlterText{1} stuff." +
				"The last word here should appear really {2}Large{3}.",
				AlterFont.Color(Color.Green), AlterFont.Color(Color.Black),
				AlterFont.Scale(3.0, 3.0), AlterFont.Scale(1.0, 1.0));
		}
	}
}
