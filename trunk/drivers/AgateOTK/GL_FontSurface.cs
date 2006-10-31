using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib;
using ERY.AgateLib.Drivers;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

using OpenTK.OpenGL;

namespace ERY.AgateLib.OpenGL
{
    public class GL_FontSurface : FontSurfaceImpl 
    {
        public override int StringDisplayWidth(string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int StringDisplayHeight(string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override Size StringDisplaySize(string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawText(int destX, int destY, string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawText(double destX, double destY, string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawText(Point destPt, string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawText(PointF destPt, string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }


        /*
        private GL_Display mDisplay;
        private FontSurface mFontSurface;
        private int mFontID;

        private const int fontChars = 256 - 32;

        private int mFontHeight;

        public GL_FontSurface(FontSurface owner, Font font)
        {
            mDisplay = Display.Impl as WGL_Display;
            mFontSurface = owner;

            BuildFont(font);
        }

        private void BuildFont(Font winFont)
        {
            IntPtr font;                                                        // Windows Font ID
            IntPtr oldfont;                                                     // Used For Good House Keeping

            mFontHeight = winFont.Height;

            mFontID = Gl.glGenLists(fontChars);                                // Storage For fontChars Characters

            font = Gdi.CreateFont(                                              // Create The Font
                mFontHeight,                                           // Height Of Font
                0,                                                              // Width Of Font
                0,                                                              // Angle Of Escapement
                0,                                                              // Orientation Angle
                winFont.Bold ? 400 : Gdi.FW_BOLD,                                // Font Weight
                winFont.Italic,                                                          // Italic
                winFont.Underline,                                                          // Underline
                winFont.Strikeout,                                                          // Strikeout
                Gdi.ANSI_CHARSET,                                               // Character Set Identifier
                Gdi.OUT_TT_PRECIS,                                              // Output Precision
                Gdi.CLIP_DEFAULT_PRECIS,                                        // Clipping Precision
                Gdi.ANTIALIASED_QUALITY,                                        // Output Quality
                Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH,                            // Family And Pitch
                winFont.Name);                                                  // Font Name

            IntPtr hDC = (mDisplay.CurrentWindow.Impl as WGL_DisplayWindow).hDC;

            oldfont = Gdi.SelectObject(hDC, font);                              // Selects The Font We Want
            Wgl.wglUseFontBitmaps(hDC, 32, fontChars, mFontID);                // Builds fontChars Characters Starting At Character 32
            Gdi.SelectObject(hDC, oldfont);                                     // Selects The Font We Want
            Gdi.DeleteObject(font);                                             // Delete The Font
        }

        private void glPrint(string text)
        {
            if (string.IsNullOrEmpty(text))
            {                            
                return;                                                         // Do Nothing
            }

            Gl.glPushAttrib(Gl.GL_LIST_BIT);                                    // Pushes The Display List Bits
            Gl.glListBase(mFontID - 32);                                   // Sets The Base Character to 32

            // .NET -- we can't just pass text, we need to convert to a byte array
            byte[] textbytes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++) 
                textbytes[i] = (byte)text[i];

            Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);        // Draws The Display List Text
            Gl.glPopAttrib();                                                   // Pops The Display List Bits
        }

        public override int StringDisplayWidth(string text)
        {
            return 10;
        }

        public override int StringDisplayHeight(string text)
        {
            return mFontHeight;
        }

        public override Size StringDisplaySize(string text)
        {
            return new Size(10, mFontHeight);
        }

        public override void DrawText(int dest_x, int dest_y, string text)
        {
            mDisplay.SetGLColor(mFontSurface.Color);

            // Position The Text On The Screen
            Gl.glRasterPos2f((float)dest_x, (float)dest_y + mFontHeight);

            // Print GL Text To The Screen
            glPrint(text);
            
            
        }

        public override void DrawText(double dest_x, double dest_y, string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawText(Point dest_pt, string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawText(PointF dest_pt, string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawText(string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        public override void Dispose()
        {
            Gl.glDeleteLists(mFontID, fontChars);  
        }
         * */
    }
}
