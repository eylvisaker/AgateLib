using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib;
using ERY.AgateLib.ImplBase;

namespace FontCreator
{
    class FontCreator
    {
        private string mText;
        private object mRenderTarget;
        private object mZoomRenderTarget;
        
        DisplayWindow wind;
        DisplayWindow zoomWind;
        FontSurface font;

        BitmapFontOptions mOptions = new BitmapFontOptions();


        private void SetStyle(FontStyle fontStyle, bool value)
        {
            if (value)
            {
                mOptions.FontStyle = mOptions.FontStyle | fontStyle;
            }
            else
            {
                mOptions.FontStyle = mOptions.FontStyle & ~fontStyle;
            }

            CreateFont();
        }

        private bool StyleContains(FontStyle fontStyle)
        {
            return (mOptions.FontStyle & fontStyle) == fontStyle;
        }

        public bool Underline
        {
            get { return StyleContains(FontStyle.Underline); }
            set { SetStyle(FontStyle.Underline, value); }
        }

        public bool Strikeout
        {
            get { return StyleContains(FontStyle.Strikeout); }
            set { SetStyle(FontStyle.Strikeout, value); }
        }

        public bool Italic
        {
            get { return StyleContains(FontStyle.Italic); }
            set { SetStyle(FontStyle.Italic, value); }
        }

        public bool Bold
        {
            get { return StyleContains(FontStyle.Bold); }
            set { SetStyle(FontStyle.Bold, value); }
        }

        public float FontSize
        {
            get { return mOptions.SizeInPoints; }
            set
            {
                mOptions.SizeInPoints = value;
                CreateFont();
            }
        }

        public object RenderTarget
        {
            get { return mRenderTarget; }
        }
        public string FontFamily
        {
            get { return mOptions.FontFamily; }
            set
            {
                mOptions.FontFamily = value;
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

        public void CreateFont()
        {
            if (string.IsNullOrEmpty(FontFamily))
                return;
            if (font != null)
                font.Dispose();

            font = new FontSurface(mOptions);

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