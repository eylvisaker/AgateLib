using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Resources.Legacy;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Testing.FontTests
{
	class FontAlignment : ISerialModelTest
	{
		public string Name
		{
			get { return "Font Alignment"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}

		int fontIndex;


		public void EntryPoint()
		{
			AgateResourceCollection resources = new AgateResourceCollection("TestResourceFile.xml");

			List<FontSurface> fonts = new List<FontSurface>();

			fonts.Add(new FontSurface(resources, "MedievalSharp14"));
			fonts.Add(new FontSurface(resources, "MedievalSharpBold14"));
			fonts.Add(new FontSurface(resources, "MedievalSharp18"));
			fonts.Add(new FontSurface(resources, "MedievalSharpBold18"));
			fonts.Add(new FontSurface(resources, "MedievalSharpBold22"));

			Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

			int[] numbers = new int[] { 0, 0, 1, 11, 22, 33, 44, 99, 100, 111, 222, 333, 444, 555, 666, 777, 888, 999 };

			while (Display.CurrentWindow.IsClosed == false)
			{
				FontSurface f = fonts[fontIndex];

				Display.BeginFrame();
				Display.Clear(Color.Black);
				Display.FillRect(new Rectangle(0, 0, 300, 600), Color.DarkSlateGray);

				f.Color = Color.White;
				f.DisplayAlignment = OriginAlignment.TopLeft;
				f.DrawText("Left-aligned numbers");

				for (int i = 1; i < numbers.Length; i++)
				{
					f.DrawText(0, i * f.FontHeight, numbers[i].ToString());
				}

				f.DrawText(300, 0, "Right-aligned numbers");
				f.DisplayAlignment = OriginAlignment.TopRight;

				for (int i = 1; i < numbers.Length; i++)
				{
					f.DrawText(300.0, i * f.FontHeight, numbers[i].ToString());
				}


				Display.EndFrame();
				Core.KeepAlive();

				if (fontIndex >= fonts.Count)
					fontIndex = fonts.Count - 1;
			}
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.NumPadPlus)
				fontIndex++;
			else if (e.KeyCode == KeyCode.NumPadMinus)
			{
				fontIndex--;
				if (fontIndex < 0)
					fontIndex = 0;
			}
		}

		public void ModifyModelParameters(ApplicationModels.SerialModelParameters parameters)
		{
		}

	}
}
