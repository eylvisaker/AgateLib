using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Gui;

namespace Tests.GuiTests
{
	public class ScrollBar : AgateGame, IAgateTest 
	{
		public string Name
		{
			get { return "Scroll Bars"; }
		}

		public string Category
		{
			get { return "Gui"; }
		}

		public void Main(string[] args)
		{
			Run(args);
		}

		protected override void AdjustAppInitParameters(ref AppInitParameters initParams)
		{
			initParams.ShowSplashScreen = false;
			initParams.InitializeAudio = false;
			initParams.InitializeJoysticks = false;

		}

		protected override void Initialize()
		{
			base.GuiRoot = new GuiRoot();

			Window wind = new Window("Scroll Bars test");
			wind.Size = new Size(400, 300);

			wind.Layout = new AgateLib.Gui.Layout.VerticalBox();

			Panel rightPanel = new Panel();
			Label vbarValueLabel = new Label();
			rightPanel.Children.Add(vbarValueLabel);

			VerticalScrollBar vbar = new VerticalScrollBar();
			vbar.ValueChanged += (sender, e) => { vbarValueLabel.Text = "Vertical value: " + vbar.Value.ToString(); };

			wind.Children.Add(vbar);
			wind.Children.Add(rightPanel);

			base.GuiRoot.Children.Add(wind);
		}
	}
}
