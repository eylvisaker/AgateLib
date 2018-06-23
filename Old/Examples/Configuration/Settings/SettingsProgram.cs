using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.WinForms;

namespace Examples.Configuration.Settings
{
	static class SettingsProgram
	{
		class ColorSetting
		{
			public double Hue { get; set; }
			public double Saturation { get; set; } = 1;
			public double Value { get; set; } = 1;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (new AgateWinForms(args).Initialize())
			using (new DisplayWindowBuilder(args)
				.Title("AgateLib Settings Storage")
				.BackbufferSize(500, 400)
				.QuitOnClose()
				.Build())
			{
				Input.Unhandled.KeyDown += InputKeyDown;

				var font = new Font(Font.AgateSans)
				{
					Size = 12,
					Style = FontStyles.Bold,
				};

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.Gray);

					font.DrawText(0, 0,
						"Use +/- keys to adjust hue.\nUse < and > to adjust saturation\nThese settings will persist \neach time this example is run.");

					// Read the ColorSetting object from the configuration, or create it if it does not exist.
					var setting = AgateApp.Settings.GetOrCreate<ColorSetting>("color", () => new ColorSetting());

					Point point = new Point(10, 300);
					Size size = new Size(480, 90);

					Rectangle dest = new Rectangle(point, size);
					Display.Primitives.FillRect(Color.FromHsv(setting.Hue, setting.Saturation, setting.Value), dest);

					Display.EndFrame();

					AgateApp.KeepAlive();
				}

				AgateApp.Settings.Save();
			}
		}

		private static void InputKeyDown(object sender, AgateInputEventArgs agateInputEventArgs)
		{
			// The get method will throw an exception if there is no item in the settings data named "color".
			var setting = AgateApp.Settings.Get<ColorSetting>("color");

			switch (agateInputEventArgs.KeyCode)
			{
				case KeyCode.Escape:
					AgateApp.IsAlive = false;
					break;

				case KeyCode.Minus:
				case KeyCode.NumPadMinus:
					setting.Hue -= 10;
					break;

				case KeyCode.Plus:
				case KeyCode.NumPadPlus:
					setting.Hue += 10;
					break;

				case KeyCode.Period:
					setting.Saturation += 0.1;
					if (setting.Saturation > 1)
						setting.Saturation = 1;
					break;

				case KeyCode.Comma:
					setting.Saturation -= 0.1;
					if (setting.Saturation < 0)
						setting.Saturation = 0;
					break;
			}
		}
	}
}
