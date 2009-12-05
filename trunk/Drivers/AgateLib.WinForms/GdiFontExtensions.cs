using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.BitmapFont;

namespace AgateLib.WinForms
{
	class GdiFontExtensions
	{
		[DllImport("gdi32.dll")]
		static extern uint GetKerningPairs(IntPtr hdc, uint nNumPairs,[Out] KERNINGPAIR[] lpkrnpair);

		[DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
		static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

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

			public override string ToString()
			{
				return (String.Format("{{First={0}, Second={1}, Amount={2}}}", wFirst, wSecond, iKernAmount));
			}
		}

		internal void LoadKerningPairs(FontMetrics glyphs, Font font, Bitmap bmp, Graphics g)
		{
			IntPtr hdc = g.GetHdc();
			SelectObject(hdc, font.ToHfont());

			uint numPairs = GetKerningPairs(hdc, 0, null);

			if (numPairs == 0)
				return;

			KERNINGPAIR[] kerningArray = new KERNINGPAIR[numPairs];

			uint err = GetKerningPairs(hdc, numPairs, kerningArray);

			if (err == 0) throw new Exception();

			foreach (var kern in kerningArray)
			{
				if (kern.iKernAmount == 0)
					continue;

				if (glyphs.ContainsKey((char)kern.wFirst) == false)
					continue;
				if (glyphs.ContainsKey((char)kern.wSecond) == false)
					continue;

				glyphs[(char)kern.wFirst].KerningPairs.Add((char)kern.wSecond, kern.iKernAmount);
			}

		}
	}
}
