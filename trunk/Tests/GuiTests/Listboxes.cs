using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Gui;

namespace Tests.GuiTests
{
	class Listboxes : AgateGame, IAgateTest
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Listboxes"; }
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
			this.GuiRoot = new GuiRoot();

			Window wind = new Window("Textboxes test");
			wind.Size = new AgateLib.Geometry.Size(320, 240);

			ListBox a = new ListBox();
			
			wind.Children.Add(a);

			ListBox b = new ListBox();
			b.Text = "This is a multiline textbox.\nYou can enter multiple lines.\n" +
				"In code, these lines are separated by the usual \"\\n\" character.";

			wind.Children.Add(b);

			this.GuiRoot.Children.Add(wind);
		}
	}
}