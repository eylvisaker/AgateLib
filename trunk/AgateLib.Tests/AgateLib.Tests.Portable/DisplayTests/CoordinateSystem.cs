using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Testing.DisplayTests
{
	class CoordinateSystem : ISerialModelTest 
	{
		int ortho = 0;

		public string Name { get { return "Coordinate System"; } }
		public string Category { get { return "Shaders"; } }

		public void EntryPoint()
		{
			Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
			Mouse.MouseDown += Mouse_MouseDown;
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

				Display.EndFrame();

				Core.KeepAlive();
			}
		}

		void Mouse_MouseDown(InputEventArgs e)
		{
			IncrementCounter();
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				IncrementCounter();

				Keyboard.ReleaseKey(KeyCode.Space);
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

		public void ModifyModelParameters(ApplicationModels.SerialModelParameters parameters)
		{
		}
	}
}
