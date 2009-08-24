using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.WinForms;

namespace Tests.WinFormsTests
{
	class Converters : AgateApplication, IAgateTest 
	{
		public string Name
		{
			get { return "Conversion Tests"; }
		}

		public string Category
		{
			get { return "WinForms"; }
		}

		public void Main(string[] args)
		{
			Run(args);
		}

		protected override void Initialize()
		{
			Surface surf = new Surface("attacke.png");

			System.Drawing.Bitmap bmp = surf.ToBitmap();

			bmp.Save("test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
		}
	}
}
