//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using Drawing = System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Resources;
using AgateLib.Quality;

namespace AgateLib.Platform.WinForms.Fonts
{
	static partial class BitmapFontUtil
	{
		interface ICharacterRenderer
		{
			Drawing.Font Font { get; set; }

			int Padding { get; }
			Size MeasureChar(Drawing.Graphics g, char c);
			void DrawChar(Drawing.Graphics g, char c, Point location, Drawing.Color clr);

			void ModifyMetrics(FontMetrics glyphs, Drawing.Graphics g);
		}

		class TextRend : ICharacterRenderer
		{
			Drawing.Font font;

			public TextRend(Drawing.Font font)
			{
				Font = font;
			}
			public System.Drawing.Font Font
			{
				get { return font; }
				set { font = value; }
			}
			public int Padding
			{
				get { return 1; }
			}

			private const TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;

			public void ModifyMetrics(FontMetrics glyphs, Drawing.Graphics g)
			{ }
			public Size MeasureChar(System.Drawing.Graphics g, char c)
			{
				string text = c.ToString();

				Drawing.Size size = TextRenderer.MeasureText(g, text,
						   font, new System.Drawing.Size(256, 256), flags);

				return new Size(size.Width, size.Height);
			}

			public void DrawChar(System.Drawing.Graphics g, char c, Point location, Drawing.Color clr)
			{
				string text = c.ToString();

				TextRenderer.DrawText(g, text, font,
						new System.Drawing.Rectangle(location.X, location.Y, 256, 256),
						clr, flags);
			}

			
		}
		class GraphicsRend : ICharacterRenderer
		{
			Drawing.Font font;
			float padding;

			public GraphicsRend(Drawing.Font font)
			{
				Font = font;
			}
			public System.Drawing.Font Font
			{
				get { return font; }
				set { font = value; }
			}

			public void ModifyMetrics(FontMetrics glyphs, Drawing.Graphics g)
			{ }

			public int Padding
			{
				get { return (int)Math.Ceiling(padding); }
			}
			void CalculatePadding(Drawing.Graphics g)
			{
				// apparently .NET (or GDI+) does this stupid thing on Windows
				// where is reports extra padded space around the characters drawn.
				// Fortunately, this padding is equal the reported size of the
				// space character, which is not drawn when by itself.
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					SizeF padSize = g.MeasureString(" ", font).ToGeometry();

					padding = padSize.Width - 1;
				}
			}
			public Size MeasureChar(Drawing.Graphics g, char c)
			{
				string text = c.ToString();

				if (padding == 0)
					CalculatePadding(g);

				Drawing.SizeF size = g.MeasureString(text, font);
				size.Width -= padding;

				// for space character on windows.
				if (text == " " && padding > 0)
					size.Width = padding;

				return new Size((int)(size.Width), (int)(size.Height));
			}

			public void DrawChar(Drawing.Graphics g, char c, Point location, Drawing.Color clr)
			{
				string text = c.ToString();

				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

				// we need to adjust the position by half the padding
				location.X -= (int)Math.Ceiling(padding / 2);

				using (Drawing.Brush brush = new Drawing.SolidBrush(clr))
				{
					g.DrawString(text, font, brush, new Drawing.Point(location.X, location.Y));
				}
			}
		}
		class GdiWindows : ICharacterRenderer
		{
			Drawing.Font font;
			IntPtr fontHandle;

			bool hasMetric = false;
			TEXTMETRIC metric;

			#region --- Imports ---

			[DllImport("gdi32.dll")]
			static extern bool GetCharWidth32(IntPtr hdc, uint iFirstChar, uint iLastChar,
			   [Out] int[] lpBuffer);
			
			[DllImport("gdi32.dll")]
			static extern uint GetKerningPairs(IntPtr hdc, uint nNumPairs, [Out] KERNINGPAIR[] lpkrnpair);

