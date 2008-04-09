using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.ImplBase
{
    /// <summary>
    /// Provides a basic implementation for the use of non-system fonts provided
    /// as a bitmap.
    /// 
    /// To construct a bitmap font, call the appropriate static FontSurface method.
    /// </summary>
    public class BitmapFontImpl : FontSurfaceImpl
    {
        Surface mSurface;

        /// <summary>
        /// Stores source rectangles for all characters.
        /// </summary>
        Dictionary<char, Rectangle> mSrcRects = new Dictionary<char, Rectangle>();

        int mCharHeight;
        double mAverageCharWidth;

        /// <summary>
        /// Constructs a BitmapFontImpl, assuming the characters in the given file
        /// are all the same size, and are in their ASCII order.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="characterSize"></param>
        public BitmapFontImpl(string filename, Size characterSize)
        {
            mSurface = new Surface(filename);
            mCharHeight = characterSize.Height;

            ExtractMonoSpaceAsciiFont(characterSize);
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public override void Dispose()
        {
            mSurface.Dispose();
        }

        private void ExtractMonoSpaceAsciiFont(Size characterSize)
        {
            int x = 0;
            int y = 0;
            char val = '\0';

            while (y + characterSize.Height <= mSurface.SurfaceHeight)
            {
                Rectangle src = new Rectangle(x, y, characterSize.Width, characterSize.Height);

                mSrcRects[val] = src;

                val++;
                x += characterSize.Width;

                if (x + characterSize.Width > mSurface.SurfaceWidth)
                {
                    y += characterSize.Height;
                    x = 0;
                }
            }

            CalcAverageCharWidth();
        }

        private void CalcAverageCharWidth()
        {
            IEnumerable<Rectangle> rects = mSrcRects.Values;
            int total = 0;
            int count = 0;

            foreach (Rectangle r in rects)
            {
                total += r.Width;
                count++;
            }

            mAverageCharWidth = total / (double)count;
        }

        /// <summary>
        /// Overrides the base Color method to catch color changes to set them on the surface.
        /// </summary>
        public override Color Color
        {
            get
            {
                return base.Color;
            }
            set
            {
                base.Color = value;

                mSurface.Color = value;
            }
        }
        /// <summary>
        /// Measures the width of the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override int StringDisplayWidth(string text)
        {
            int highestLineWidth = 0;
            
            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd();
                int lineWidth = 0;

                for (int j = 0; j < line.Length; j++)
                {
                    lineWidth += mSrcRects[line[j]].Width;
                }
                
                if (lineWidth > highestLineWidth)
                    highestLineWidth = lineWidth;

            }

            return highestLineWidth;
        }
        /// <summary>
        /// Measures the height of the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override int StringDisplayHeight(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            int CRcount = 0;
            int i = 0;
            
            do
            {
                i = text.IndexOf('\n', i + 1);

                if (i == -1)
                    break;

                CRcount++;

            } while (i != -1);

            if (text[text.Length - 1] == '\n')
                CRcount--;

            return mCharHeight * (CRcount+1);
        }
        /// <summary>
        /// Measures the size of the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override Size StringDisplaySize(string text)
        {
            return new Size(StringDisplayWidth(text), StringDisplayHeight(text));
        }

        private void GetRects(string text, out Rectangle[] srcRects, out Rectangle[] destRects)
        {
            srcRects = new Rectangle[text.Length];
            destRects = new Rectangle[text.Length];

            int destX = 0;
            int destY = 0;
            int height = mCharHeight;

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '\n':
                        destX = 0;
                        destY += height;
                        break;

                    default:
                        srcRects[i] = mSrcRects[text[i]];
                        destRects[i] =
                            new Rectangle(destX, destY,
                            (int)(srcRects[i].Width * ScaleWidth + 0.5),
                            (int)(srcRects[i].Height * ScaleHeight + 0.5));

                        destX += destRects[i].Width;
                        break;
                }
            }
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public override void DrawText(int destX, int destY, string text)
        {
            Rectangle[] srcRects;
            Rectangle[] destRects;

            GetRects(text, out srcRects, out destRects);

            if (DisplayAlignment != OriginAlignment.TopLeft)
            {
                Point value = Origin.Calc(DisplayAlignment, StringDisplaySize(text));

                destX -= value.X;
                destY -= value.Y;
            }

            for (int i = 0; i < destRects.Length; i++)
            {
                destRects[i].X += destX;
                destRects[i].Y += destY;
            }

            mSurface.DrawRects(srcRects, destRects);
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public override void DrawText(double destX, double destY, string text)
        {
            DrawText((int)destX, (int)destY, text);
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public override void DrawText(Point destPt, string text)
        {
            DrawText(destPt.X, destPt.Y, text);
        }


        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public override void DrawText(PointF destPt, string text)
        {
            DrawText(destPt.X, destPt.Y, text);
        }

    }
}