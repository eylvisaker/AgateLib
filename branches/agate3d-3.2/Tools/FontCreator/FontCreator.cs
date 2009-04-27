using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.BitmapFont;
using AgateLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Resources;

namespace FontCreator
{
	public class FontBuilder
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
		public int BottomMarginAdjust
		{
			get { return mOptions.BottomMarginAdjust; }
			set
			{
				mOptions.BottomMarginAdjust = value;
				CreateFont();
			}
		}
		public int TopMarginAdjust
		{
			get { return mOptions.TopMarginAdjust; }
			set
			{
				mOptions.TopMarginAdjust = value;
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

			zoomWind = DisplayWindow.CreateFromControl(zoomRender);
			wind = DisplayWindow.CreateFromControl(render);
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

		public FontBuilder()
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
			((BitmapFontImpl)font.Impl).InterpolationHint = InterpolationMode.Fastest;

			DrawBackground();
			DrawText();

			Display.EndFrame();


			Display.RenderTarget = wind;
			Display.BeginFrame();
			Display.Clear();

			font.SetScale(1.0, 1.0);
			((BitmapFontImpl)font.Impl).InterpolationHint = InterpolationMode.Nicest;

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

		internal bool SaveFont(string resourceFile, string fontName, string imageFile)
		{
			AgateResourceCollection resources;

			if (File.Exists(resourceFile))
				resources = AgateResourceLoader.LoadResources(resourceFile);
			else
				resources = new AgateResourceCollection();

			if (Path.IsPathRooted(resourceFile) == false)
			{
				resourceFile = Path.Combine(Directory.GetCurrentDirectory(), resourceFile);
			}

			string localImagePath;
			string dir = Path.GetDirectoryName(resourceFile);

			if (Path.IsPathRooted(imageFile) == false)
			{
				localImagePath = imageFile;
				imageFile = Path.Combine(Path.GetDirectoryName(resourceFile), imageFile);
			}
			else
				localImagePath = GetRelativePath(dir, imageFile);

			SaveImage(imageFile);

			localImagePath = localImagePath.Replace(Path.DirectorySeparatorChar.ToString(), "/");

			BitmapFontResource res = new BitmapFontResource(fontName);
			res.Image = localImagePath;
			res.FontMetrics = ((BitmapFontImpl)Font.Impl).FontMetrics.Clone();

			if (resources.Contains(res.Name))
			{
				if (System.Windows.Forms.MessageBox.Show(
					"The specified resource file already contains a resource named \""
					+ res.Name + "\"." + Environment.NewLine
					+ "Would you like to overwrite it?", res.Name + " already exists",
					System.Windows.Forms.MessageBoxButtons.YesNo,
					System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
				{
					resources.Remove(res.Name);
				}
				else
				{
					return false;
				}
			}
			resources.Add(res);

			AgateResourceLoader.SaveResources(resources, resourceFile);

			return true;
		}

		private void SaveImage(string imageFile)
		{
			EnsureDirectoryExists(Path.GetDirectoryName(imageFile));

			((BitmapFontImpl)Font.Impl).Surface.SaveTo(imageFile);
		}

		private void EnsureDirectoryExists(string dirname)
		{
			if (Directory.Exists(dirname))
				return;

			string parentDir = Path.GetDirectoryName(dirname);

			if (Directory.Exists(parentDir) == false)
				EnsureDirectoryExists(parentDir);

			Directory.CreateDirectory(dirname);
		}

		private string GetRelativePath(string dir, string imageFile)
		{
			if (dir.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
				dir += Path.DirectorySeparatorChar;

			for (int i = 0; i < dir.Length; i++)
			{
				if (imageFile.StartsWith(dir.Substring(0, i)) == false)
				{
					return imageFile.Substring(i - 1);
				}
			}


			return imageFile.Substring(dir.Length);
		}
	}
}