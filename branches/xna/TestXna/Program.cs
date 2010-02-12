using System;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace TestXna
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				var wind = DisplayWindow.CreateWindowed("Hello", 800, 600);

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.Red);

					Display.EndFrame();
					Core.KeepAlive();

				}
			}
		}
	}
}