			[DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
			static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

			[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
			static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);

			[DllImport("user32.dll")]
			static extern int DrawText(IntPtr hDC, string lpString, int nCount, ref RECT lpRect, uint uFormat);
			
			[DllImport("user32.dll")]
			static extern int DrawTextEx(IntPtr hdc, StringBuilder lpchText, int cchText,
			   ref RECT lprc, uint dwDTFormat, ref DRAWTEXTPARAMS lpDTParams);

			[DllImport("gdi32.dll")]
			static extern uint SetTextColor(IntPtr hdc, int crColor);

			[DllImport("gdi32.dll")]
			static extern uint SetBkColor(IntPtr hdc, int crColor);

			[DllImport("gdi32.dll")]
			static extern int SetBkMode(IntPtr hdc, int iBkMode);

			[DllImport("gdi32.dll")]
			static extern int SetMapMode(IntPtr hdc, int fnMapMode);

			[DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
			static extern bool GetCharABCWidths(IntPtr hdc, uint uFirstChar,
			   uint uLastChar, out ABC lpabc);

			#endregion
			#region --- Structures ---

			const int TRANSPARENT = 1;
			const int OPAQUE = 2;

			//Mapping Modes
			const int MM_TEXT = 1;
			const int MM_LOMETRIC = 2;
			const int MM_HIMETRIC = 3;
			const int MM_LOENGLISH = 4;
			const int MM_HIENGLISH = 5;
			const int MM_TWIPS = 6;
			const int MM_ISOTROPIC = 7;
			const int MM_ANISOTROPIC = 8;

			//Minimum and Maximum Mapping Mode values
			const int MM_MIN = MM_TEXT;
			const int MM_MAX = MM_ANISOTROPIC;
			const int MM_MAX_FIXEDSCALE = MM_TWIPS;

			[StructLayout(LayoutKind.Sequential)]
			private struct ABC
			{
				public int abcA;
				public uint abcB;
				public int abcC;

				public int AgateWidth
				{
					get { return Math.Abs(abcA) + Math.Abs((int)abcB) + Math.Abs(abcC); }
				}
				public int AdvanceWidth
				{
					get { return abcA + (int)abcB + abcC; }
				}
				public override string ToString()
				{
					return string.Format("A={0}, B={1}, C={2}", abcA, abcB, abcC);
				}
			}

			[StructLayout(LayoutKind.Sequential)]
			struct KERNINGPAIR
			{
				public ushort wFirst; // might be better off defined as char
				public ushort wSecond; // might be better off defined as char
				public int iKernAmount;

				public KERNINGPAIR(ushort wFirst, ushort wSecond, int iKernAmount)
				{
					this.wFirst = wFirst;
					this.wSecond = wSecond;
					this.iKernAmount = iKernAmount;
				}

				public char First { get { return (char)wFirst; } }
				public char Second { get { return (char)wSecond; } }
				public int Amount { get { return iKernAmount; } }

				public override string ToString()
				{
					return (String.Format("{{First={0}, Second={1}, Amount={2}}}", wFirst, wSecond, iKernAmount));
				}
			}

			[Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			public struct TEXTMETRIC
			{
				public int tmHeight;
				public int tmAscent;
				public int tmDescent;
				public int tmInternalLeading;
				public int tmExternalLeading;
				public int tmAveCharWidth;
				public int tmMaxCharWidth;
				public int tmWeight;
				public int tmOverhang;
				public int tmDigitizedAspectX;
				public int tmDigitizedAspectY;
				public char tmFirstChar;
				public char tmLastChar;
				public char tmDefaultChar;
				public char tmBreakChar;
				public byte tmItalic;
				public byte tmUnderlined;
				public byte tmStruckOut;
				public byte tmPitchAndFamily;
				public byte tmCharSet;
			}

			private struct RECT
			{
				public int Left, Top, Right, Bottom;
				public RECT(Rectangle r)
				{
					this.Left = r.Left;
					this.Top = r.Top;
					this.Bottom = r.Bottom;
					this.Right = r.Right;
				}
			}

			[StructLayout(LayoutKind.Sequential)]
			public struct DRAWTEXTPARAMS
			{
				public uint cbSize;
				public int iTabLength;
				public int iLeftMargin;
				public int iRightMargin;
				public uint uiLengthDrawn;
			}

			#endregion


			public GdiWindows(Drawing.Font font)
			{
				Condition.Requires<InvalidOperationException>(AgateApp.Platform.PlatformType == PlatformType.Windows,
					"Gdi renderer only works on Windows.");

				Font = font;
			}
			public System.Drawing.Font Font
			{
				get { return font; }
				set
				{
					font = value;
					fontHandle = font.ToHfont();
					
				}
			}
			public int Padding
			{
				get { return 0; }
			}

			Dictionary<char, ABC> mABCs = new Dictionary<char, ABC>();

			int ColorToWin32Color(Drawing.Color clr)
			{
				return clr.R + clr.G << 8 + clr.B << 16;
			}
			public Size MeasureChar(System.Drawing.Graphics g, char c)
			{
				IntPtr hdc = g.GetHdc();

				if (SetMapMode(hdc, MM_TEXT) == 0)
					throw new InvalidOperationException("Failed to set map mode.");

				SelectObject(hdc, fontHandle);

				if (hasMetric == false)
				{
					GetTextMetrics(hdc, out metric);
					hasMetric = true;
				}

				ABC abc;

				GetCharABCWidths(hdc, (uint)c, (uint)c, out abc);
				
				// hack because the lower case f doesn't come out with the right
				// source rect on some small fonts.
				if (c == 'f')
				{
					abc.abcA -= 1;
				}

				mABCs[c] = abc;

				int width = abc.AgateWidth;

				g.ReleaseHdc();
				return new Size(width+1, metric.tmHeight + metric.tmInternalLeading + metric.tmExternalLeading);
			}

			public void DrawChar(System.Drawing.Graphics g, char c, Point location, System.Drawing.Color clr)
			{
				
				location.X -= mABCs[c].abcA;

				TextRenderer.DrawText(g, c.ToString(), font, location.ToDrawing(), clr, 
					TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
				//StringBuilder b = new StringBuilder();
				//b.Append(c.ToString());

				//IntPtr hdc = g.GetHdc();
				//RECT r = new RECT { Left = location.X, Top = location.Y, Right = 10000, Bottom = 10000 };

				//SelectObject(hdc, fontHandle);
				//SetBkMode(hdc, TRANSPARENT);
				////SetBkColor(hdc, 0);
				//SetTextColor(hdc, ColorToWin32Color(clr));

				//DRAWTEXTPARAMS dtpa = new DRAWTEXTPARAMS();

				
				//DrawText(hdc, b, -1, ref r, 0, ref dtpa);

				//g.ReleaseHdc();
			}

			public void ModifyMetrics(FontMetrics glyphs, Drawing.Graphics g)
			{
				IntPtr hdc = g.GetHdc();
				SelectObject(hdc, font.ToHfont());

				if (SetMapMode(hdc, MM_TEXT) == 0)
					throw new InvalidOperationException("Failed to set map mode.");

				foreach (char c in glyphs.Keys)
				{
					ABC abc = mABCs[c];

					int left = -abc.abcA;
					int right = -(abc.AdvanceWidth - abc.AgateWidth - 1 + left);

					glyphs[c].LeftOverhang += left;
					glyphs[c].RightOverhang += right;
				}

				uint numPairs = GetKerningPairs(hdc, 0, null);

				if (numPairs == 0)
					return;

				KERNINGPAIR[] kerningArray = new KERNINGPAIR[numPairs];

				uint err = GetKerningPairs(hdc, numPairs, kerningArray);

				if (err == 0)
					throw new InvalidOperationException();

				foreach (var kern in kerningArray)
				{
					int amount = kern.iKernAmount;

					// Kerning is too strong, particularly for the letter T for some reason.  
					// This hack fixes it.
					amount /= 2;

					if (amount == 0)
						continue;

					if (glyphs.ContainsKey(kern.First) == false)
						continue;
					if (glyphs.ContainsKey(kern.Second) == false)
						continue;
						
					glyphs[kern.First].KerningPairs[kern.Second] = amount;
				}

				g.ReleaseHdc();
			}

		}
	}
}
