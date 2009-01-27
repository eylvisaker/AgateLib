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
            ControlFont = new FontSurface("Sans Serif", 12);
            TitleFont = new FontSurface("Sans Serif", 10);
            CenterTitle = true;

            Color lightColor = Color.FromArgb(200, 200, 200);
            Color darkColor = Color.FromArgb(140, 140, 140);
            
            WindowBorderColor = Color.FromArgb(40, 40, 40);
            WindowBackColor = new Gradient(
                lightColor, lightColor, darkColor, darkColor);

            FontColor = Color.Black;
            FontColorDisabled = Color.Gray;
            DropShadowSize = 10;

            CloseButton = new Surface("Images/button-close.png");
            CloseButtonInactive = new Surface("Images/button-inactive.png");
            CloseButtonMouseOver = new Surface("Images/button-close-focus.png");

            Button = new Surface("Images/button_round.png");
            ButtonDefault = new Surface("Images/button_round_blue.png");
            ButtonMouseOver = ButtonDefault;
            ButtonActivate = new Surface("Images/button_round_blue_push.png");
            ButtonDisabled = new Surface("Images/button_round_insens.png");
            ButtonStretchRegion = new Rectangle(8, 8, 28, 12);

            CheckBoxDown = new Surface("Images/checkbox_checked.png");
            CheckBoxUp = new Surface("Images/checkbox_unchecked.png");
            CheckBoxSpacing = 4;

            TextBox = new Surface("Images/textbox.png");
            TextBoxDisabled = new Surface("Images/textbox_disabled.png");
            TextBoxMouseOver = new Surface("Images/textbox_mouseover.png");
            TextBoxStretchRegion = Rectangle.FromLTRB(4, 3, 28, 20);
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

        public Gradient WindowBackColor { get; set; }
        public Color FontColor { get; set; }
        public Color FontColorDisabled { get; set; }
        public Color WindowBorderColor { get; set; }

        public Surface CloseButton { get; set; }
        public Surface CloseButtonMouseOver { get; set; }
        public Surface CloseButtonInactive { get; set; }

        public Rectangle ButtonStretchRegion { get; set; }
        public Surface Button { get; set; }
        public Surface ButtonDefault { get; set; }
        public Surface ButtonMouseOver { get; set; }
        public Surface ButtonActivate { get; set; }
        public Surface ButtonDisabled { get; set; }

        public Surface CheckBoxDown { get; set; }
        public Surface CheckBoxUp { get; set; }
        public int CheckBoxSpacing { get; set; }

        public Surface TextBox { get; set; }
        public Surface TextBoxMouseOver { get; set; }
        public Surface TextBoxDisabled { get; set; }
        public Rectangle TextBoxStretchRegion { get; set; }

    }
}
