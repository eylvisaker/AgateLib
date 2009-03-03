﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.Tester
{
    class RenderGui
    {
        GuiRoot root = new GuiRoot();

        public RenderGui()
        {
            CreateGui();
        }

        internal void Run()
        {
            root.DrawBehindGui += new EventHandler(root_DrawBehindGui);
            root.Update += new EventHandler(root_Update);
            root.Run();
        }

        Label fps;
        Label infoLabel;
        Button hideMouse;

        void root_Update(object sender, EventArgs e)
        {
            fps.Text = Display.FramesPerSecond.ToString();

            infoLabel.Text = "Control has focus: ";
            
            if (root.FocusControl != null)
                infoLabel.Text += root.FocusControl;
        }

        void root_DrawBehindGui(object sender, EventArgs e)
        {
            Display.Clear(Color.FromArgb(50,0,0));
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
                rightPanel.Children.Add(new Button("right " + i.ToString()));

                if (i % 2 == 1)
                    leftPanel.Children.Add(new Button("Button Left " + i.ToString()));
                else 
                    rightPanel.Children[i].Enabled = false;
            }

            hideMouse = rightPanel.Children[0] as Button;
            hideMouse.Text = "Hide Mouse Pointer";
            hideMouse.Click += new EventHandler(hideMouse_Click);
            hideMouse.Enabled = true;

            leftPanel.Children.Add(new TextBox { Text = "Blank" });
            leftPanel.Children.Add(new TextBox { Enabled = false, Text = "Disabled" });

            CheckBox vsync = new CheckBox("VSync");
            vsync.CheckChanged += new EventHandler(vsync_CheckChanged);
            vsync_CheckChanged(this, EventArgs.Empty);

            rightPanel.Children.Add(vsync);
            rightPanel.Children.Add(new CheckBox("Test box 2"));
            rightPanel.Children.Add(new CheckBox { Text = "Disabled", Enabled = false });
            rightPanel.Children.Add(new CheckBox { Text = "Disabled Checked", Enabled = false, Checked = true });

            leftPanel.Children.Add(info);

            topPanel.Children.Add(leftPanel);
            topPanel.Children.Add(rightPanel);
            topPanel.Layout = new Layout.HorizontalBox();

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

            root.Children.Add(wind);
            root.ResumeLayout();

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
            if (InputLib.Mouse.IsHidden)
            {
                InputLib.Mouse.Show();
                hideMouse.Text = "Hide Mouse Pointer";
            }
            else
            {
                InputLib.Mouse.Hide();
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
    }
}
