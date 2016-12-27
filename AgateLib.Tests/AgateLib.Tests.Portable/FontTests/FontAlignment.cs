using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;
using AgateLib.Resources;

namespace AgateLib.Tests.FontTests
{
	class FontAlignment : IAgateTest
	{
		int fontIndex;

		public string Name
		{
			get { return "Font Alignment"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}

		public Font font { get; set; }

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			var resources = new AgateResourceManager("FontAlignment.yaml");
			resources.InitializeContainer(this);

			var fonts = new List<IFont> { Font.AgateSans, Font.AgateSerif, Font.AgateMono, };

			Input.Unhandled.KeyDown += Keyboard_KeyDown;

			int[] numbers = new int[] { 0, 0, 1, 11, 22, 33, 44, 99, 100, 111, 222, 333, 444, 555, 666, 777, 888, 999 };

			while (Display.CurrentWindow.IsClosed == false)
			{
				IFont f = fonts[fontIndex];

				Display.BeginFrame();
				Display.Clear(Color.Black);

				var firstLineFont = fonts.First();
				var firstLineHeight = firstLineFont.FontHeight;

				Display.FillRect(new Rectangle(0, firstLineHeight, 300, 600), Color.DarkSlateGray);
				Display.FillRect(new Rectangle(300, firstLineHeight, 300, 600), Color.DarkBlue);

				firstLineFont.DisplayAlignment = OriginAlignment.TopLeft;
				firstLineFont.Color = Color.White;
				firstLineFont.DrawText(0, 0 ,"Press space to cycle fonts.");
				
				f.Color = Color.White;
				f.DisplayAlignment = OriginAlignment.TopLeft;
				f.DrawText(0, firstLineHeight, "Left-aligned numbers");

				for (int i = 1; i < numbers.Length; i++)
				{
					f.DrawText(0, firstLineHeight + i * f.FontHeight, numbers[i].ToString());
				}

				f.DisplayAlignment = OriginAlignment.TopRight;
				f.DrawText(600, firstLineHeight, "Right-aligned numbers");

				for (int i = 1; i < numbers.Length; i++)
				{
					f.DrawText(600.0, firstLineHeight + i * f.FontHeight, numbers[i].ToString());
				}

				Display.EndFrame();
				Core.KeepAlive();

				if (fontIndex >= fonts.Count)
					fontIndex = 0;
			}
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
				fontIndex++;

			Input.Unhandled.Keys.Release(KeyCode.Space);
		}

		public void ModifySetup(IAgateSetup setup)
		{
		}
	}
}
