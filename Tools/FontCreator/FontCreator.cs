using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib;
using ERY.AgateLib.BitmapFont;
using ERY.AgateLib.ImplBase;
using ERY.AgateLib.Geometry;

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
        Surface bgDark, bgLight;

        BitmapFontOptions mOptions = new BitmapFontOptions();

        public FontSurface Font
        {
            get { return font; }
        }

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

        private bool mDarkBackground;

        private Color mColor;

        public Color DisplayColor
        {
            get { return mColor; }
            set { mColor = value; }
        }
        private double mDisplayScale = 4.0;

        public double DisplayScale
        { 
            get { return mDisplayScale; }
            set { mDisplayScale = value; }
        }
	
        public bool LightBackground
        {
            get { return mDarkBackground; }
            set
            {
                mDarkBackground = value;
                Draw();
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
                bgDark.Dispose();
                bgLight.Dispose();
            }

            zoomWind = new DisplayWindow(zoomRender);
            wind = new DisplayWindow(render);
            //wind = new DisplayWindow(render);
            //zoomWind = new DisplayWindow(zoomRender);

            bgDark = new Surface("bgdark.png");
            bgLight = new Surface("bglight.png");

            DisplayColor = Color.White;
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
        public BitmapFontOptions Options
        {
            get { return mOptions; }
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

            mOptions.UseTextRenderer = true;
        }

        public void Draw()
        {
            if (zoomWind == null)
                return;

            Display.RenderTarget = zoomWind;
            Display.BeginFrame();
            Display.Clear();

            font.SetScale(mDisplayScale, mDisplayScale);

            DrawBackground();
            DrawText();

            Display.EndFrame();


            Display.RenderTarget = wind;
            Display.BeginFrame();
            Display.Clear();

            font.SetScale(1.0, 1.0);

            DrawBackground();
            DrawText();

            Display.EndFrame();


            Core.KeepAlive();
        }

        private void DrawBackground()
        {
            Surface background = LightBackground ? bgLight : bgDark;

            for (int x = 0; x < Display.RenderTarget.Width; x += background.DisplayWidth)
            {
                for (int y = 0; y < Display.RenderTarget.Height; y += background.DisplayHeight)
                {
                    background.Draw(x, y);
                }
            }
        }

        private void DrawText()
        {
            if (font == null)
                return;

            font.Color = DisplayColor;
            font.DrawText(Text);
        }
    }
}