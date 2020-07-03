using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    public class UserInterfaceConfig
    {
        public Rectangle ScreenArea { get; set; } = new Rectangle(0, 0, 1280, 720);

        public Size ScreenSize => ScreenArea.Size;

        public float VisualScaling => SystemScaling * UserScaling;

        public string DefaultTheme { get; set; }

        public string DefaultFont { get; set; }

        public float SystemScaling { get; set; } = 1;

        public float UserScaling { get; set; } = 1;
    }
}
