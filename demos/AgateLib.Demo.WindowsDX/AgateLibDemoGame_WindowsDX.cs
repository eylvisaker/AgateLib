using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AgateLib.Demo.WindowsDX
{
    public class AgateLibDemoGame_WindowsDX : AgateLibDemoGame
    {
        public AgateLibDemoGame_WindowsDX() : base(getScalingFactor())
        { }

        private static float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);

            return g.DpiX / 96.0f;
        }
    }

}
