using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform;
using AgateLib.Platform.WinForms;

namespace RigidBodyDynamics.Demo
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (new AgateWinForms(args)
				.Initialize()
				.InstallConsoleCommands()
				.AssetPath("Assets"))
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(1024, 768)
				.QuitOnClose()
				.Build())
			{
				var game = new Game();

				Input.Unhandled.KeyDown += Unhandled_KeyDown;
				while (AgateApp.IsAlive)
				{
					game.Update(AgateApp.GameClock.Elapsed);

					Display.BeginFrame();
					Display.Clear(Color.Gray);
					game.Draw();

					Display.EndFrame();
					AgateApp.KeepAlive();
				}
			}
		}

		private static void Unhandled_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Escape)
				AgateApp.IsAlive = false;
		}
	}
}
