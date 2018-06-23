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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.WinForms;
using FontStyle = AgateLib.DisplayLib.FontStyle;
using Geometry = AgateLib.Geometry;

namespace AgateDrawing
{
	class Drawing_FontSurface : FontSurfaceImpl
	{
		Font mFont;

		public Drawing_FontSurface(string fontFamily, float sizeInPoints, FontStyle style)
		{
			System.Drawing.FontStyle drawingStyle = System.Drawing.FontStyle.Regular;

			if ((style & FontStyle.Bold) > 0)
				drawingStyle |= System.Drawing.FontStyle.Bold;
			if ((style & FontStyle.Italic) > 0)
				drawingStyle |= System.Drawing.FontStyle.Italic;
			if ((style & FontStyle.Strikeout) > 0)
				drawingStyle |= System.Drawing.FontStyle.Strikeout;
			if ((style & FontStyle.Underline) > 0)
				drawingStyle |= System.Drawing.FontStyle.Underline;

			mFont = new Font(fontFamily, sizeInPoints, drawingStyle);
		}
		public override void Dispose()
		{
			mFont = null;
		}

		public override int FontHeight
		{
			get { return mFont.Height; }
		}
		public override void DrawText(FontState state)
		{
			Geometry.PointF shift = Origin.CalcF(state.DisplayAlignment,
				MeasureString(state, state.Text));

			PointF dest_pt = Interop.Convert(state.Location);
			dest_pt.X -= shift.X;
			dest_pt.Y -= shift.Y;

			Drawing_Display disp = Display.Impl as Drawing_Display;
			Graphics g = disp.FrameGraphics;

			GraphicsState g_state = g.Save();
			double scalex = state.ScaleWidth, scaley = state.ScaleHeight;

			g.TranslateTransform(dest_pt.X, dest_pt.Y);
			g.ScaleTransform((float)scalex, (float)scaley);

			g.DrawString(state.Text, mFont,
				new SolidBrush(Interop.Convert(state.Color)), Point.Empty);

			g.Restore(g_state);
		}
		public override AgateLib.Geometry.Size MeasureString(FontState state, string text)
		{
			Drawing_Display disp = Display.Impl as Drawing_Display;
			Graphics g = disp.FrameGraphics;
			bool disposeGraphics = false;

			if (g == null)
			{
				g = Graphics.FromImage((disp.RenderTarget.Impl as Drawing_FrameBuffer).BackBufferBitmap);

				disposeGraphics = true;
			}

			SizeF result = new SizeF(g.MeasureString(text, mFont));

			if (disposeGraphics)
				g.Dispose();


			result.Height *= (float)state.ScaleWidth;
			result.Width *= (float)state.ScaleHeight;

			return new Geometry.Size((int)result.Width, (int)result.Height);
		}
	}
}
