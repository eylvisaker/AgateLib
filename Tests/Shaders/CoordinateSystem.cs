using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.CoordinateSystemTest
{
	class CoordinateSystem : IAgateTest 
	{
		int ortho = 0;
		DisplayWindow wind;

		#region IAgateTest Members

		public string Name { get { return "Coordinate System"; } }
		public string Category { get { return "Shaders"; } }

		#endregion

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

				wind = DisplayWindow.CreateWindowed("Ortho Projection Test", 640, 480, false);

				Surface surf = new Surface("jellybean.png");
				surf.Color = Color.Cyan;

				while (wind.IsClosed == false)
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
			});
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				ortho++;
				if (ortho > 2)
					ortho = 0;

				Keyboard.ReleaseKey(KeyCode.Space);
			}
			else if (e.KeyCode == KeyCode.Escape)
				wind.Dispose();

		}
	}
}
