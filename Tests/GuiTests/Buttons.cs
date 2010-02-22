using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Gui;
using AgateLib.Resources;

namespace Tests.GuiTests
{
	class Buttons : AgateGame, IAgateTest
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Buttons"; }
		}

		public string Category
		{
			get { return "Gui"; }
		}

		public void Main(string[] args)
		{
			Run(args);
		}

		#endregion

		protected override void AdjustAppInitParameters(ref AppInitParameters initParams)
		{
			initParams.ShowSplashScreen = false;
		}
		protected override void Initialize()
		{
			AgateResourceCollection resources = AgateResourceCollection.FromZipArchive("Data/gui.zip");

			this.GuiRoot = new GuiRoot();
			this.GuiRoot.ThemeEngine = new AgateLib.Gui.ThemeEngines.Venus.Venus(resources);

			Window wind = new Window("Buttons test");
			wind.Size = new AgateLib.Geometry.Size(320, 240);
			
			Button b = new Button();
			b.Text = "Press me";

			Button c = new Button();
			c.Text = "No, press me!";

			wind.Children.Add(b);
			wind.Children.Add(c);

			this.GuiRoot.Children.Add(wind);
		}

		protected override void Render()
		{
			Display.Clear(Color.Maroon);
		}
	}
}