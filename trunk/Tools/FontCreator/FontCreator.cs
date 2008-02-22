using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib;

namespace FontCreator
{
    class FontCreator
    {
        private string mText;
        private object mRenderTarget;
        private object mZoomRenderTarget;
        private string mFontFamily;
        private float mFontSize = 10.0f;

        DisplayWindow wind;
        DisplayWindow zoomWind;
        FontSurface font;

        private bool mBold;
        private bool mItalic;
        private bool mStrike;
        private bool mUnderline;

        public bool Underline
        {
            get { return mUnderline; }
            set
            {
                mUnderline = value;
                CreateFont();
            }
        }

        public bool Strikeout
        {
            get { return mStrike; }
            set
            {
                mStrike = value;
                CreateFont();
            }
        }

        public bool Italic
        {
            get { return mItalic; }
            set
            {
                mItalic = value;
                CreateFont();
            }
        }

        public bool Bold
        {
            get { return mBold; }
            set
            {
                mBold = value;
                CreateFont();
            }
        }

        public float FontSize
        {
            get { return mFontSize; }
            set
            {
                mFontSize = value;
                CreateFont();
            }
        }

        public object RenderTarget
        {
            get { return mRenderTarget; }
        }
        public string FontFamily
        {
            get { return mFontFamily; }
            set
            {
                mFontFamily = value;
                CreateFont();
            }
        }

        public void SetRenderTarget(object render, object zoomRender)
        {
            mRenderTarget = render;
            mZoomRenderTarget = zoomRender;

            if (wind != null)
            {
                wind.Dispose();
                zoomWind.Dispose();
            }

            wind = DisplayWindow.FromControl(render);
            zoomWind = DisplayWindow.FromControl(zoomRender);
        }

        private void CreateFont()
        {
            if (string.IsNullOrEmpty(mFontFamily))
                return;
            if (font != null)
                font.Dispose();

            font = new FontSurface(mFontFamily, mFontSize, Style);

            Draw();
        }

        FontStyle Style
        {
            get
            {
                return
                    (Bold ? FontStyle.Bold : 0) |
                    (Italic ? FontStyle.Italic : 0) |
                    (Underline ? FontStyle.Underline : 0) |
                    (Strikeout ? FontStyle.Strikeout : 0);
            }
        }

        public string Text
        {
            get { return mText; }
            set
            {
                mText = value;
                Draw();
            }
        }

        public FontCreator()
        {
            StringBuilder b = new StringBuilder();

            b.AppendLine("Sample Text");
            b.AppendLine("abcdefghijklm   ABCDEFGHIJKLM");
            b.AppendLine("nopqrstuvwxyz   NOPQRSTUVWXYZ");
            b.AppendLine("01234567890");
            b.AppendLine("!@#$%^&*(),<.>/?;:'\"-_=+\\|");

            mText = b.ToString();
        }

        public void Draw()
        {
            Display.RenderTarget = wind;
            Display.BeginFrame();
            Display.Clear();

            font.SetScale(1.0, 1.0);
            DrawText();

            Display.EndFrame();


            Display.RenderTarget = zoomWind;
            Display.BeginFrame();
            Display.Clear();

            font.SetScale(8.0, 8.0);
            DrawText();

            Display.EndFrame();
        }

        private void DrawText()
        {
            if (font == null)
                return;

            font.DrawText(Text);
        }
    }
}