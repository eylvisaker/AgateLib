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
	public class ScrollBars : AgateGame, IAgateTest 
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

			wind.Layout = new AgateLib.Gui.Layout.HorizontalBox();

			Panel rightPanel = new Panel();
			Label leftVbarValueLabel = new Label();
			leftVbarValueLabel.Text = "Left Vertical value: ";
			rightPanel.Children.Add(leftVbarValueLabel);

			Label rightVbarValueLabel = new Label();
			rightVbarValueLabel.Text = "Right Vertical value: ";
			rightPanel.Children.Add(rightVbarValueLabel);

			VerticalScrollBar leftVbar = new VerticalScrollBar();
			leftVbar.ValueChanged += (sender, e) => { leftVbarValueLabel.Text = "Left Vertical value: " + leftVbar.Value.ToString(); };

			VerticalScrollBar rightVbar = new VerticalScrollBar();
			rightVbar.ValueChanged += (sender, e) => { rightVbarValueLabel.Text = "Right  Vertical value: " + rightVbar.Value.ToString(); };
			rightVbar.MaxValue = 10;
			rightVbar.LargeChange = 5;

			wind.Children.Add(leftVbar);
			wind.Children.Add(rightVbar);
			wind.Children.Add(rightPanel);

			base.GuiRoot.Children.Add(wind);
		}
	}
}
