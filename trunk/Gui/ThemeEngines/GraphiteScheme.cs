using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Graphite
{
    public class GraphiteScheme
    {
        public GraphiteScheme()
        {
        }

        public static GraphiteScheme DefaultScheme
        {
            get
            {
                GraphiteScheme retval = new GraphiteScheme();
                retval.SetDefaults();
                return retval;
            }
        }

        void SetDefaults()
        {
            ControlFont = new FontSurface("Sans Serif", 8);
            TitleFont = new FontSurface("Sans Serif", 10);
            CenterTitle = true;

            FontColor = Color.White;
            FontColorDisabled = Color.Gray;
            DropShadowSize = 10;

            WindowNoTitle = new Surface("Images/window_no_title.png");
            WindowWithTitle = new Surface("Images/window_client.png");
            WindowTitleBar = new Surface("Images/window_titlebar.png");
            WindowTitleBarStretchRegion = new Rectangle(5, 3, 58, 27);
            WindowNoTitleStretchRegion = new Rectangle(5, 5, 54, 54);
            WindowWithTitleStretchRegion = new Rectangle(6, 3, 52, 55);

            SetButtonImages(new Surface("Images/black_button.png"), new Size(64, 32));
            ButtonStretchRegion = new Rectangle(6, 6, 52, 20);
            ButtonTextPadding = 2;

            CloseButton = Button;
            CloseButtonInactive = Button;
            CloseButtonMouseOver = Button;

            CheckBoxUnchecked= Button;
            CheckBoxChecked = Button;
            CheckBoxSpacing = 4;

            TextBox = Button;
            TextBoxDisabled = Button;
            TextBoxMouseOver = Button;
            TextBoxStretchRegion = Rectangle.FromLTRB(4, 3, 28, 20);
        }

        private void SetButtonImages(Surface surface, Size buttonSize)
        {
            Surface[] surfs = SplitSurface("button", surface, buttonSize, 6, 6);

            Button = surfs[0];
            ButtonHover = surfs[1];
            ButtonDefault = surfs[2];
            ButtonDefaultHover = surfs[3];
            ButtonPressed = surfs[4];
            ButtonDisabled = surfs[5];

            surface.Dispose();
        }

        private Surface[] SplitSurface(string name, Surface surface, Size subSurfaceSize, 
            int requiredImages, int maxImages)
        {
            Point pt = new Point();
            PixelBuffer pixels = surface.ReadPixels();

            List<Surface> retval = new List<Surface>();

            for (int i = 0; i < maxImages; i++)
            {
                Surface surf = new Surface(subSurfaceSize);
                surf.WritePixels(pixels, new Rectangle(pt, subSurfaceSize), Point.Empty);

                retval.Add(surf);

                pt.X += subSurfaceSize.Width;
                if (pt.X == surface.SurfaceWidth)
                {
                    pt.X = 0;
                    pt.Y += subSurfaceSize.Height;
                }
                else if (pt.X > surface.SurfaceWidth)
                {
                    throw new AgateGuiException(
                        "Image for " + name + 
                        " does not have a width that is a multiple of " + subSurfaceSize.Width + "."); 
                }

                if (pt.Y + subSurfaceSize.Height > surface.SurfaceHeight)
                {
                    if (retval.Count < requiredImages)
                        throw new AgateGuiException(
                            "There are not enough subimages in the " + name + " image.");
                }
            }

            return retval.ToArray();
        }

        int mInsertionPointBlinkTime = 500;

        public int InsertionPointBlinkTime
        {
            get { return mInsertionPointBlinkTime; }
            set
            {
                if (value < 1)
                    throw new ArgumentNullException();

                mInsertionPointBlinkTime = value;
            }
        }
        public FontSurface ControlFont { get; set; }
        public FontSurface TitleFont { get; set; }
        public bool CenterTitle { get; set; }
        public int DropShadowSize { get; set; }

        public Color FontColor { get; set; }
        public Color FontColorDisabled { get; set; }
        public Surface WindowNoTitle { get; set; }
        public Surface WindowWithTitle { get; set; }
        public Surface WindowTitleBar { get; set; }
        public Rectangle WindowNoTitleStretchRegion { get; set; }
        public Rectangle WindowWithTitleStretchRegion { get; set; }
        public Rectangle WindowTitleBarStretchRegion { get; set; }

        public Surface CloseButton { get; set; }
        public Surface CloseButtonMouseOver { get; set; }
        public Surface CloseButtonInactive { get; set; }

        public Rectangle ButtonStretchRegion { get; set; }
        public Surface Button { get; set; }
        public Surface ButtonDefault { get; set; }
        public Surface ButtonDefaultHover { get; set; }
        public Surface ButtonHover { get; set; }
        public Surface ButtonPressed { get; set; }
        public Surface ButtonDisabled { get; set; }
        public int ButtonTextPadding { get; set; }

        public Surface CheckBoxChecked { get; set; }
        public Surface CheckBoxUnchecked { get; set; }
        public int CheckBoxSpacing { get; set; }

        public Surface TextBox { get; set; }
        public Surface TextBoxMouseOver { get; set; }
        public Surface TextBoxDisabled { get; set; }
        public Rectangle TextBoxStretchRegion { get; set; }

    }
}
