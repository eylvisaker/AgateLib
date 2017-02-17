using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Resources;

namespace AgateLib.Tests.FontTests
{
	class FontAlignment : IAgateTest
	{
		int fontIndex;

		public string Name => "Font Alignment";

		public string Category => "Fonts";

		public Font font { get; set; }

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				var resources = new AgateResourceManager("UserInterface/FontAlignment.yaml");
				resources.InitializeContainer(this);

				var fonts = new List<IFont> { Font.AgateSans, Font.AgateSerif, Font.AgateMono, };

				Input.Unhandled.KeyDown += Keyboard_KeyDown;

				int[] numbers = new int[] { 0, 0, 1, 11, 22, 33, 44, 99, 100, 111, 222, 333, 444, 555, 666, 777, 888, 999 };

				while (AgateApp.IsAlive)
				{
					IFont f = fonts[fontIndex];

					Display.BeginFrame();
					Display.Clear(Color.Black);

					var firstLineFont = fonts.First();
					var firstLineHeight = firstLineFont.FontHeight;

					Display.Primitives.FillRect(Color.DarkSlateGray, new Rectangle(0, firstLineHeight, 300, 600));
					Display.Primitives.FillRect(Color.DarkBlue, new Rectangle(300, firstLineHeight, 300, 600));

					firstLineFont.DisplayAlignment = OriginAlignment.TopLeft;
					firstLineFont.Color = Color.White;
					firstLineFont.DrawText(0, 0, "Press space to cycle fonts.");

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
					AgateApp.KeepAlive();

					if (fontIndex >= fonts.Count)
						fontIndex = 0;
				}
			}
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
				fontIndex++;

			Input.Unhandled.Keys.Release(KeyCode.Space);
		}
	}
}
