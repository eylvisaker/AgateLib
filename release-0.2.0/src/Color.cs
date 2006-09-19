//     ``The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ERY.AgateLib
{
    /// <summary>
    /// Replacement for System.Drawing.Color structure.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    [Serializable]
    public struct Color
    {

        #region --- Static Named Color properties ---
        /// <summary>
        /// 
        /// </summary>
        public static Color AliceBlue { get { return new Color(System.Drawing.Color.AliceBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color AntiqueWhite { get { return new Color(System.Drawing.Color.AntiqueWhite); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Aqua { get { return new Color(System.Drawing.Color.Aqua); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Aquamarine { get { return new Color(System.Drawing.Color.Aquamarine); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Azure { get { return new Color(System.Drawing.Color.Azure); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Beige { get { return new Color(System.Drawing.Color.Beige); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Bisque { get { return new Color(System.Drawing.Color.Bisque); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Black { get { return new Color(System.Drawing.Color.Black); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color BlanchedAlmond { get { return new Color(System.Drawing.Color.BlanchedAlmond); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Blue { get { return new Color(System.Drawing.Color.Blue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color BlueViolet { get { return new Color(System.Drawing.Color.BlueViolet); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Brown { get { return new Color(System.Drawing.Color.Brown); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color BurlyWood { get { return new Color(System.Drawing.Color.BurlyWood); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color CadetBlue { get { return new Color(System.Drawing.Color.CadetBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Chartreuse { get { return new Color(System.Drawing.Color.Chartreuse); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Chocolate { get { return new Color(System.Drawing.Color.Chocolate); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Coral { get { return new Color(System.Drawing.Color.Coral); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color CornflowerBlue { get { return new Color(System.Drawing.Color.CornflowerBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Cornsilk { get { return new Color(System.Drawing.Color.Cornsilk); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Crimson { get { return new Color(System.Drawing.Color.Crimson); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Cyan { get { return new Color(System.Drawing.Color.Cyan); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkBlue { get { return new Color(System.Drawing.Color.DarkBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkCyan { get { return new Color(System.Drawing.Color.DarkCyan); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkGoldenrod { get { return new Color(System.Drawing.Color.DarkGoldenrod); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkGray { get { return new Color(System.Drawing.Color.DarkGray); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkGreen { get { return new Color(System.Drawing.Color.DarkGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkKhaki { get { return new Color(System.Drawing.Color.DarkKhaki); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkMagenta { get { return new Color(System.Drawing.Color.DarkMagenta); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkOliveGreen { get { return new Color(System.Drawing.Color.DarkOliveGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkOrange { get { return new Color(System.Drawing.Color.DarkOrange); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkOrchid { get { return new Color(System.Drawing.Color.DarkOrchid); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkRed { get { return new Color(System.Drawing.Color.DarkRed); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkSalmon { get { return new Color(System.Drawing.Color.DarkSalmon); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkSeaGreen { get { return new Color(System.Drawing.Color.DarkSeaGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkSlateBlue { get { return new Color(System.Drawing.Color.DarkSlateBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkSlateGray { get { return new Color(System.Drawing.Color.DarkSlateGray); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkTurquoise { get { return new Color(System.Drawing.Color.DarkTurquoise); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkViolet { get { return new Color(System.Drawing.Color.DarkViolet); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DeepPink { get { return new Color(System.Drawing.Color.DeepPink); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DeepSkyBlue { get { return new Color(System.Drawing.Color.DeepSkyBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DimGray { get { return new Color(System.Drawing.Color.DimGray); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color DodgerBlue { get { return new Color(System.Drawing.Color.DodgerBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Firebrick { get { return new Color(System.Drawing.Color.Firebrick); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color FloralWhite { get { return new Color(System.Drawing.Color.FloralWhite); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color ForestGreen { get { return new Color(System.Drawing.Color.ForestGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Fuchsia { get { return new Color(System.Drawing.Color.Fuchsia); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Gainsboro { get { return new Color(System.Drawing.Color.Gainsboro); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color GhostWhite { get { return new Color(System.Drawing.Color.GhostWhite); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Gold { get { return new Color(System.Drawing.Color.Gold); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Goldenrod { get { return new Color(System.Drawing.Color.Goldenrod); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Gray { get { return new Color(System.Drawing.Color.Gray); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Green { get { return new Color(System.Drawing.Color.Green); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color GreenYellow { get { return new Color(System.Drawing.Color.GreenYellow); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Honeydew { get { return new Color(System.Drawing.Color.Honeydew); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color HotPink { get { return new Color(System.Drawing.Color.HotPink); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color IndianRed { get { return new Color(System.Drawing.Color.IndianRed); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Indigo { get { return new Color(System.Drawing.Color.Indigo); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Ivory { get { return new Color(System.Drawing.Color.Ivory); } }

        /// <summary>
        /// 
        /// </summary>
        public static Color Khaki { get { return new Color(System.Drawing.Color.Khaki); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Lavender { get { return new Color(System.Drawing.Color.Lavender); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LavenderBlush { get { return new Color(System.Drawing.Color.LavenderBlush); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LawnGreen { get { return new Color(System.Drawing.Color.LawnGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LemonChiffon { get { return new Color(System.Drawing.Color.LemonChiffon); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightBlue { get { return new Color(System.Drawing.Color.LightBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightCoral { get { return new Color(System.Drawing.Color.LightCoral); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightCyan { get { return new Color(System.Drawing.Color.LightCyan); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightGoldenrodYellow { get { return new Color(System.Drawing.Color.LightGoldenrodYellow); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightGray { get { return new Color(System.Drawing.Color.LightGray); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightGreen { get { return new Color(System.Drawing.Color.LightGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightPink { get { return new Color(System.Drawing.Color.LightPink); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightSalmon { get { return new Color(System.Drawing.Color.LightSalmon); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightSeaGreen { get { return new Color(System.Drawing.Color.LightSeaGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightSkyBlue { get { return new Color(System.Drawing.Color.LightSkyBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightSlateGray { get { return new Color(System.Drawing.Color.LightSlateGray); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightSteelBlue { get { return new Color(System.Drawing.Color.LightSteelBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LightYellow { get { return new Color(System.Drawing.Color.LightYellow); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Lime { get { return new Color(System.Drawing.Color.Lime); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color LimeGreen { get { return new Color(System.Drawing.Color.LimeGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Linen { get { return new Color(System.Drawing.Color.Linen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Magenta { get { return new Color(System.Drawing.Color.Magenta); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Maroon { get { return new Color(System.Drawing.Color.Maroon); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumAquamarine { get { return new Color(System.Drawing.Color.MediumAquamarine); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumBlue { get { return new Color(System.Drawing.Color.MediumBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumOrchid { get { return new Color(System.Drawing.Color.MediumOrchid); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumPurple { get { return new Color(System.Drawing.Color.MediumPurple); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumSeaGreen { get { return new Color(System.Drawing.Color.MediumSeaGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumSlateBlue { get { return new Color(System.Drawing.Color.MediumSlateBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumSpringGreen { get { return new Color(System.Drawing.Color.MediumSpringGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumTurquoise { get { return new Color(System.Drawing.Color.MediumTurquoise); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MediumVioletRed { get { return new Color(System.Drawing.Color.MediumVioletRed); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MidnightBlue { get { return new Color(System.Drawing.Color.MidnightBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MintCream { get { return new Color(System.Drawing.Color.MintCream); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color MistyRose { get { return new Color(System.Drawing.Color.MistyRose); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Moccasin { get { return new Color(System.Drawing.Color.Moccasin); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color NavajoWhite { get { return new Color(System.Drawing.Color.NavajoWhite); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Navy { get { return new Color(System.Drawing.Color.Navy); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color OldLace { get { return new Color(System.Drawing.Color.OldLace); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Olive { get { return new Color(System.Drawing.Color.Olive); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color OliveDrab { get { return new Color(System.Drawing.Color.OliveDrab); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Orange { get { return new Color(System.Drawing.Color.Orange); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color OrangeRed { get { return new Color(System.Drawing.Color.OrangeRed); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Orchid { get { return new Color(System.Drawing.Color.Orchid); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color PaleGoldenrod { get { return new Color(System.Drawing.Color.PaleGoldenrod); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color PaleGreen { get { return new Color(System.Drawing.Color.PaleGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color PaleTurquoise { get { return new Color(System.Drawing.Color.PaleTurquoise); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color PaleVioletRed { get { return new Color(System.Drawing.Color.PaleVioletRed); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color PapayaWhip { get { return new Color(System.Drawing.Color.PapayaWhip); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color PeachPuff { get { return new Color(System.Drawing.Color.PeachPuff); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Peru { get { return new Color(System.Drawing.Color.Peru); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Pink { get { return new Color(System.Drawing.Color.Pink); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Plum { get { return new Color(System.Drawing.Color.Plum); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color PowderBlue { get { return new Color(System.Drawing.Color.PowderBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Purple { get { return new Color(System.Drawing.Color.Purple); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Red { get { return new Color(System.Drawing.Color.Red); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color RosyBrown { get { return new Color(System.Drawing.Color.RosyBrown); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color RoyalBlue { get { return new Color(System.Drawing.Color.RoyalBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SaddleBrown { get { return new Color(System.Drawing.Color.SaddleBrown); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Salmon { get { return new Color(System.Drawing.Color.Salmon); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SandyBrown { get { return new Color(System.Drawing.Color.SandyBrown); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SeaGreen { get { return new Color(System.Drawing.Color.SeaGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SeaShell { get { return new Color(System.Drawing.Color.SeaShell); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Sienna { get { return new Color(System.Drawing.Color.Sienna); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Silver { get { return new Color(System.Drawing.Color.Silver); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SkyBlue { get { return new Color(System.Drawing.Color.SkyBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SlateBlue { get { return new Color(System.Drawing.Color.SlateBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SlateGray { get { return new Color(System.Drawing.Color.SlateGray); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Snow { get { return new Color(System.Drawing.Color.Snow); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SpringGreen { get { return new Color(System.Drawing.Color.SpringGreen); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color SteelBlue { get { return new Color(System.Drawing.Color.SteelBlue); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Tan { get { return new Color(System.Drawing.Color.Tan); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Teal { get { return new Color(System.Drawing.Color.Teal); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Thistle { get { return new Color(System.Drawing.Color.Thistle); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Tomato { get { return new Color(System.Drawing.Color.Tomato); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Transparent { get { return new Color(System.Drawing.Color.Transparent); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Turquoise { get { return new Color(System.Drawing.Color.Turquoise); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Violet { get { return new Color(System.Drawing.Color.Violet); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Wheat { get { return new Color(System.Drawing.Color.Wheat); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color White { get { return new Color(System.Drawing.Color.White); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color WhiteSmoke { get { return new Color(System.Drawing.Color.WhiteSmoke); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color Yellow { get { return new Color(System.Drawing.Color.Yellow); } }
        /// <summary>
        /// 
        /// </summary>
        public static Color YellowGreen { get { return new Color(System.Drawing.Color.YellowGreen); } }

        #endregion

        #region --- Private Data ---

        [FieldOffset(3)]
        byte a;

        [FieldOffset(2)]
        byte r;

        [FieldOffset(1)]
        byte g;
        
        [FieldOffset(0)]
        byte b;

        [FieldOffset(0)]
        int argb;

        #endregion

        #region --- Construction ---

        private Color(int a, int r, int g, int b)
        {
            this.argb = 0;
            this.a = (byte)a;
            this.r = (byte)r;
            this.g = (byte)g;
            this.b = (byte)b;
        }        
        private Color(System.Drawing.Color clr)
            : this(clr.A, clr.R, clr.G, clr.B)
        {
        }

        /// <summary>
        /// Creates a Color structure from the given color and alpha value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="baseColor"></param>
        /// <returns></returns>
        public static Color FromArgb(int a, Color baseColor)
        {
            ValidateByteValue(ref a);

            return new Color((byte)a, baseColor.r, baseColor.g, baseColor.b);
        }
        /// <summary>
        /// Creates a color structure from r, g, b values.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color FromArgb(int r, int g, int b)
        {
            ValidateByteValue(ref r);
            ValidateByteValue(ref g);
            ValidateByteValue(ref b);

            return new Color(255, r, g, b);
        }
        /// <summary>
        /// Creates a color structure from a, r, g, b values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color FromArgb(int a, int r, int g, int b)
        {
            ValidateByteValue(ref a);
            ValidateByteValue(ref r);
            ValidateByteValue(ref g);
            ValidateByteValue(ref b);

            return new Color(a, r, g, b);
        }
        /// <summary>
        /// Creates a color structure from a 32-bit integer, in
        /// the format 0xAARRGGBB.
        /// </summary>
        /// <param name="argbvalue"></param>
        /// <returns></returns>
        public static Color FromArgb(int argbvalue)
        {
            byte b = (byte)(argbvalue & 0xff); argbvalue >>= 8;
            byte g = (byte)(argbvalue & 0xff); argbvalue >>= 8;
            byte r = (byte)(argbvalue & 0xff); argbvalue >>= 8;
            byte a = (byte)(argbvalue & 0xff); argbvalue >>= 8;

            return new Color(a, r, g, b);
        }

        private static void ValidateByteValue(ref int val)
        {
            if (val > 255)
                val = 255;
            else if (val < 0)
                val = 0;
        }
        #endregion
        #region --- Public Properties ---

        /// <summary>
        /// Gets or sets the alpha channel.
        /// </summary>
        public byte A { get { return a; } set { a = value; } }
        /// <summary>
        /// Gets or sets the red channel.
        /// </summary>
        public byte R { get { return r; } set { r = value; } }
        /// <summary>
        /// Gets or sets the green channel.
        /// </summary>
        public byte G { get { return g; } set { g = value; } }
        /// <summary>
        /// Gets or sets the blue channel.
        /// </summary>
        public byte B { get { return b; } set { b = value; } }

        #endregion

        #region --- Object Overrides ---

        /// <summary>
        /// Returns a string representing this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Color: " + ToArgb().ToString("X");
        }
        /// <summary>
        /// Returns a unique hashcode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToArgb();
        }
        /// <summary>
        /// Checks to see if this is equal to another object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Color)
                return Equals((Color)obj);
            else
                return base.Equals(obj);
        }
        /// <summary>
        /// Checks to see if this is equal to another Color structure.
        /// </summary>
        /// <param name="clr"></param>
        /// <returns></returns>
        public bool Equals(Color clr)
        {
            if (this.ToArgb() == clr.ToArgb())
                return true;
            else
                return false;
        }

        #endregion
        #region --- Operator Overrides ---

        /// <summary>
        /// Compares two colors to see if they are equal.
        /// </summary>
        /// <param name="clra"></param>
        /// <param name="clrb"></param>
        /// <returns></returns>
        public static bool operator ==(Color clra, Color clrb)
        {
            return clra.Equals(clrb);
        }
        /// <summary>
        /// Compares two colors to see if they are not equal.
        /// </summary>
        /// <param name="clra"></param>
        /// <param name="clrb"></param>
        /// <returns></returns>
        public static bool operator !=(Color clra, Color clrb)
        {
            return !clra.Equals(clrb);
        }

        /// <summary>
        /// Explicit conversion to a System.Drawing.Color structure, for
        /// interop with System.Drawing namespaces.
        /// </summary>
        /// <param name="clr"></param>
        /// <returns></returns>
        public static explicit operator System.Drawing.Color(Color clr)
        {
            return System.Drawing.Color.FromArgb(clr.ToArgb());
        }
        /// <summary>
        /// Explicit conversion from a System.Drawing.Color structure, for
        /// interop with System.Drawing namespaces.
        /// </summary>
        /// <param name="clr"></param>
        /// <returns></returns>
        public static explicit operator Color(System.Drawing.Color clr)
        {
            return Color.FromArgb(clr.ToArgb());
        }
        #endregion


        /// <summary>
        /// Converts this Color structure to a 32-bit integer in the format
        /// 0xAARRGGBB.  This is suitable for input to Color.FromArgb to 
        /// reproduce the color structure.
        /// </summary>
        /// <returns></returns>
        public int ToArgb()
        {
            return argb;

            /*
            int val = A;

            val <<= 8;
            val |= R;

            val <<= 8;
            val |= G;

            val <<= 8;
            val |= B;

            return val;
             * */
        }

    }
}