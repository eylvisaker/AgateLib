using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;
using AgateLib.Tests;
using AgateLib.Platform.WinForms;
using AgateLib.Configuration;

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

		public AgateConfig Configuration { get; set; }

		public override void Update(double deltaT)
		{
		}

		public override void Draw()
		{
			SceneFinished = true;
		}

		protected override internal void OnSceneStart()
		{
			Surface surf = new Surface("attacke.png");

			System.Drawing.Bitmap bmp = surf.ToBitmap();

			bmp.Save("test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
		}

		public void ModifySetup(IAgateSetup setup)
		{
		}

		public void Run()
		{
			SceneStack.Start(this);
		}
	}
}
