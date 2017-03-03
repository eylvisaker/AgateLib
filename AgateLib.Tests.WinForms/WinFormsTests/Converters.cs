using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Platform;
using AgateLib.Platform.WinForms;
using AgateLib.Tests;

namespace AgateLib.Tests.WinFormsTests
{
	class Converters : Scene, IAgateTest 
	{
		public string Name => "Conversion Tests";

		public string Category => "WinForms";

		protected override void OnUpdate(ClockTimeSpan args)
		{
		}

		protected override void OnRedraw()
		{
			IsFinished = true;
		}

		protected override void OnSceneStart()
		{
			Surface surf = new Surface("Images/attacke.png");

			System.Drawing.Bitmap bmp = surf.ToBitmap();

			bmp.Save("test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
		}

		public void Run(string[] args)
		{
			new SceneStack().Start(this);
		}
	}
}
