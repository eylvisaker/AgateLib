using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Mathematics.Geometry;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using AgateLib.Platform.WinForms;

namespace FontCreatorApp
{
	public class FontCreator
	{
		private ResourceDataLoader dataLoader = new ResourceDataLoader();

		private string mSampleText;
		private object mRenderTarget;
		private object mZoomRenderTarget;

		FrameBuffer renderTarget;
		DisplayWindow wind;
		DisplayWindow zoomWind;
		Font font;
		Surface bgDark, bgLight;

		private bool mDarkBackground;
		private const int zoomScale = 4;

		private Color mColor;
		private int displaySize = 12;
		private FontStyles displayStyle;

		public FontCreator()
		{
			StringBuilder b = new StringBuilder();

			b.AppendLine("Sample Text");
			b.AppendLine("abcdefghijklm   ABCDEFGHIJKLM");
			b.AppendLine("nopqrstuvwxyz   NOPQRSTUVWXYZ");
			b.AppendLine("01234567890");
			b.AppendLine("!@#$%^&*(),<.>/?;:'\"-_=+\\|");

			mSampleText = b.ToString();
		}

		public FontBuilderParameters Parameters { get; set; } = new FontBuilderParameters();

		public Font Font => font;

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

		public Color DisplayColor
		{
			get { return mColor; }
			set { mColor = value; }
		}

		public string SampleText
		{
			get { return mSampleText; }
			set
			{
				mSampleText = value;
				Draw();
			}
		}

		IEnumerable<FontSettings> FontSettings
		{
			get
			{
				foreach (var size in Parameters.FontSizes)
				{
					yield return new AgateLib.DisplayLib.FontSettings(size, FontStyles.None);

					if (Parameters.Bold)
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
			if (string.IsNullOrEmpty(Parameters.Family))
				return;
			font?.Dispose();

			FontBuilder fontBuilder = new FontBuilder(Parameters.Family);

			foreach (var fontSetting in FontSettings)
			{
				BitmapFontOptions options = CreateBitmapFontOptions(fontSetting);

				var fontSurface = new FontSurface
					(AgateLib.Platform.WinForms.Fonts.BitmapFontUtil.ConstructFromOSFont(options));

				fontBuilder.AddFontSurface(fontSetting, fontSurface);
			}

			font = fontBuilder.Build();

			Draw();
		}

		private BitmapFontOptions CreateBitmapFontOptions(FontSettings fontSetting)
		{
			BitmapFontOptions options = new BitmapFontOptions();

			options.BorderColor = Parameters.BorderColor;
			options.BottomMarginAdjust = Parameters.BottomMarginAdjust;
			options.CreateBorder = Parameters.CreateBorder;
			options.EdgeOptions = Parameters.EdgeOptions;
			options.FontFamily = Parameters.Family;
			options.MonospaceNumbers = Parameters.MonospaceNumbers;
			options.NumberWidthAdjust = Parameters.NumberWidthAdjust;
			options.TopMarginAdjust = Parameters.TopMarginAdjust;
			options.BottomMarginAdjust = Parameters.BottomMarginAdjust;
			options.TextRenderer = Parameters.TextRenderer;

			options.SizeInPoints = fontSetting.Size;
			options.FontStyle = fontSetting.Style;

			return options;
		}

		public void Draw()
		{
			if (zoomWind == null)
				return;

			if (font == null)
				return;

			renderTarget = new FrameBuffer(800, 600);

			Display.RenderTarget = renderTarget;
			Display.BeginFrame();
			Display.Clear(Color.FromArgb(0, 0, 0, 0));

			font.Size = displaySize;
			font.Style = DisplayStyle;
			font.InterpolationHint = InterpolationMode.Nicest;

			DrawText();

			Display.EndFrame();

			Display.RenderTarget = wind.FrameBuffer;
			Display.BeginFrame();
			Display.Clear();

			DrawBackground();
			renderTarget.RenderTarget.Draw();

			Display.EndFrame();

			Display.RenderTarget = zoomWind.FrameBuffer;
			Display.BeginFrame();
			Display.Clear();

			DrawBackground();

			Point dest = Point.Zero;

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

			AgateApp.KeepAlive();
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
			font.DrawText(Point.Zero, SampleText);
		}

		public bool SaveFont(string resourceFile, string fontName, string imageFileRoot)
		{
			if (Path.IsPathRooted(resourceFile) == false)
			{
				resourceFile = Path.Combine(Directory.GetCurrentDirectory(), resourceFile);
			}

			string localImagePartialPath;
			string dir = Path.GetDirectoryName(resourceFile);

			if (Path.IsPathRooted(imageFileRoot) == false)
			{
				localImagePartialPath = imageFileRoot;
				imageFileRoot = Path.Combine(Path.GetDirectoryName(resourceFile), imageFileRoot);
			}
			else
				localImagePartialPath = GetRelativePath(dir, imageFileRoot);

			localImagePartialPath = localImagePartialPath.Replace(Path.DirectorySeparatorChar.ToString(), "/");

			FontResource fontResource = new FontResource();

			foreach (var fs in Font.FontSurfaces)
			{
				var res = new FontSurfaceResource();
				var localImagePath = localImagePartialPath + "-" + fs.Key.ToString() + ".png";
				var impl = (BitmapFontImpl)fs.Value.Impl;

				res.Name = fontName;
				res.Image = localImagePath;
				res.Metrics = impl.FontMetrics.Clone();
				res.Size = fs.Key.Size;
				res.Style = fs.Key.Style;

				var imagePath = Path.Combine(dir, localImagePath);
				var surface = (Surface)impl.Surface;
				surface.SaveTo(imagePath);

				fontResource.Add(res);
			}

			var fonts = new FontResourceCollection { { fontName, fontResource } };
			var resLoader = new ResourceDataSerializer();
			var result = resLoader.Serialize(fonts);

			File.WriteAllText(resourceFile, result);

			return true;
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