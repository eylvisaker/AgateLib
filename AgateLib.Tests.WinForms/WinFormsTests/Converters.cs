using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Platform.WinForms;
using AgateLib.Tests;

namespace AgateLib.Tests.WinFormsTests
{
	class Converters : Scene, IAgateTest 
	{
		public string Name
		{
			get { return "Conversion Tests"; }
		}

		public string Category
		{
			get { return "WinForms"; }
		}

		public override void Update(double deltaT)
		{
		}

		public override void Draw()
		{
			SceneFinished = true;
		}

		protected override internal void OnSceneStart()
		{
			Surface surf = new Surface("Images/attacke.png");

			System.Drawing.Bitmap bmp = surf.ToBitmap();

			bmp.Save("test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
		}

		public void Run(string[] args)
		{
			SceneStack.Start(this);
		}
	}
}
