using System;
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
        void root_Update(object sender, EventArgs e)
        {
            fps.Text = Display.FramesPerSecond.ToString();
        }

        void root_DrawBehindGui(object sender, EventArgs e)
        {
            Display.Clear(Color.DarkBlue);
        }

        private void CreateGui()
        {
            //ThemeEngines.Graphite.Graphite.DebugOutlines = true;

            Panel topPanel = new Panel();
            Panel bottomPanel = new Panel();

            Panel leftPanel = new Panel();
            Panel rightPanel = new Panel();

            for (int i = 0; i < 4; i++)
            {
                leftPanel.Children.Add(new TextBox());
                rightPanel.Children.Add(new TextBox());
            }

            topPanel.Children.Add(leftPanel);
            topPanel.Children.Add(rightPanel);
            topPanel.Layout = new Layout.HorizontalBox();

            fps = new Label();
            bottomPanel.Children.Add(fps);

            Window wind = new Window("Test Window");
            wind.SuspendLayout();
            wind.Children.Add(topPanel);
            wind.Children.Add(bottomPanel);
            wind.AllowDrag = true;
            wind.Size = new Size(400, 300);
            bottomPanel.LayoutExpand = LayoutExpand.ExpandToMax;

            root.Children.Add(wind);
            root.ResumeLayout();

            System.Diagnostics.Debug.Assert(
                leftPanel.PointToClient(leftPanel.PointToScreen(new Point(10, 8))) == new Point(10, 8));

            Display.PackAllSurfaces();
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
