using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace Tests.Fonts
{
	class Builtin : AgateGame, IAgateTest 
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Built-in font tests"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}

		public void Main(string[] args)
		{
			Run(args);
		}

		#endregion

		List<FontSurface> fonts = new List<FontSurface>();
		string nonenglish;

		protected override void AdjustAppInitParameters(ref AppInitParameters initParams)
		{
			initParams.AllowResize = true;
			initParams.ShowSplashScreen = false;
		}

		protected override void Initialize()
		{
			base.Initialize();

			fonts.Add(FontSurface.AgateSans10);
			fonts.Add(FontSurface.AgateSans14);
			fonts.Add(FontSurface.AgateSerif10);
			fonts.Add(FontSurface.AgateSerif14);
			fonts.Add(FontSurface.AgateMono10);

			for (char i = (char)128; i < 255; i++)
			{
				nonenglish += i;
			}
		}
		protected override void Render()
		{
			Display.Clear(Color.Black);

			int y = 0;
			foreach(var font in fonts)
			{
				font.Color = Color.White;

				font.DrawText(0, y, font.FontName);
				int x = 20;
				y += font.FontHeight;

				font.DrawText(x,y,"01234567890!@#$%^&*()[]{}\\|/?'\";:.>,<`~-_=+");
				y +=font.FontHeight;

				font.DrawText(x, y, "The quick brown fox jumped over the lazy dogs.");
				y += font.FontHeight;

				font.DrawText(x, y, "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS.");
				y += font.FontHeight;

				font.DrawText(x, y, nonenglish);
				y += font.FontHeight;
				
				y += font.FontHeight;
			}
		}
	}
}
