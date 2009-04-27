using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.OrthoProjectionTest
{
	class Ortho
	{
		int ortho = 0;
		DisplayWindow wind;


		#region IAgateTest Members

		public string Name { get { return "Ortho Projection"; } }
		public string Category { get { return "Display"; } }

		#endregion

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup())
			{
				setup.AskUser = true;
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

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
							Display.SetOrthoProjection(0, 0, surf.SurfaceWidth * 2, surf.SurfaceHeight * 2);
							break;

						case 2:
							Display.SetOrthoProjection(-surf.SurfaceWidth, -surf.SurfaceHeight,
								surf.SurfaceWidth, surf.SurfaceHeight);
							break;
					}

					Display.FillRect(-2, -2, 4, 4, Color.Red);

					surf.Draw();

					Display.EndFrame();

					Core.KeepAlive();
				}
			}
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
