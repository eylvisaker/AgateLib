using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Tests.DisplayTests
{
	class CoordinateSystem : IAgateTest 
	{
		int ortho = 0;

		public string Name { get { return "Coordinate System"; } }

		public string Category { get { return "Display"; } }

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			Input.Unhandled.KeyDown += Keyboard_KeyDown;
			Input.Unhandled.MouseDown += Mouse_MouseDown;

			Surface surf = new Surface("jellybean.png");
			surf.Color = Color.Cyan;

			while (Display.CurrentWindow.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear();

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

				Display.FillRect(-2, -2, 4, 4, Color.Red);
				surf.Draw();

				Display.FlushDrawBuffer();
				AgateBuiltInShaders.Basic2DShader.CoordinateSystem =
					new Rectangle(Point.Empty, Display.CurrentWindow.Size);
				AgateBuiltInShaders.Basic2DShader.Activate();

				DefaultAssets.Fonts.AgateSans.DrawText("Press space to cycle through coordinate systems.");

				Display.EndFrame();

				Core.KeepAlive();
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

		public void ModifySetup(IAgateSetup setup)
		{
			setup.DesiredDisplayWindowResolution = new Size(800, 600);
		}
	}
}
