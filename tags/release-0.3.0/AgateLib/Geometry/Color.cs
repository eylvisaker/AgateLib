//     The contents of this file are subject to the Mozilla Public License
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

namespace AgateLib.Geometry
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
        /// Color AliceBlue.  0xf0f8ff
        /// </summary>
        public static Color AliceBlue { get { return Color.FromArgb(240, 248, 255); } }
        /// <summary>
        /// Color AntiqueWhite.  0xfaebd7
        /// </summary>
        public static Color AntiqueWhite { get { return Color.FromArgb(250, 235, 215); } }
        /// <summary>
        /// Color Aqua.  0x0ffff
        /// </summary>
        public static Color Aqua { get { return Color.FromArgb(0, 255, 255); } }
        /// <summary>
        /// Color Aquamarine.  0x7fffd4
        /// </summary>
        public static Color Aquamarine { get { return Color.FromArgb(127, 255, 212); } }
        /// <summary>
        /// Color Azure.  0xf0ffff
        /// </summary>
        public static Color Azure { get { return Color.FromArgb(240, 255, 255); } }
        /// <summary>
        /// Color Beige.  0xf5f5dc
        /// </summary>
        public static Color Beige { get { return Color.FromArgb(245, 245, 220); } }
        /// <summary>
        /// Color Bisque.  0xffe4c4
        /// </summary>
        public static Color Bisque { get { return Color.FromArgb(255, 228, 196); } }
        /// <summary>
        /// Color Black.  0x000
        /// </summary>
        public static Color Black { get { return Color.FromArgb(0, 0, 0); } }
        /// <summary>
        /// Color BlanchedAlmond.  0xffebcd
        /// </summary>
        public static Color BlanchedAlmond { get { return Color.FromArgb(255, 235, 205); } }
        /// <summary>
        /// Color Blue.  0x00ff
        /// </summary>
        public static Color Blue { get { return Color.FromArgb(0, 0, 255); } }
        /// <summary>
        /// Color BlueViolet.  0x8a2be2
        /// </summary>
        public static Color BlueViolet { get { return Color.FromArgb(138, 43, 226); } }
        /// <summary>
        /// Color Brown.  0xa52a2a
        /// </summary>
        public static Color Brown { get { return Color.FromArgb(165, 42, 42); } }
        /// <summary>
        /// Color BurlyWood.  0xdeb887
        /// </summary>
        public static Color BurlyWood { get { return Color.FromArgb(222, 184, 135); } }
        /// <summary>
        /// Color CadetBlue.  0x5f9ea0
        /// </summary>
        public static Color CadetBlue { get { return Color.FromArgb(95, 158, 160); } }
        /// <summary>
        /// Color Chartreuse.  0x7fff0
        /// </summary>
        public static Color Chartreuse { get { return Color.FromArgb(127, 255, 0); } }
        /// <summary>
        /// Color Chocolate.  0xd2691e
        /// </summary>
        public static Color Chocolate { get { return Color.FromArgb(210, 105, 30); } }
        /// <summary>
        /// Color Coral.  0xff7f50
        /// </summary>
        public static Color Coral { get { return Color.FromArgb(255, 127, 80); } }
        /// <summary>
        /// Color CornflowerBlue.  0x6495ed
        /// </summary>
        public static Color CornflowerBlue { get { return Color.FromArgb(100, 149, 237); } }
        /// <summary>
        /// Color Cornsilk.  0xfff8dc
        /// </summary>
        public static Color Cornsilk { get { return Color.FromArgb(255, 248, 220); } }
        /// <summary>
        /// Color Crimson.  0xdc143c
        /// </summary>
        public static Color Crimson { get { return Color.FromArgb(220, 20, 60); } }
        /// <summary>
        /// Color Cyan.  0x0ffff
        /// </summary>
        public static Color Cyan { get { return Color.FromArgb(0, 255, 255); } }
        /// <summary>
        /// Color DarkBlue.  0x008b
        /// </summary>
        public static Color DarkBlue { get { return Color.FromArgb(0, 0, 139); } }
        /// <summary>
        /// Color DarkCyan.  0x08b8b
        /// </summary>
        public static Color DarkCyan { get { return Color.FromArgb(0, 139, 139); } }
        /// <summary>
        /// Color DarkGoldenrod.  0xb886b
        /// </summary>
        public static Color DarkGoldenrod { get { return Color.FromArgb(184, 134, 11); } }
        /// <summary>
        /// Color DarkGray.  0xa9a9a9
        /// </summary>
        public static Color DarkGray { get { return Color.FromArgb(169, 169, 169); } }
        /// <summary>
        /// Color DarkGreen.  0x0640
        /// </summary>
        public static Color DarkGreen { get { return Color.FromArgb(0, 100, 0); } }
        /// <summary>
        /// Color DarkKhaki.  0xbdb76b
        /// </summary>
        public static Color DarkKhaki { get { return Color.FromArgb(189, 183, 107); } }
        /// <summary>
        /// Color DarkMagenta.  0x8b08b
        /// </summary>
        public static Color DarkMagenta { get { return Color.FromArgb(139, 0, 139); } }
        /// <summary>
        /// Color DarkOliveGreen.  0x556b2f
        /// </summary>
        public static Color DarkOliveGreen { get { return Color.FromArgb(85, 107, 47); } }
        /// <summary>
        /// Color DarkOrange.  0xff8c0
        /// </summary>
        public static Color DarkOrange { get { return Color.FromArgb(255, 140, 0); } }
        /// <summary>
        /// Color DarkOrchid.  0x9932cc
        /// </summary>
        public static Color DarkOrchid { get { return Color.FromArgb(153, 50, 204); } }
        /// <summary>
        /// Color DarkRed.  0x8b00
        /// </summary>
        public static Color DarkRed { get { return Color.FromArgb(139, 0, 0); } }
        /// <summary>
        /// Color DarkSalmon.  0xe9967a
        /// </summary>
        public static Color DarkSalmon { get { return Color.FromArgb(233, 150, 122); } }
        /// <summary>
        /// Color DarkSeaGreen.  0x8fbc8b
        /// </summary>
        public static Color DarkSeaGreen { get { return Color.FromArgb(143, 188, 139); } }
        /// <summary>
        /// Color DarkSlateBlue.  0x483d8b
        /// </summary>
        public static Color DarkSlateBlue { get { return Color.FromArgb(72, 61, 139); } }
        /// <summary>
        /// Color DarkSlateGray.  0x2f4f4f
        /// </summary>
        public static Color DarkSlateGray { get { return Color.FromArgb(47, 79, 79); } }
        /// <summary>
        /// Color DarkTurquoise.  0x0ced1
        /// </summary>
        public static Color DarkTurquoise { get { return Color.FromArgb(0, 206, 209); } }
        /// <summary>
        /// Color DarkViolet.  0x940d3
        /// </summary>
        public static Color DarkViolet { get { return Color.FromArgb(148, 0, 211); } }
        /// <summary>
        /// Color DeepPink.  0xff1493
        /// </summary>
        public static Color DeepPink { get { return Color.FromArgb(255, 20, 147); } }
        /// <summary>
        /// Color DeepSkyBlue.  0x0bfff
        /// </summary>
        public static Color DeepSkyBlue { get { return Color.FromArgb(0, 191, 255); } }
        /// <summary>
        /// Color DimGray.  0x696969
        /// </summary>
        public static Color DimGray { get { return Color.FromArgb(105, 105, 105); } }
        /// <summary>
        /// Color DodgerBlue.  0x1e90ff
        /// </summary>
        public static Color DodgerBlue { get { return Color.FromArgb(30, 144, 255); } }
        /// <summary>
        /// Color Firebrick.  0xb22222
        /// </summary>
        public static Color Firebrick { get { return Color.FromArgb(178, 34, 34); } }
        /// <summary>
        /// Color FloralWhite.  0xfffaf0
        /// </summary>
        public static Color FloralWhite { get { return Color.FromArgb(255, 250, 240); } }
        /// <summary>
        /// Color ForestGreen.  0x228b22
        /// </summary>
        public static Color ForestGreen { get { return Color.FromArgb(34, 139, 34); } }
        /// <summary>
        /// Color Fuchsia.  0xff0ff
        /// </summary>
        public static Color Fuchsia { get { return Color.FromArgb(255, 0, 255); } }
        /// <summary>
        /// Color Gainsboro.  0xdcdcdc
        /// </summary>
        public static Color Gainsboro { get { return Color.FromArgb(220, 220, 220); } }
        /// <summary>
        /// Color GhostWhite.  0xf8f8ff
        /// </summary>
        public static Color GhostWhite { get { return Color.FromArgb(248, 248, 255); } }
        /// <summary>
        /// Color Gold.  0xffd70
        /// </summary>
        public static Color Gold { get { return Color.FromArgb(255, 215, 0); } }
        /// <summary>
        /// Color Goldenrod.  0xdaa520
        /// </summary>
        public static Color Goldenrod { get { return Color.FromArgb(218, 165, 32); } }
        /// <summary>
        /// Color Gray.  0x808080
        /// </summary>
        public static Color Gray { get { return Color.FromArgb(128, 128, 128); } }
        /// <summary>
        /// Color Green.  0x0800
        /// </summary>
        public static Color Green { get { return Color.FromArgb(0, 128, 0); } }
        /// <summary>
        /// Color GreenYellow.  0xadff2f
        /// </summary>
        public static Color GreenYellow { get { return Color.FromArgb(173, 255, 47); } }
        /// <summary>
        /// Color Honeydew.  0xf0fff0
        /// </summary>
        public static Color Honeydew { get { return Color.FromArgb(240, 255, 240); } }
        /// <summary>
        /// Color HotPink.  0xff69b4
        /// </summary>
        public static Color HotPink { get { return Color.FromArgb(255, 105, 180); } }
        /// <summary>
        /// Color IndianRed.  0xcd5c5c
        /// </summary>
        public static Color IndianRed { get { return Color.FromArgb(205, 92, 92); } }
        /// <summary>
        /// Color Indigo.  0x4b082
        /// </summary>
        public static Color Indigo { get { return Color.FromArgb(75, 0, 130); } }
        /// <summary>
        /// Color Ivory.  0xfffff0
        /// </summary>
        public static Color Ivory { get { return Color.FromArgb(255, 255, 240); } }
        /// <summary>
        /// Color Khaki.  0xf0e68c
        /// </summary>
        public static Color Khaki { get { return Color.FromArgb(240, 230, 140); } }
        /// <summary>
        /// Color Lavender.  0xe6e6fa
        /// </summary>
        public static Color Lavender { get { return Color.FromArgb(230, 230, 250); } }
        /// <summary>
        /// Color LavenderBlush.  0xfff0f5
        /// </summary>
        public static Color LavenderBlush { get { return Color.FromArgb(255, 240, 245); } }
        /// <summary>
        /// Color LawnGreen.  0x7cfc0
        /// </summary>
        public static Color LawnGreen { get { return Color.FromArgb(124, 252, 0); } }
        /// <summary>
        /// Color LemonChiffon.  0xfffacd
        /// </summary>
        public static Color LemonChiffon { get { return Color.FromArgb(255, 250, 205); } }
        /// <summary>
        /// Color LightBlue.  0xadd8e6
        /// </summary>
        public static Color LightBlue { get { return Color.FromArgb(173, 216, 230); } }
        /// <summary>
        /// Color LightCoral.  0xf08080
        /// </summary>
        public static Color LightCoral { get { return Color.FromArgb(240, 128, 128); } }
        /// <summary>
        /// Color LightCyan.  0xe0ffff
        /// </summary>
        public static Color LightCyan { get { return Color.FromArgb(224, 255, 255); } }
        /// <summary>
        /// Color LightGoldenrodYellow.  0xfafad2
        /// </summary>
        public static Color LightGoldenrodYellow { get { return Color.FromArgb(250, 250, 210); } }
        /// <summary>
        /// Color LightGray.  0xd3d3d3
        /// </summary>
        public static Color LightGray { get { return Color.FromArgb(211, 211, 211); } }
        /// <summary>
        /// Color LightGreen.  0x90ee90
        /// </summary>
        public static Color LightGreen { get { return Color.FromArgb(144, 238, 144); } }
        /// <summary>
        /// Color LightPink.  0xffb6c1
        /// </summary>
        public static Color LightPink { get { return Color.FromArgb(255, 182, 193); } }
        /// <summary>
        /// Color LightSalmon.  0xffa07a
        /// </summary>
        public static Color LightSalmon { get { return Color.FromArgb(255, 160, 122); } }
        /// <summary>
        /// Color LightSeaGreen.  0x20b2aa
        /// </summary>
        public static Color LightSeaGreen { get { return Color.FromArgb(32, 178, 170); } }
        /// <summary>
        /// Color LightSkyBlue.  0x87cefa
        /// </summary>
        public static Color LightSkyBlue { get { return Color.FromArgb(135, 206, 250); } }
        /// <summary>
        /// Color LightSlateGray.  0x778899
        /// </summary>
        public static Color LightSlateGray { get { return Color.FromArgb(119, 136, 153); } }
        /// <summary>
        /// Color LightSteelBlue.  0xb0c4de
        /// </summary>
        public static Color LightSteelBlue { get { return Color.FromArgb(176, 196, 222); } }
        /// <summary>
        /// Color LightYellow.  0xffffe0
        /// </summary>
        public static Color LightYellow { get { return Color.FromArgb(255, 255, 224); } }
        /// <summary>
        /// Color Lime.  0x0ff0
        /// </summary>
        public static Color Lime { get { return Color.FromArgb(0, 255, 0); } }
        /// <summary>
        /// Color LimeGreen.  0x32cd32
        /// </summary>
        public static Color LimeGreen { get { return Color.FromArgb(50, 205, 50); } }
        /// <summary>
        /// Color Linen.  0xfaf0e6
        /// </summary>
        public static Color Linen { get { return Color.FromArgb(250, 240, 230); } }
        /// <summary>
        /// Color Magenta.  0xff0ff
        /// </summary>
        public static Color Magenta { get { return Color.FromArgb(255, 0, 255); } }
        /// <summary>
        /// Color Maroon.  0x8000
        /// </summary>
        public static Color Maroon { get { return Color.FromArgb(128, 0, 0); } }
        /// <summary>
        /// Color MediumAquamarine.  0x66cdaa
        /// </summary>
        public static Color MediumAquamarine { get { return Color.FromArgb(102, 205, 170); } }
        /// <summary>
        /// Color MediumBlue.  0x00cd
        /// </summary>
        public static Color MediumBlue { get { return Color.FromArgb(0, 0, 205); } }
        /// <summary>
        /// Color MediumOrchid.  0xba55d3
        /// </summary>
        public static Color MediumOrchid { get { return Color.FromArgb(186, 85, 211); } }
        /// <summary>
        /// Color MediumPurple.  0x9370db
        /// </summary>
        public static Color MediumPurple { get { return Color.FromArgb(147, 112, 219); } }
        /// <summary>
        /// Color MediumSeaGreen.  0x3cb371
        /// </summary>
        public static Color MediumSeaGreen { get { return Color.FromArgb(60, 179, 113); } }
        /// <summary>
        /// Color MediumSlateBlue.  0x7b68ee
        /// </summary>
        public static Color MediumSlateBlue { get { return Color.FromArgb(123, 104, 238); } }
        /// <summary>
        /// Color MediumSpringGreen.  0x0fa9a
        /// </summary>
        public static Color MediumSpringGreen { get { return Color.FromArgb(0, 250, 154); } }
        /// <summary>
        /// Color MediumTurquoise.  0x48d1cc
        /// </summary>
        public static Color MediumTurquoise { get { return Color.FromArgb(72, 209, 204); } }
        /// <summary>
        /// Color MediumVioletRed.  0xc71585
        /// </summary>
        public static Color MediumVioletRed { get { return Color.FromArgb(199, 21, 133); } }
        /// <summary>
        /// Color MidnightBlue.  0x191970
        /// </summary>
        public static Color MidnightBlue { get { return Color.FromArgb(25, 25, 112); } }
        /// <summary>
        /// Color MintCream.  0xf5fffa
        /// </summary>
        public static Color MintCream { get { return Color.FromArgb(245, 255, 250); } }
        /// <summary>
        /// Color MistyRose.  0xffe4e1
        /// </summary>
        public static Color MistyRose { get { return Color.FromArgb(255, 228, 225); } }
        /// <summary>
        /// Color Moccasin.  0xffe4b5
        /// </summary>
        public static Color Moccasin { get { return Color.FromArgb(255, 228, 181); } }
        /// <summary>
        /// Color NavajoWhite.  0xffdead
        /// </summary>
        public static Color NavajoWhite { get { return Color.FromArgb(255, 222, 173); } }
        /// <summary>
        /// Color Navy.  0x0080
        /// </summary>
        public static Color Navy { get { return Color.FromArgb(0, 0, 128); } }
        /// <summary>
        /// Color OldLace.  0xfdf5e6
        /// </summary>
        public static Color OldLace { get { return Color.FromArgb(253, 245, 230); } }
        /// <summary>
        /// Color Olive.  0x80800
        /// </summary>
        public static Color Olive { get { return Color.FromArgb(128, 128, 0); } }
        /// <summary>
        /// Color OliveDrab.  0x6b8e23
        /// </summary>
        public static Color OliveDrab { get { return Color.FromArgb(107, 142, 35); } }
        /// <summary>
        /// Color Orange.  0xffa50
        /// </summary>
        public static Color Orange { get { return Color.FromArgb(255, 165, 0); } }
        /// <summary>
        /// Color OrangeRed.  0xff450
        /// </summary>
        public static Color OrangeRed { get { return Color.FromArgb(255, 69, 0); } }
        /// <summary>
        /// Color Orchid.  0xda70d6
        /// </summary>
        public static Color Orchid { get { return Color.FromArgb(218, 112, 214); } }
        /// <summary>
        /// Color PaleGoldenrod.  0xeee8aa
        /// </summary>
        public static Color PaleGoldenrod { get { return Color.FromArgb(238, 232, 170); } }
        /// <summary>
        /// Color PaleGreen.  0x98fb98
        /// </summary>
        public static Color PaleGreen { get { return Color.FromArgb(152, 251, 152); } }
        /// <summary>
        /// Color PaleTurquoise.  0xafeeee
        /// </summary>
        public static Color PaleTurquoise { get { return Color.FromArgb(175, 238, 238); } }
        /// <summary>
        /// Color PaleVioletRed.  0xdb7093
        /// </summary>
        public static Color PaleVioletRed { get { return Color.FromArgb(219, 112, 147); } }
        /// <summary>
        /// Color PapayaWhip.  0xffefd5
        /// </summary>
        public static Color PapayaWhip { get { return Color.FromArgb(255, 239, 213); } }
        /// <summary>
        /// Color PeachPuff.  0xffdab9
        /// </summary>
        public static Color PeachPuff { get { return Color.FromArgb(255, 218, 185); } }
        /// <summary>
        /// Color Peru.  0xcd853f
        /// </summary>
        public static Color Peru { get { return Color.FromArgb(205, 133, 63); } }
        /// <summary>
        /// Color Pink.  0xffc0cb
        /// </summary>
        public static Color Pink { get { return Color.FromArgb(255, 192, 203); } }
        /// <summary>
        /// Color Plum.  0xdda0dd
        /// </summary>
        public static Color Plum { get { return Color.FromArgb(221, 160, 221); } }
        /// <summary>
        /// Color PowderBlue.  0xb0e0e6
        /// </summary>
        public static Color PowderBlue { get { return Color.FromArgb(176, 224, 230); } }
        /// <summary>
        /// Color Purple.  0x80080
        /// </summary>
        public static Color Purple { get { return Color.FromArgb(128, 0, 128); } }
        /// <summary>
        /// Color Red.  0xff00
        /// </summary>
        public static Color Red { get { return Color.FromArgb(255, 0, 0); } }
        /// <summary>
        /// Color RosyBrown.  0xbc8f8f
        /// </summary>
        public static Color RosyBrown { get { return Color.FromArgb(188, 143, 143); } }
        /// <summary>
        /// Color RoyalBlue.  0x4169e1
        /// </summary>
        public static Color RoyalBlue { get { return Color.FromArgb(65, 105, 225); } }
        /// <summary>
        /// Color SaddleBrown.  0x8b4513
        /// </summary>
        public static Color SaddleBrown { get { return Color.FromArgb(139, 69, 19); } }
        /// <summary>
        /// Color Salmon.  0xfa8072
        /// </summary>
        public static Color Salmon { get { return Color.FromArgb(250, 128, 114); } }
        /// <summary>
        /// Color SandyBrown.  0xf4a460
        /// </summary>
        public static Color SandyBrown { get { return Color.FromArgb(244, 164, 96); } }
        /// <summary>
        /// Color SeaGreen.  0x2e8b57
        /// </summary>
        public static Color SeaGreen { get { return Color.FromArgb(46, 139, 87); } }
        /// <summary>
        /// Color SeaShell.  0xfff5ee
        /// </summary>
        public static Color SeaShell { get { return Color.FromArgb(255, 245, 238); } }
        /// <summary>
        /// Color Sienna.  0xa0522d
        /// </summary>
        public static Color Sienna { get { return Color.FromArgb(160, 82, 45); } }
        /// <summary>
        /// Color Silver.  0xc0c0c0
        /// </summary>
        public static Color Silver { get { return Color.FromArgb(192, 192, 192); } }
        /// <summary>
        /// Color SkyBlue.  0x87ceeb
        /// </summary>
        public static Color SkyBlue { get { return Color.FromArgb(135, 206, 235); } }
        /// <summary>
        /// Color SlateBlue.  0x6a5acd
        /// </summary>
        public static Color SlateBlue { get { return Color.FromArgb(106, 90, 205); } }
        /// <summary>
        /// Color SlateGray.  0x708090
        /// </summary>
        public static Color SlateGray { get { return Color.FromArgb(112, 128, 144); } }
        /// <summary>
        /// Color Snow.  0xfffafa
        /// </summary>
        public static Color Snow { get { return Color.FromArgb(255, 250, 250); } }
        /// <summary>
        /// Color SpringGreen.  0x0ff7f
        /// </summary>
        public static Color SpringGreen { get { return Color.FromArgb(0, 255, 127); } }
        /// <summary>
        /// Color SteelBlue.  0x4682b4
        /// </summary>
        public static Color SteelBlue { get { return Color.FromArgb(70, 130, 180); } }
        /// <summary>
        /// Color Tan.  0xd2b48c
        /// </summary>
        public static Color Tan { get { return Color.FromArgb(210, 180, 140); } }
        /// <summary>
        /// Color Teal.  0x08080
        /// </summary>
        public static Color Teal { get { return Color.FromArgb(0, 128, 128); } }
        /// <summary>
        /// Color Thistle.  0xd8bfd8
        /// </summary>
        public static Color Thistle { get { return Color.FromArgb(216, 191, 216); } }
        /// <summary>
        /// Color Tomato.  0xff6347
        /// </summary>
        public static Color Tomato { get { return Color.FromArgb(255, 99, 71); } }
        /// <summary>
        /// Color Transparent.  0xffffff
        /// </summary>
        public static Color Transparent { get { return Color.FromArgb(255, 255, 255); } }
        /// <summary>
        /// Color Turquoise.  0x40e0d0
        /// </summary>
        public static Color Turquoise { get { return Color.FromArgb(64, 224, 208); } }
        /// <summary>
        /// Color Violet.  0xee82ee
        /// </summary>
        public static Color Violet { get { return Color.FromArgb(238, 130, 238); } }
        /// <summary>
        /// Color Wheat.  0xf5deb3
        /// </summary>
        public static Color Wheat { get { return Color.FromArgb(245, 222, 179); } }
        /// <summary>
        /// Color White.  0xffffff
        /// </summary>
        public static Color White { get { return Color.FromArgb(255, 255, 255); } }
        /// <summary>
        /// Color WhiteSmoke.  0xf5f5f5
        /// </summary>
        public static Color WhiteSmoke { get { return Color.FromArgb(245, 245, 245); } }
        /// <summary>
        /// Color Yellow.  0xffff0
        /// </summary>
        public static Color Yellow { get { return Color.FromArgb(255, 255, 0); } }
        /// <summary>
        /// Color YellowGreen.  0x9acd32
        /// </summary>
        public static Color YellowGreen { get { return Color.FromArgb(154, 205, 50); } }

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
        /*
        private Color(System.Drawing.Color clr)
            : this(clr.A, clr.R, clr.G, clr.B)
        {
        }
        */
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


        /// <summary>
        /// Converts a string to an AgateLib.Geometry.Color structure.
        /// </summary>
        /// <param name="str">The string to convert.  It must be in one of the following formats
        /// RRGGBB, AARRGGBB, 0xRRGGBB, 0xAARRGGBB where AA, RR, GG, BB are each a hexidecimal
        /// number (such as "ff" or "8B").</param>
        /// <returns></returns>
        public static Color FromArgb(string str)
        {
            if (str.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                str = str.Substring(2);

            if (str.Length == 6)
            {
                byte r = ByteValueFromHex(str.Substring(0, 2));
                byte g = ByteValueFromHex(str.Substring(2, 2));
                byte b = ByteValueFromHex(str.Substring(4, 2));

                return FromArgb(r, g, b);
            }
            else if (str.Length == 8)
            {
                byte a = ByteValueFromHex(str.Substring(0, 2));
                byte r = ByteValueFromHex(str.Substring(2, 2));
                byte g = ByteValueFromHex(str.Substring(4, 2));
                byte b = ByteValueFromHex(str.Substring(6, 2));

                return FromArgb(a, r, g, b);
            }
            else
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                    "Argument \"{0}\" is not a valid Color string.", str));
        }

        /// <summary>
        /// Converts a string like "FF" to a byte value.  Throws an exception if the
        /// string does not convert to a value which fits into a byte.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static byte ByteValueFromHex(string hex)
        {
            int value;

            if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out value))
            {
                if (value > 255 || value < 0)
                    throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                        "Invalid result.  Input Hex number: {0}, Result: {1}", hex, value));

                return (byte)value;
            }
            else
                throw new ArgumentException("Not a hex number.");
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
            return "Color: " + ToArgb().ToString("X", System.Globalization.CultureInfo.CurrentCulture);
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
        [Obsolete("Features moved to AgateWinForms.dll")]
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
        [Obsolete("Features moved to AgateWinForms.dll")]
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