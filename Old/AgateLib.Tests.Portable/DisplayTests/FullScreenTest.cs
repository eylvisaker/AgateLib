using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.DisplayTests
{
	class FullScreenTest : IAgateTest
	{
		private static readonly List<IResolution> resolutions = new List<IResolution>
		{
			new Resolution(640, 480, RenderMode.RetainAspectRatio),
			new Resolution(800, 600, RenderMode.RetainAspectRatio),
			new Resolution(1024, 768, RenderMode.RetainAspectRatio),
			new Resolution(1280, 720, RenderMode.RetainAspectRatio),
			new Resolution(1920, 1080, RenderMode.RetainAspectRatio),
			new Resolution(3840, 2160, RenderMode.RetainAspectRatio),
			new Resolution(640, 480, RenderMode.Stretch),
			new Resolution(800, 600, RenderMode.Stretch),
			new Resolution(1024, 768, RenderMode.Stretch),
			new Resolution(1280, 720, RenderMode.Stretch),
			new Resolution(1920, 1080, RenderMode.Stretch),
			new Resolution(3840, 2160, RenderMode.Stretch),
		};

		private string topText = @"Press Esc or Enter to exit.
Press arrow keys to adjust resolution
";
		private string bottomText = $@"Press F1-F12 to change resolution - Hold shift to use new DisplayWindow
    F1 - {resolutions[0 % resolutions.Count]}
    F2 - {resolutions[1 % resolutions.Count]}
    F3 - {resolutions[2 % resolutions.Count]}
    F4 - {resolutions[3 % resolutions.Count]}
    F5 - {resolutions[4 % resolutions.Count]}
    F6 - {resolutions[5 % resolutions.Count]}
    F7 - {resolutions[6 % resolutions.Count]}
    F8 - {resolutions[7 % resolutions.Count]}
    F9 - {resolutions[8 % resolutions.Count]}
    F10 - {resolutions[9 % resolutions.Count]}
    F11 - {resolutions[10 % resolutions.Count]}
    F12 - {resolutions[11 % resolutions.Count]}";

		private DisplayWindow wind;
		private Point mousePosition;
		private IResolution currentResolution;

		public string Name => "Full Screen";

		public string Category => "Display";

		public void Run(string[] args)
		{
			ChangeDisplayWindow(3);

			Surface mySurface = new Surface("Images/pointer.png");

			Input.Unhandled.KeyDown += Keyboard_KeyDown;
			Input.Unhandled.MouseMove += (sender, e) => mousePosition = e.MousePosition;

			IFont font = Font.AgateSans;

			Size bottomSize = font.MeasureString(bottomText);
			Size topSize = font.MeasureString(topText + "z\nz");

			// Run the program while the window is open.
			while (AgateApp.IsAlive)
			{
				var mouseText = topText +
								$"Resolution: {currentResolution}\nMouse: {mousePosition}";

				Display.BeginFrame();
				Display.Clear(Color.DarkGreen);

				font.DrawText(0, Display.CurrentWindow.Height - bottomSize.Height, bottomText);

				Display.Primitives.FillRect(Color.Maroon,
					new Rectangle(0, 0, Display.CurrentWindow.Width, topSize.Height));

				font.DrawText(mouseText);

				mySurface.Draw(mousePosition.X, mousePosition.Y);

				Display.EndFrame();
				AgateApp.KeepAlive();
			}

			mySurface.Dispose();
			wind.Dispose();
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			IResolution resolution;

			switch (e.KeyCode)
			{
				case KeyCode.Escape:
					AgateApp.IsAlive = false;
					break;

				case KeyCode.F1:
				case KeyCode.F2:
				case KeyCode.F3:
				case KeyCode.F4:
				case KeyCode.F5:
				case KeyCode.F6:
				case KeyCode.F7:
				case KeyCode.F8:
				case KeyCode.F9:
				case KeyCode.F10:
				case KeyCode.F11:
				case KeyCode.F12:
					var index = e.KeyCode - KeyCode.F1;

					if (e.KeyModifiers.Shift)
						ChangeDisplayWindow(index);
					else
						ChangeResolution(index);

					break;

				case KeyCode.Left:
					resolution = currentResolution.Clone(
						new Size(currentResolution.Width - 1, currentResolution.Height));
					ChangeResolution(resolution);
					break;

				case KeyCode.Right:
					resolution = currentResolution.Clone(
						new Size(currentResolution.Width + 1, currentResolution.Height));
					ChangeResolution(resolution);
					break;

				case KeyCode.Up:
					resolution = currentResolution.Clone(
						new Size(currentResolution.Width, currentResolution.Height - 1));
					ChangeResolution(resolution);
					break;

				case KeyCode.Down:
					resolution = currentResolution.Clone(
						new Size(currentResolution.Width, currentResolution.Height + 1));
					ChangeResolution(resolution);
					break;
			}
		}

		private void ChangeDisplayWindow(int index)
		{
			ChangeDisplayWindow(resolutions[index % resolutions.Count]);
		}

		private void ChangeDisplayWindow(IResolution resolution)
		{
			currentResolution = resolution;

			wind?.Dispose();

			wind = DisplayWindow.CreateFullScreen(Name, resolution);
			Display.RenderTarget = wind.FrameBuffer;
		}

		private void ChangeResolution(int index)
		{
			ChangeResolution(resolutions[index % resolutions.Count]);
		}

		private void ChangeResolution(IResolution resolution)
		{
			currentResolution = resolution;

			wind.Resolution = resolution;
		}
	}
}