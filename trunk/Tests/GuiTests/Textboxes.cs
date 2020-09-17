using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Gui;

namespace Tests.GuiTests
{
	class Textboxes : AgateGame, IAgateTest
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Textboxes"; }
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

			TextBox b = new TextBox();
			b.Text = "This is a single line textbox.";

			wind.Children.Add(b);

			TextBox multi = new TextBox();
			multi.Text = "This is a multiline textbox.\nYou can enter multiple lines.\n" +
				"In code, these lines are separated by the usual \"\\n\" character.";

			multi.MultiLine = true;

			wind.Children.Add(multi);

			this.GuiRoot.Children.Add(wind);
		}
	}
}