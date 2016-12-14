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

		FrameBuffer renderTarget;
		DisplayWindow wind;
		DisplayWindow zoomWind;
		Font font;
		Surface bgDark, bgLight;
		List<int> fontSizes = new List<int> { 8, 9, 10, 12, 14, 16, 18, 24, 30 };

		BitmapFontOptions options = new BitmapFontOptions();

		private bool mDarkBackground;
		private const int zoomScale = 4;

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

		public bool Underline { get; set; }
		public bool Strikeout { get; set; }
		public bool Italic { get; set; }
		public bool Bold { get; set; } = true;

		public List<int> FontSizes
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

			renderTarget = new FrameBuffer(800, 600);

			Display.RenderTarget = renderTarget;
			Display.BeginFrame();
			Display.Clear(Color.FromArgb(0, 0, 0, 0));

			font.Size = displaySize;
			((BitmapFontImpl)font.FontSurface.Impl).Surface.InterpolationHint = InterpolationMode.Nicest;

			DrawText();

			Display.EndFrame();

			Display.RenderTarget = wind;
			Display.BeginFrame();
			Display.Clear();

			DrawBackground();
			renderTarget.RenderTarget.Draw();

			Display.EndFrame();

			Display.RenderTarget = zoomWind;
			Display.BeginFrame();
			Display.Clear();

			DrawBackground();

			Point dest = Point.Empty;

			var topLeftWindow = new PointF(
				ZoomLocation.X * (float)zoomScale - zoomWind.Width / 2,
				ZoomLocation.Y * (float)zoomScale - zoomWind.Height / 2);

			if (topLeftWindow.X < 0) topLeftWindow.X = 0;
			if (topLeftWindow.Y < 0) topLeftWindow.Y = 0;

			dest = new Point(-(int)topLeftWindow.X, -(int)topLeftWindow.Y);

			renderTarget.RenderTarget.InterpolationHint = InterpolationMode.Fastest;
			renderTarget.RenderTarget.Draw(
				new Rectangle(dest.X, dest.Y,
				renderTarget.Width * zoomScale,
				renderTarget.Height * zoomScale));


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
			font.DrawText(Point.Empty, Text);
		}

		public bool SaveFont(string resourceFile, string fontName, string imageFileRoot)
		{
			if (Path.IsPathRooted(resourceFile) == false)
			{
				resourceFile = Path.Combine(Directory.GetCurrentDirectory(), resourceFile);
			}

			string localImagePath;
			string dir = Path.GetDirectoryName(resourceFile);

			if (Path.IsPathRooted(imageFileRoot) == false)
			{
				localImagePath = imageFileRoot;
				imageFileRoot = Path.Combine(Path.GetDirectoryName(resourceFile), imageFileRoot);
			}
			else
				localImagePath = GetRelativePath(dir, imageFileRoot);

			SaveImage(imageFileRoot);

			localImagePath = localImagePath.Replace(Path.DirectorySeparatorChar.ToString(), "/");

			FontResource fontResource = new FontResource();

			foreach (var fs in Font.FontItems)
			{
				var res = new FontSurfaceResource();
				var imagePath = localImagePath + fs.Key.ToString() + ".png";

				res.Name = fontName;
				res.Image = imagePath;
				res.Metrics = ((BitmapFontImpl)fs.Value.Impl).FontMetrics.Clone();

				fontResource.Add(res);
			}

			var fonts = new FontResourceCollection { { fontName, fontResource } };
			var resLoader = new ResourceDataSerializer();
			var result = resLoader.Serialize(fonts);

			File.WriteAllText(resourceFile, result);

			return true;
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