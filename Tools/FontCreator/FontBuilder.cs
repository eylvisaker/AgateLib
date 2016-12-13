using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Geometry;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using AgateLib.Platform.WinForms;

namespace FontCreator
{
	public class FontBuilder
	{
		private ResourceDataLoader dataLoader = new ResourceDataLoader();

		private string mText;
		private object mRenderTarget;
		private object mZoomRenderTarget;

		DisplayWindow wind;
		DisplayWindow zoomWind;
		Font font;
		Surface bgDark, bgLight;
		IReadOnlyList<int> fontSizes = new List<int> { 8, 9, 10, 12, 14, 16, 18, 24, 30 };

		BitmapFontOptions options = new BitmapFontOptions();

		private bool mDarkBackground;
		private const double displayScale = 4.0;

		private Color mColor;
		private int displaySize = 12;
		private FontStyles displayStyle;

		public FontBuilder()
		{
			StringBuilder b = new StringBuilder();

			b.AppendLine("Sample Text");
			b.AppendLine("abcdefghijklm   ABCDEFGHIJKLM");
			b.AppendLine("nopqrstuvwxyz   NOPQRSTUVWXYZ");
			b.AppendLine("01234567890");
			b.AppendLine("!@#$%^&*(),<.>/?;:'\"-_=+\\|");

			mText = b.ToString();

			options.UseTextRenderer = true;
		}

		public Font Font
		{
			get { return font; }
		}

		private void SetStyle(FontStyles fontStyle, bool value)
		{
			if (value)
			{
				options.FontStyle = options.FontStyle | fontStyle;
			}
			else
			{
				options.FontStyle = options.FontStyle & ~fontStyle;
			}

			CreateFont();
		}
		private bool StyleContains(FontStyles fontStyle)
		{
			return (options.FontStyle & fontStyle) == fontStyle;
		}

		public bool Underline
		{
			get { return StyleContains(FontStyles.Underline); }
			set { SetStyle(FontStyles.Underline, value); }
		}
		public bool Strikeout
		{
			get { return StyleContains(FontStyles.Strikeout); }
			set { SetStyle(FontStyles.Strikeout, value); }
		}
		public bool Italic
		{
			get { return StyleContains(FontStyles.Italic); }
			set { SetStyle(FontStyles.Italic, value); }
		}
		public bool Bold
		{
			get { return StyleContains(FontStyles.Bold); }
			set { SetStyle(FontStyles.Bold, value); }
		}

		public IReadOnlyList<int> FontSizes
		{
			get { return fontSizes; }
			set
			{
				fontSizes = value;
				CreateFont();
			}
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

		public Point ZoomLocation { get; set; }

		public object RenderTarget
		{
			get { return mRenderTarget; }
		}
		public string FontFamily
		{
			get { return options.FontFamily; }
			set
			{
				options.FontFamily = value;
				CreateFont();
			}
		}
		public int BottomMarginAdjust
		{
			get { return options.BottomMarginAdjust; }
			set
			{
				options.BottomMarginAdjust = value;
				CreateFont();
			}
		}
		public int TopMarginAdjust
		{
			get { return options.TopMarginAdjust; }
			set
			{
				options.TopMarginAdjust = value;
				CreateFont();
			}
		}

		public Color DisplayColor
		{
			get { return mColor; }
			set { mColor = value; }
		}

		public BitmapFontOptions Options
		{
			get { return options; }
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

		IEnumerable<FontSettings> FontSettings
		{
			get
			{
				foreach (var size in FontSizes)
				{
					yield return new AgateLib.DisplayLib.FontSettings(size, FontStyles.None);

					if (Bold)
						yield return new AgateLib.DisplayLib.FontSettings(size, FontStyles.Bold);
				}
			}
		}

		public int DisplaySize
		{
			get { return displaySize; }
			set
			{
				displaySize = value;
				Draw();
			}
		}

		public FontStyles DisplayStyle
		{
			get { return displayStyle; }
			set
			{
				displayStyle = value;
				Draw();
			}
		}

		public void CreateFont()
		{
			if (string.IsNullOrEmpty(FontFamily))
				return;
			if (font != null)
				font.Dispose();

			font = new Font(FontFamily);

			foreach (var fontSetting in FontSettings)
			{
				options.SizeInPoints = fontSetting.Size;
				options.FontStyle = fontSetting.Style;

				var fontSurface = new FontSurface
					(AgateLib.Platform.WinForms.Fonts.BitmapFontUtil.ConstructFromOSFont(options));

				font.AddFont(fontSetting, fontSurface);
			}

			Draw();
		}

		public void Draw()
		{
			if (zoomWind == null)
				return;

			Display.RenderTarget = zoomWind;
			Display.BeginFrame();
			Display.Clear();

			font.Style = displayStyle;

			font.Size = (int)(displaySize * displayScale);
			((BitmapFontImpl)font.FontSurface.Impl).Surface.InterpolationHint = InterpolationMode.Fastest;

			DrawBackground();
			DrawText(true);

			Display.EndFrame();


			Display.RenderTarget = wind;
			Display.BeginFrame();
			Display.Clear();

			font.Size = displaySize;
			((BitmapFontImpl)font.FontSurface.Impl).Surface.InterpolationHint = InterpolationMode.Nicest;

			DrawBackground();
			DrawText(false);

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

		private void DrawText(bool zoom)
		{
			if (font == null)
				return;

			Point dest = Point.Empty;

			if (zoom)
			{
				var topLeftWindow = new PointF(
					ZoomLocation.X * (float)displayScale - zoomWind.Width / 2,
					ZoomLocation.Y * (float)displayScale - zoomWind.Height / 2);

				if (topLeftWindow.X < 0) topLeftWindow.X = 0;
				if (topLeftWindow.Y < 0) topLeftWindow.Y = 0;

				dest = new Point(-(int)topLeftWindow.X, -(int)topLeftWindow.Y);
			}

			font.Color = DisplayColor;
			font.DrawText(dest, Text);
		}

		public bool SaveFont(string resourceFile, string fontName, string imageFile)
		{
			ResourceDataModel resources;

			if (File.Exists(resourceFile))
				resources = dataLoader.Load(resourceFile);
			else
				resources = new ResourceDataModel();

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

			System.Windows.Forms.MessageBox.Show("Saving not implemented yet.");

			/*
			FontResource res = new FontResource();
			
			res.Name = fontName;
			res.Image = localImagePath;
			res.Metrics = ((BitmapFontImpl)Font.Impl).FontMetrics.Clone();

			if (resources.Fonts.ContainsKey(res.Name))
			{
				if (System.Windows.Forms.MessageBox.Show(
					"The specified resource file already contains a resource named \""
					+ res.Name + "\"." + Environment.NewLine
					+ "Would you like to overwrite it?", res.Name + " already exists",
					System.Windows.Forms.MessageBoxButtons.YesNo,
					System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
				{
					resources.Fonts.Remove(res.Name);
				}
				else
				{
					return false;
				}
			}
			resources.Fonts.Add(res.Name, res);

			AgateResourceLoader.SaveResources(resources, resourceFile);
			
			return true;
			*/
			return false;
		}

		private void SaveImage(string imageFile)
		{
			EnsureDirectoryExists(Path.GetDirectoryName(imageFile));

			((Surface)((BitmapFontImpl)Font.FontSurface.Impl).Surface).SaveTo(imageFile);
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

			bgDark = new Surface("bgdark.png");
			bgLight = new Surface("bglight.png");

			DisplayColor = Color.White;
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