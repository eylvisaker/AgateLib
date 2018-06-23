using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.DisplayTests
{
	class CoordinateSystem : IAgateTest
	{
		int ortho = 0;

		public string Name => "Coordinate System";

		public string Category => "Display";

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				Input.Unhandled.KeyDown += Keyboard_KeyDown;
				Input.Unhandled.MouseDown += Mouse_MouseDown;

				Surface surf = new Surface("Images/jellybean.png") {Color = Color.Cyan};

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.DarkGreen);

					switch (ortho)
					{
						case 1:
							AgateBuiltInShaders.Basic2DShader.CoordinateSystem = new Rectangle
								(0, 0, surf.SurfaceWidth * 2, surf.SurfaceHeight * 2);
							break;

						case 2:
							AgateBuiltInShaders.Basic2DShader.CoordinateSystem = new Rectangle
								(-surf.SurfaceWidth, -surf.SurfaceHeight, surf.SurfaceWidth * 2, surf.SurfaceHeight * 2);
							break;
					}

					AgateBuiltInShaders.Basic2DShader.Activate();

					Display.Primitives.FillRect(Color.Red, new Rectangle(-2, -2, 4, 4));
					surf.Draw();

					Display.FlushDrawBuffer();
					AgateBuiltInShaders.Basic2DShader.CoordinateSystem =
						new Rectangle(Point.Zero, Display.CurrentWindow.Size);
					AgateBuiltInShaders.Basic2DShader.Activate();

					Font.AgateSans.DrawText("Press space to cycle through coordinate systems.");

					Display.EndFrame();

					AgateApp.KeepAlive();
				}
			}
		}

		void Mouse_MouseDown(object sender, AgateInputEventArgs e)
		{
			IncrementCounter();
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				IncrementCounter();

				Input.Unhandled.Keys.Release(KeyCode.Space);
			}
			else if (e.KeyCode == KeyCode.Escape)
				Display.CurrentWindow.Dispose();
		}

		private void IncrementCounter()
		{
			ortho++;
			if (ortho > 2)
				ortho = 0;
		}
	}
}
