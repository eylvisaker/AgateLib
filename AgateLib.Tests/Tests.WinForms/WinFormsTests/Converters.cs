using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;
using AgateLib.Testing;
using AgateLib.Platform.WinForms;

namespace AgateLib.Testing.WinFormsTests
{
	class Converters : Scene, ISceneModelTest 
	{
		public string Name
		{
			get { return "Conversion Tests"; }
		}

		public string Category
		{
			get { return "WinForms"; }
		}

		protected override void OnSceneStart()
		{
			Surface surf = new Surface("attacke.png");

			System.Drawing.Bitmap bmp = surf.ToBitmap();

			bmp.Save("test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
		}

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public Scene StartScene
		{
			get { return this; }
		}

		public override void Update(double deltaT)
		{
		}

		public override void Draw()
		{
			SceneFinished = true;
		}
	}
}
