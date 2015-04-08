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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace AgateLib.Geometry
{
	/// <summary>
	/// Replacement for System.Drawing.Color structure.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	[DataContract]
	public struct Color
	{
		#region --- Static Named Color properties ---

		/// <summary>
		/// Color AliceBlue.  0xf0f8ff
		/// </summary>
		public static readonly Color AliceBlue = Color.FromArgb(240, 248, 255);
		/// <summary>
		/// Color AntiqueWhite.  0xfaebd7
		/// </summary>
		public static readonly Color AntiqueWhite = Color.FromArgb(250, 235, 215);
		/// <summary>
		/// Color Aqua.  0x00ffff
		/// </summary>
		public static readonly Color Aqua = Color.FromArgb(0, 255, 255);
		/// <summary>
		/// Color Aquamarine.  0x7fffd4
		/// </summary>
		public static readonly Color Aquamarine = Color.FromArgb(127, 255, 212);
		/// <summary>
		/// Color Azure.  0xf0ffff
		/// </summary>
		public static readonly Color Azure = Color.FromArgb(240, 255, 255);
		/// <summary>
		/// Color Beige.  0xf5f5dc
		/// </summary>
		public static readonly Color Beige = Color.FromArgb(245, 245, 220);
		/// <summary>
		/// Color Bisque.  0xffe4c4
		/// </summary>
		public static readonly Color Bisque = Color.FromArgb(255, 228, 196);
		/// <summary>
		/// Color Black.  0x000000
		/// </summary>
		public static readonly Color Black = Color.FromArgb(0, 0, 0);
		/// <summary>
		/// Color BlanchedAlmond.  0xffebcd
		/// </summary>
		public static readonly Color BlanchedAlmond = Color.FromArgb(255, 235, 205);
		/// <summary>
		/// Color Blue.  0x0000ff
		/// </summary>
		public static readonly Color Blue = Color.FromArgb(0, 0, 255);
		/// <summary>
		/// Color BlueViolet.  0x8a2be2
		/// </summary>
		public static readonly Color BlueViolet = Color.FromArgb(138, 43, 226);
		/// <summary>
		/// Color Brown.  0xa52a2a
		/// </summary>
		public static readonly Color Brown = Color.FromArgb(165, 42, 42);
		/// <summary>
		/// Color BurlyWood.  0xdeb887
		/// </summary>
		public static readonly Color BurlyWood = Color.FromArgb(222, 184, 135);
		/// <summary>
		/// Color CadetBlue.  0x5f9ea0
		/// </summary>
		public static readonly Color CadetBlue = Color.FromArgb(95, 158, 160);
		/// <summary>
		/// Color Chartreuse.  0x7fff00
		/// </summary>
		public static readonly Color Chartreuse = Color.FromArgb(127, 255, 0);
		/// <summary>
		/// Color Chocolate.  0xd2691e
		/// </summary>
		public static readonly Color Chocolate = Color.FromArgb(210, 105, 30);
		/// <summary>
		/// Color Coral.  0xff7f50
		/// </summary>
		public static readonly Color Coral = Color.FromArgb(255, 127, 80);
		/// <summary>
		/// Color CornflowerBlue.  0x6495ed
		/// </summary>
		public static readonly Color CornflowerBlue = Color.FromArgb(100, 149, 237);
		/// <summary>
		/// Color Cornsilk.  0xfff8dc
		/// </summary>
		public static readonly Color Cornsilk = Color.FromArgb(255, 248, 220);
		/// <summary>
		/// Color Crimson.  0xdc143c
		/// </summary>
		public static readonly Color Crimson = Color.FromArgb(220, 20, 60);
		/// <summary>
		/// Color Cyan.  0x00ffff
		/// </summary>
		public static readonly Color Cyan = Color.FromArgb(0, 255, 255);
		/// <summary>
		/// Color DarkBlue.  0x00008b
		/// </summary>
		public static readonly Color DarkBlue = Color.FromArgb(0, 0, 139);
		/// <summary>
		/// Color DarkCyan.  0x008b8b
		/// </summary>
		public static readonly Color DarkCyan = Color.FromArgb(0, 139, 139);
		/// <summary>
		/// Color DarkGoldenrod.  0xb886b
		/// </summary>
		public static readonly Color DarkGoldenrod = Color.FromArgb(184, 134, 11);
		/// <summary>
		/// Color DarkGray.  0xa9a9a9
		/// </summary>
		public static readonly Color DarkGray = Color.FromArgb(169, 169, 169);
		/// <summary>
		/// Color DarkGreen.  0x006400
		/// </summary>
		public static readonly Color DarkGreen = Color.FromArgb(0, 100, 0);
		/// <summary>
		/// Color DarkKhaki.  0xbdb76b
		/// </summary>
		public static readonly Color DarkKhaki = Color.FromArgb(189, 183, 107);
		/// <summary>
		/// Color DarkMagenta.  0x8b008b
		/// </summary>
		public static readonly Color DarkMagenta = Color.FromArgb(139, 0, 139);
		/// <summary>
		/// Color DarkOliveGreen.  0x556b2f
		/// </summary>
		public static readonly Color DarkOliveGreen = Color.FromArgb(85, 107, 47);
		/// <summary>
		/// Color DarkOrange.  0xff8c00
		/// </summary>
		public static readonly Color DarkOrange = Color.FromArgb(255, 140, 0);
		/// <summary>
		/// Color DarkOrchid.  0x9932cc
		/// </summary>
		public static readonly Color DarkOrchid = Color.FromArgb(153, 50, 204);
		/// <summary>
		/// Color DarkRed.  0x8b0000
		/// </summary>
		public static readonly Color DarkRed = Color.FromArgb(139, 0, 0);
		/// <summary>
		/// Color DarkSalmon.  0xe9967a
		/// </summary>
		public static readonly Color DarkSalmon = Color.FromArgb(233, 150, 122);
		/// <summary>
		/// Color DarkSeaGreen.  0x8fbc8b
		/// </summary>
		public static readonly Color DarkSeaGreen = Color.FromArgb(143, 188, 139);
		/// <summary>
		/// Color DarkSlateBlue.  0x483d8b
		/// </summary>
		public static readonly Color DarkSlateBlue = Color.FromArgb(72, 61, 139);
		/// <summary>
		/// Color DarkSlateGray.  0x2f4f4f
		/// </summary>
		public static readonly Color DarkSlateGray = Color.FromArgb(47, 79, 79);
		/// <summary>
		/// Color DarkTurquoise.  0x00ced1
		/// </summary>
		public static readonly Color DarkTurquoise = Color.FromArgb(0, 206, 209);
		/// <summary>
		/// Color DarkViolet.  0x9400d3
		/// </summary>
		public static readonly Color DarkViolet = Color.FromArgb(148, 0, 211);
		/// <summary>
		/// Color DeepPink.  0xff1493
		/// </summary>
		public static readonly Color DeepPink = Color.FromArgb(255, 20, 147);
		/// <summary>
		/// Color DeepSkyBlue.  0x00bfff
		/// </summary>
		public static readonly Color DeepSkyBlue = Color.FromArgb(0, 191, 255);
		/// <summary>
		/// Color DimGray.  0x696969
		/// </summary>
		public static readonly Color DimGray = Color.FromArgb(105, 105, 105);
		/// <summary>
		/// Color DodgerBlue.  0x1e90ff
		/// </summary>
		public static readonly Color DodgerBlue = Color.FromArgb(30, 144, 255);
		/// <summary>
		/// Color Firebrick.  0xb22222
		/// </summary>
		public static readonly Color Firebrick = Color.FromArgb(178, 34, 34);
		/// <summary>
		/// Color FloralWhite.  0xfffaf0
		/// </summary>
		public static readonly Color FloralWhite = Color.FromArgb(255, 250, 240);
		/// <summary>
		/// Color ForestGreen.  0x228b22
		/// </summary>
		public static readonly Color ForestGreen = Color.FromArgb(34, 139, 34);
		/// <summary>
		/// Color Fuchsia.  0xff00ff
		/// </summary>
		public static readonly Color Fuchsia = Color.FromArgb(255, 0, 255);
		/// <summary>
		/// Color Gainsboro.  0xdcdcdc
		/// </summary>
		public static readonly Color Gainsboro = Color.FromArgb(220, 220, 220);
		/// <summary>
		/// Color GhostWhite.  0xf8f8ff
		/// </summary>
		public static readonly Color GhostWhite = Color.FromArgb(248, 248, 255);
		/// <summary>
		/// Color Gold.  0xffd700
		/// </summary>
		public static readonly Color Gold = Color.FromArgb(255, 215, 0);
		/// <summary>
		/// Color Goldenrod.  0xdaa520
		/// </summary>
		public static readonly Color Goldenrod = Color.FromArgb(218, 165, 32);
		/// <summary>
		/// Color Gray.  0x808080
		/// </summary>
		public static readonly Color Gray = Color.FromArgb(128, 128, 128);
		/// <summary>
		/// Color Green.  0x008000
		/// </summary>
		public static readonly Color Green = Color.FromArgb(0, 128, 0);
		/// <summary>
		/// Color GreenYellow.  0xadff2f
		/// </summary>
		public static readonly Color GreenYellow = Color.FromArgb(173, 255, 47);
		/// <summary>
		/// Color Honeydew.  0xf0fff0
		/// </summary>
		public static readonly Color Honeydew = Color.FromArgb(240, 255, 240);
		/// <summary>
		/// Color HotPink.  0xff69b4
		/// </summary>
		public static readonly Color HotPink = Color.FromArgb(255, 105, 180);
		/// <summary>
		/// Color IndianRed.  0xcd5c5c
		/// </summary>
		public static readonly Color IndianRed = Color.FromArgb(205, 92, 92);
		/// <summary>
		/// Color Indigo.  0x4b0082
		/// </summary>
		public static readonly Color Indigo = Color.FromArgb(75, 0, 130);
		/// <summary>
		/// Color Ivory.  0xfffff0
		/// </summary>
		public static readonly Color Ivory = Color.FromArgb(255, 255, 240);
		/// <summary>
		/// Color Khaki.  0xf0e68c
		/// </summary>
		public static readonly Color Khaki = Color.FromArgb(240, 230, 140);
		/// <summary>
		/// Color Lavender.  0xe6e6fa
		/// </summary>
		public static readonly Color Lavender = Color.FromArgb(230, 230, 250);
		/// <summary>
		/// Color LavenderBlush.  0xfff0f5
		/// </summary>
		public static readonly Color LavenderBlush = Color.FromArgb(255, 240, 245);
		/// <summary>
		/// Color LawnGreen.  0x7cfc00
		/// </summary>
		public static readonly Color LawnGreen = Color.FromArgb(124, 252, 0);
		/// <summary>
		/// Color LemonChiffon.  0xfffacd
		/// </summary>
		public static readonly Color LemonChiffon = Color.FromArgb(255, 250, 205);
		/// <summary>
		/// Color LightBlue.  0xadd8e6
		/// </summary>
		public static readonly Color LightBlue = Color.FromArgb(173, 216, 230);
		/// <summary>
		/// Color LightCoral.  0xf08080
		/// </summary>
		public static readonly Color LightCoral = Color.FromArgb(240, 128, 128);
		/// <summary>
		/// Color LightCyan.  0xe0ffff
		/// </summary>
		public static readonly Color LightCyan = Color.FromArgb(224, 255, 255);
		/// <summary>
		/// Color LightGoldenrodYellow.  0xfafad2
		/// </summary>
		public static readonly Color LightGoldenrodYellow = Color.FromArgb(250, 250, 210);
		/// <summary>
		/// Color LightGray.  0xd3d3d3
		/// </summary>
		public static readonly Color LightGray = Color.FromArgb(211, 211, 211);
		/// <summary>
		/// Color LightGreen.  0x90ee90
		/// </summary>
		public static readonly Color LightGreen = Color.FromArgb(144, 238, 144);
		/// <summary>
		/// Color LightPink.  0xffb6c1
		/// </summary>
		public static readonly Color LightPink = Color.FromArgb(255, 182, 193);
		/// <summary>
		/// Color LightSalmon.  0xffa07a
		/// </summary>
		public static readonly Color LightSalmon = Color.FromArgb(255, 160, 122);
		/// <summary>
		/// Color LightSeaGreen.  0x20b2aa
		/// </summary>
		public static readonly Color LightSeaGreen = Color.FromArgb(32, 178, 170);
		/// <summary>
		/// Color LightSkyBlue.  0x87cefa
		/// </summary>
		public static readonly Color LightSkyBlue = Color.FromArgb(135, 206, 250);
		/// <summary>
		/// Color LightSlateGray.  0x778899
		/// </summary>
		public static readonly Color LightSlateGray = Color.FromArgb(119, 136, 153);
		/// <summary>
		/// Color LightSteelBlue.  0xb0c4de
		/// </summary>
		public static readonly Color LightSteelBlue = Color.FromArgb(176, 196, 222);
		/// <summary>
		/// Color LightYellow.  0xffffe0
		/// </summary>
		public static readonly Color LightYellow = Color.FromArgb(255, 255, 224);
		/// <summary>
		/// Color Lime.  0x00ff00
		/// </summary>
		public static readonly Color Lime = Color.FromArgb(0, 255, 0);
		/// <summary>
		/// Color LimeGreen.  0x32cd32
		/// </summary>
		public static readonly Color LimeGreen = Color.FromArgb(50, 205, 50);
		/// <summary>
		/// Color Linen.  0xfaf0e6
		/// </summary>
		public static readonly Color Linen = Color.FromArgb(250, 240, 230);
		/// <summary>
		/// Color Magenta.  0xff00ff
		/// </summary>
		public static readonly Color Magenta = Color.FromArgb(255, 0, 255);
		/// <summary>
		/// Color Maroon.  0x800000
		/// </summary>
		public static readonly Color Maroon = Color.FromArgb(128, 0, 0);
		/// <summary>
		/// Color MediumAquamarine.  0x66cdaa
		/// </summary>
		public static readonly Color MediumAquamarine = Color.FromArgb(102, 205, 170);
		/// <summary>
		/// Color MediumBlue.  0x0000cd
		/// </summary>
		public static readonly Color MediumBlue = Color.FromArgb(0, 0, 205);
		/// <summary>
		/// Color MediumOrchid.  0xba55d3
		/// </summary>
		public static readonly Color MediumOrchid = Color.FromArgb(186, 85, 211);
		/// <summary>
		/// Color MediumPurple.  0x9370db
		/// </summary>
		public static readonly Color MediumPurple = Color.FromArgb(147, 112, 219);
		/// <summary>
		/// Color MediumSeaGreen.  0x3cb371
		/// </summary>
		public static readonly Color MediumSeaGreen = Color.FromArgb(60, 179, 113);
		/// <summary>
		/// Color MediumSlateBlue.  0x7b68ee
		/// </summary>
		public static readonly Color MediumSlateBlue = Color.FromArgb(123, 104, 238);
		/// <summary>
		/// Color MediumSpringGreen.  0x00fa9a
		/// </summary>
		public static readonly Color MediumSpringGreen = Color.FromArgb(0, 250, 154);
		/// <summary>
		/// Color MediumTurquoise.  0x48d1cc
		/// </summary>
		public static readonly Color MediumTurquoise = Color.FromArgb(72, 209, 204);
		/// <summary>
		/// Color MediumVioletRed.  0xc71585
		/// </summary>
		public static readonly Color MediumVioletRed = Color.FromArgb(199, 21, 133);
		/// <summary>
		/// Color MidnightBlue.  0x191970
		/// </summary>
		public static readonly Color MidnightBlue = Color.FromArgb(25, 25, 112);
		/// <summary>
		/// Color MintCream.  0xf5fffa
		/// </summary>
		public static readonly Color MintCream = Color.FromArgb(245, 255, 250);
		/// <summary>
		/// Color MistyRose.  0xffe4e1
		/// </summary>
		public static readonly Color MistyRose = Color.FromArgb(255, 228, 225);
		/// <summary>
		/// Color Moccasin.  0xffe4b5
		/// </summary>
		public static readonly Color Moccasin = Color.FromArgb(255, 228, 181);
		/// <summary>
		/// Color NavajoWhite.  0xffdead
		/// </summary>
		public static readonly Color NavajoWhite = Color.FromArgb(255, 222, 173);
		/// <summary>
		/// Color Navy.  0x000080
		/// </summary>
		public static readonly Color Navy = Color.FromArgb(0, 0, 128);
		/// <summary>
		/// Color OldLace.  0xfdf5e6
		/// </summary>
		public static readonly Color OldLace = Color.FromArgb(253, 245, 230);
		/// <summary>
		/// Color Olive.  0x808000
		/// </summary>
		public static readonly Color Olive = Color.FromArgb(128, 128, 0);
		/// <summary>
		/// Color OliveDrab.  0x6b8e23
		/// </summary>
		public static readonly Color OliveDrab = Color.FromArgb(107, 142, 35);
		/// <summary>
		/// Color Orange.  0xffa500
		/// </summary>
		public static readonly Color Orange = Color.FromArgb(255, 165, 0);
		/// <summary>
		/// Color OrangeRed.  0xff4500
		/// </summary>
		public static readonly Color OrangeRed = Color.FromArgb(255, 69, 0);
		/// <summary>
		/// Color Orchid.  0xda70d6
		/// </summary>
		public static readonly Color Orchid = Color.FromArgb(218, 112, 214);
		/// <summary>
		/// Color PaleGoldenrod.  0xeee8aa
		/// </summary>
		public static readonly Color PaleGoldenrod = Color.FromArgb(238, 232, 170);
		/// <summary>
		/// Color PaleGreen.  0x98fb98
		/// </summary>
		public static readonly Color PaleGreen = Color.FromArgb(152, 251, 152);
		/// <summary>
		/// Color PaleTurquoise.  0xafeeee
		/// </summary>
		public static readonly Color PaleTurquoise = Color.FromArgb(175, 238, 238);
		/// <summary>
		/// Color PaleVioletRed.  0xdb7093
		/// </summary>
		public static readonly Color PaleVioletRed = Color.FromArgb(219, 112, 147);
		/// <summary>
		/// Color PapayaWhip.  0xffefd5
		/// </summary>
		public static readonly Color PapayaWhip = Color.FromArgb(255, 239, 213);
		/// <summary>
		/// Color PeachPuff.  0xffdab9
		/// </summary>
		public static readonly Color PeachPuff = Color.FromArgb(255, 218, 185);
		/// <summary>
		/// Color Peru.  0xcd853f
		/// </summary>
		public static readonly Color Peru = Color.FromArgb(205, 133, 63);
		/// <summary>
		/// Color Pink.  0xffc0cb
		/// </summary>
		public static readonly Color Pink = Color.FromArgb(255, 192, 203);
		/// <summary>
		/// Color Plum.  0xdda0dd
		/// </summary>
		public static readonly Color Plum = Color.FromArgb(221, 160, 221);
		/// <summary>
		/// Color PowderBlue.  0xb0e0e6
		/// </summary>
		public static readonly Color PowderBlue = Color.FromArgb(176, 224, 230);
		/// <summary>
		/// Color Purple.  0x800080
		/// </summary>
		public static readonly Color Purple = Color.FromArgb(128, 0, 128);
		/// <summary>
		/// Color Red.  0xff0000
		/// </summary>
		public static readonly Color Red = Color.FromArgb(255, 0, 0);
		/// <summary>
		/// Color RosyBrown.  0xbc8f8f
		/// </summary>
		public static readonly Color RosyBrown = Color.FromArgb(188, 143, 143);
		/// <summary>
		/// Color RoyalBlue.  0x4169e1
		/// </summary>
		public static readonly Color RoyalBlue = Color.FromArgb(65, 105, 225);
		/// <summary>
		/// Color SaddleBrown.  0x8b4513
		/// </summary>
		public static readonly Color SaddleBrown = Color.FromArgb(139, 69, 19);
		/// <summary>
		/// Color Salmon.  0xfa8072
		/// </summary>
		public static readonly Color Salmon = Color.FromArgb(250, 128, 114);
		/// <summary>
		/// Color SandyBrown.  0xf4a460
		/// </summary>
		public static readonly Color SandyBrown = Color.FromArgb(244, 164, 96);
		/// <summary>
		/// Color SeaGreen.  0x2e8b57
		/// </summary>
		public static readonly Color SeaGreen = Color.FromArgb(46, 139, 87);
		/// <summary>
		/// Color SeaShell.  0xfff5ee
		/// </summary>
		public static readonly Color SeaShell = Color.FromArgb(255, 245, 238);
		/// <summary>
		/// Color Sienna.  0xa0522d
		/// </summary>
		public static readonly Color Sienna = Color.FromArgb(160, 82, 45);
		/// <summary>
		/// Color Silver.  0xc0c0c0
		/// </summary>
		public static readonly Color Silver = Color.FromArgb(192, 192, 192);
		/// <summary>
		/// Color SkyBlue.  0x87ceeb
		/// </summary>
		public static readonly Color SkyBlue = Color.FromArgb(135, 206, 235);
		/// <summary>
		/// Color SlateBlue.  0x6a5acd
		/// </summary>
		public static readonly Color SlateBlue = Color.FromArgb(106, 90, 205);
		/// <summary>
		/// Color SlateGray.  0x708090
		/// </summary>
		public static readonly Color SlateGray = Color.FromArgb(112, 128, 144);
		/// <summary>
		/// Color Snow.  0xfffafa
		/// </summary>
		public static readonly Color Snow = Color.FromArgb(255, 250, 250);
		/// <summary>
		/// Color SpringGreen.  0x00ff7f
		/// </summary>
		public static readonly Color SpringGreen = Color.FromArgb(0, 255, 127);
		/// <summary>
		/// Color SteelBlue.  0x4682b4
		/// </summary>
		public static readonly Color SteelBlue = Color.FromArgb(70, 130, 180);
		/// <summary>
		/// Color Tan.  0xd2b48c
		/// </summary>
		public static readonly Color Tan = Color.FromArgb(210, 180, 140);
		/// <summary>
		/// Color Teal.  0x008080
		/// </summary>
		public static readonly Color Teal = Color.FromArgb(0, 128, 128);
		/// <summary>
		/// Color Thistle.  0xd8bfd8
		/// </summary>
		public static readonly Color Thistle = Color.FromArgb(216, 191, 216);
		/// <summary>
		/// Color Tomato.  0xff6347
		/// </summary>
		public static readonly Color Tomato = Color.FromArgb(255, 99, 71);
		/// <summary>
		/// Color Transparent.  0xffffff
		/// </summary>
		public static readonly Color Transparent = Color.FromArgb(255, 255, 255);
		/// <summary>
		/// Color Turquoise.  0x40e0d0
		/// </summary>
		public static readonly Color Turquoise = Color.FromArgb(64, 224, 208);
		/// <summary>
		/// Color Violet.  0xee82ee
		/// </summary>
		public static readonly Color Violet = Color.FromArgb(238, 130, 238);
		/// <summary>
		/// Color Wheat.  0xf5deb3
		/// </summary>
		public static readonly Color Wheat = Color.FromArgb(245, 222, 179);
		/// <summary>
		/// Color White.  0xffffff
		/// </summary>
		public static readonly Color White = Color.FromArgb(255, 255, 255);
		/// <summary>
		/// Color WhiteSmoke.  0xf5f5f5
		/// </summary>
		public static readonly Color WhiteSmoke = Color.FromArgb(245, 245, 245);
		/// <summary>
		/// Color Yellow.  0xffff00
		/// </summary>
		public static readonly Color Yellow = Color.FromArgb(255, 255, 0);
		/// <summary>
		/// Color YellowGreen.  0x9acd32
		/// </summary>
		public static readonly Color YellowGreen = Color.FromArgb(154, 205, 50);

		#endregion
		#region --- Accessing Static Named Colors ---

		static System.Reflection.FieldInfo NamedColorStaticField(string name)
		{
			var colorType = typeof(Color).GetTypeInfo();

            var result = colorType.DeclaredFields.FirstOrDefault(
				x => x.FieldType == typeof(Color) && 
					x.IsStatic && x.IsPublic && x.IsInitOnly && 
					x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			
			if (result == null)
				return null;

			return result;
		}

		/// <summary>
		/// Gets whether or not the specified value is a named color.
		/// Named colors correspond to the public static properties of the Color structure.
		/// </summary>
		/// <param name="colorName"></param>
		/// <returns></returns>
		public static bool IsNamedColor(string colorName)
		{
			return NamedColorStaticField(colorName) != null;
		}

		/// <summary>
		/// Returns the color structure corresponding to the named value.
		/// Named colors correspond to the public static properties of the Color structure.
		/// </summary>
		/// <param name="colorName"></param>
		/// <returns></returns>
		public static Color GetNamedColor(string colorName)
		{
			var pi = NamedColorStaticField(colorName);

			if (pi == null)
				throw new ArgumentException("Argument passed was not the name of a color.");

		    return (Color)pi.GetValue(null);
		}

		#endregion

		#region --- Private Data ---

		[FieldOffset(3)]
		byte a;

		[FieldOffset(0)]
		byte r;

		[FieldOffset(1)]
		byte g;

		[FieldOffset(2)]
		byte b;

		[FieldOffset(0)]
		[DataMember]
		int abgr;

		#endregion

		#region --- Construction ---

		private Color(int a, int r, int g, int b)
		{
			this.abgr = 0;
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
			if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
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

		#endregion

		/// <summary>
		/// Returns a number from 0.0 to 1.0 indicating the intensity of the color, normalized
		/// in a way which approximately represents the human eye's response to color.
		/// </summary>
		public double Intensity
		{
			get { return (0.30 * R + 0.59 * G + 0.11 * B) / 255.0; }
		}


		/// <summary>
		/// Converts this Color structure to a 32-bit integer in the format
		/// 0xAARRGGBB.  This is suitable for input to Color.FromArgb to 
		/// reproduce the color structure.
		/// </summary>
		/// <returns></returns>
		public int ToArgb()
		{
			int val = A;

			val <<= 8;
			val |= R;

			val <<= 8;
			val |= G;

			val <<= 8;
			val |= B;

			return val;
		}
		/// <summary>
		/// Converts this Color structure to a 32-bit integer in the format
		/// 0xAABBGGRR.  This is suitable for passing to Direct3D or OpenGL
		/// in a vertex structure.
		/// </summary>
		/// <returns></returns>
		public int ToAbgr()
		{
			return abgr;
		}

		// See algorithm at http://en.wikipedia.org/wiki/YUV#Conversion_to.2Ffrom_RGB
		const double W_R = 0.299;
		const double W_B = 0.114;
		const double W_G = 0.587;
		const double Umax = 0.436;
		const double Vmax = 0.615;

		public void ToYuv(out double y, out double u, out double v)
		{
			y = (W_R * r + W_G * g + W_B * b) / 255.0;
			u = Umax * (b/255.0 - y) / (1 - W_B);
			v = Vmax * (r / 255.0 - y) / (1 - W_R);
		}

		public static Color FromYuv(double y, double u, double v)
		{
			return Color.FromArgb(
				(int)(255 * (y + v * (1 - W_R) / Vmax)),
				(int)(255 * (y - u * W_B * (1 - W_B) / (Umax * W_G) - v * W_R * (1 - W_R) / (Vmax * W_G))),
				(int)(255 * (y + u * (1 - W_B) / Umax)));
		}

		/// <summary>
		/// Returns a Color object calculated from hue, saturation and value.
		/// See algorithm at http://en.wikipedia.org/wiki/HSL_and_HSV#From_HSV
		/// </summary>
		/// <param name="h">The hue angle in degrees.</param>
		/// <param name="s">A value from 0 to 1 representing saturation.</param>
		/// <param name="v">A value from 0 to 1 representing the value.</param>
		/// <returns></returns>
		public static Color FromHsv(double h, double s, double v)
		{
			while (h < 0)
				h += 360;
			if (h > 360)
				h = h % 360;

			double hp = h / 60;
			double chroma = v * s;
			double X = chroma * (1 - Math.Abs(hp % 2 - 1));

			double r1 = 0, b1 = 0, g1 = 0;

			switch ((int)hp)
			{
				case 0: r1 = chroma; g1 = X; break;
				case 1: r1 = X; g1 = chroma; break;
				case 2: g1 = chroma; b1 = X; break;
				case 3: g1 = X; b1 = chroma; break;
				case 4: r1 = X; b1 = chroma; break;
				case 5: r1 = chroma; b1 = X; break;
			}

			double m = v - chroma;

			return Color.FromArgb((int)(255 * (r1 + m)), (int)(255 * (g1 + m)), (int)(255 * (b1 + m)));
		}
	}
}