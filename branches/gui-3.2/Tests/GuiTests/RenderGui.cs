using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib;
using AgateLib.Gui;
using AgateLib.Gui.Layout;
using AgateLib.InputLib;

namespace Tests.GuiTests
{
	class RenderGui : AgateApplication, IAgateTest 
	{
		public void Main(string[] args)
		{
			new RenderGui().Run(args);
		}

		protected override void AdjustAppInitParameters(ref AppInitParameters initParams)
		{
			initParams.ShowSplashScreen = false;
		}

		protected override void Initialize()
		{
			this.GuiRoot = new GuiRoot();

			CreateGui();
		}

		Label fps;
		Label infoLabel;
		Button hideMouse;

		protected override void Update(double time_ms)
		{
			fps.Text = Display.FramesPerSecond.ToString();

			infoLabel.Text = "Control has focus: ";

			if (GuiRoot.FocusControl != null)
				infoLabel.Text += GuiRoot.FocusControl;
		}

		protected override void Render()
		{
			Display.Clear(Color.FromArgb(50, 0, 0));
		}

		private void CreateGui()
		{
			//ThemeEngines.Graphite.Graphite.DebugOutlines = true;
			Label info = new Label("chonk");

			Panel topPanel = new Panel { Name = "topPanel" };
			Panel bottomPanel = new Panel { Name = "bottomPanel" };

			Panel leftPanel = new Panel { Name = "leftPanel" };
			Panel rightPanel = new Panel { Name = "rightPanel" };

			for (int i = 0; i < 4; i++)
			{
				rightPanel.Children.Add(new CheckBox("Check " + i.ToString()));
				leftPanel.Children.Add(new Button("Button Left " + i.ToString()));
			}

			hideMouse = leftPanel.Children[0] as Button;
			hideMouse.Text = "Hide Mouse Pointer";
			hideMouse.Click += new EventHandler(hideMouse_Click);
			hideMouse.Enabled = true;

			leftPanel.Children.Add(new TextBox { Text = "Blank" });
			leftPanel.Children.Add(new TextBox { Enabled = false, Text = "Disabled" });

			CheckBox vsync = new CheckBox("VSync");
			vsync.CheckChanged += new EventHandler(vsync_CheckChanged);
			vsync_CheckChanged(this, EventArgs.Empty);

			rightPanel.Children.Add(vsync);
			rightPanel.Children.Add(new RadioButton("Test box 2"));
			rightPanel.Children.Add(new RadioButton { Text = "Disabled", Enabled = false });
			rightPanel.Children.Add(new RadioButton { Text = "Disabled Checked", Enabled = false, Checked = true });

			leftPanel.Children.Add(info);

			topPanel.Children.Add(leftPanel);
			topPanel.Children.Add(rightPanel);
			topPanel.Layout = new HorizontalBox();

			fps = new Label();
			bottomPanel.Children.Add(fps);

			infoLabel = new Label();
			bottomPanel.Children.Add(infoLabel);

			Window wind = new Window("Test Window");
			wind.SuspendLayout();
			wind.Children.Add(topPanel);
			wind.Children.Add(bottomPanel);
			wind.AllowDrag = true;
			wind.Size = new Size(400, 300);
			bottomPanel.LayoutExpand = LayoutExpand.ExpandToMax;

			wind.AcceptButton = (Button)leftPanel.Children[0];

			GuiRoot.Children.Add(wind);
			GuiRoot.ResumeLayout();
			
			info.Text = string.Format("L:{0}:{1}   R:{2}:{3}",
				leftPanel.MinSize.Height, leftPanel.Height, rightPanel.MinSize.Height, rightPanel.Height);

			int totalsize = 0;
			foreach (Widget child in rightPanel.Children)
				totalsize += child.Height;

			info.Text += "   total: " + totalsize.ToString();

			System.Diagnostics.Debug.Assert(
				leftPanel.PointToClient(leftPanel.PointToScreen(new Point(10, 8))) == new Point(10, 8));

			Display.PackAllSurfaces();
		}

		void hideMouse_Click(object sender, EventArgs e)
		{
			if (Mouse.IsHidden)
			{
				Mouse.Show();
				hideMouse.Text = "Hide Mouse Pointer";
			}
			else
			{
				Mouse.Hide();
				hideMouse.Text = "Show Mouse Pointer";
			}
		}

		void vsync_CheckChanged(object sender, EventArgs e)
		{
			Display.VSync = !Display.VSync;
		}

		void btn_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;

			switch (btn.LayoutExpand)
			{
				case LayoutExpand.Default:
					btn.LayoutExpand = LayoutExpand.ExpandToMax;
					break;

				case LayoutExpand.ExpandToMax:
					btn.LayoutExpand = LayoutExpand.ShrinkToMin;
					break;

				case LayoutExpand.ShrinkToMin:
					btn.LayoutExpand = LayoutExpand.Default;
					break;
			}
		}

		#region IAgateTest Members

		public string Name
		{
			get { return "Basic GUI Tester"; }
		}

		public string Category
		{
			get { return "Gui"; }
		}

		#endregion
	}
}
