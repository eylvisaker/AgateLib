using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace AgateLib.Gui.ThemeEngines
{
	public class MercuryScheme
	{
		public MercuryScheme()
		{
		}

		public static MercuryScheme CreateDefaultScheme()
		{
			MercuryScheme retval = new MercuryScheme();

			ZipFileProvider provider = new ZipFileProvider("agate-black-gui", InternalResources.DataResources.agate_black_gui);
			retval.SetDefaults(provider);

			return retval;
		}

		void SetDefaults(IFileProvider files)
		{
			WidgetFont = FontSurface.Andika09;
			TitleFont = FontSurface.Andika10;
			CenterTitle = true;

			FontColor = Color.White;
			FontColorDisabled = Color.Gray;
			DropShadowSize = 10;

			WindowNoTitle = new Surface(files, "window_no_title.png");
			WindowWithTitle = new Surface(files, "window_client.png");
			WindowTitleBar = new Surface(files, "window_titlebar.png");
			WindowTitleBarStretchRegion = new Rectangle(6, 3, 52, 27);
			WindowNoTitleStretchRegion = new Rectangle(5, 5, 54, 54);
			WindowWithTitleStretchRegion = new Rectangle(7, 4, 50, 53);

			SetButtonImages(new Surface(files, "button.png"), new Size(64, 32));
			ButtonStretchRegion = new Rectangle(6, 6, 52, 20);
			ButtonTextPadding = 2;
			ButtonMargin = 1;

			SetCheckBoxImages(new Surface(files, "checkbox.png"), new Size(16, 16));
			CheckBoxSpacing = 5;
			CheckBoxMargin = 6;

			SetRadioButtonImages(new Surface(files, "radiobutton.png"), new Size(16, 16));
			RadioButtonSpacing = 5;
			RadioButtonMargin = 2;

			CloseButton = CheckBox;
			CloseButtonInactive = CheckBoxDisabled;
			CloseButtonHover = CheckBoxHover;

			SetTextBoxImages(new Surface(files, "textbox.png"), new Size(64, 16));
			TextBoxMargin = 3;
			TextBoxStretchRegion = new Rectangle(3, 3, 58, 10);
		}


		private void SetTextBoxImages(Surface surface, Size boxSize)
		{
			Surface[] surfs = SplitSurface("textbox", surface, boxSize, 4, 4);

			TextBox = surfs[0];
			TextBoxDisabled = surfs[1];
			TextBoxHover = surfs[2];
			TextBoxFocus = surfs[3];

			surface.Dispose();
		}

		private void SetCheckBoxImages(Surface surface, Size boxSize)
		{
			Surface[] surfs = SplitSurface("checkbox", surface, boxSize, 5, 5);

			CheckBox = surfs[0];
			CheckBoxDisabled = surfs[1];
			CheckBoxCheck = surfs[2];
			CheckBoxHover = surfs[3];
			CheckBoxFocus = surfs[4];

			surface.Dispose();
		}

		private void SetRadioButtonImages(Surface surface, Size boxSize)
		{
			Surface[] surfs = SplitSurface("RadioButton", surface, boxSize, 5, 5);

			RadioButton = surfs[0];
			RadioButtonDisabled = surfs[1];
			RadioButtonCheck = surfs[2];
			RadioButtonHover = surfs[3];
			RadioButtonFocus = surfs[4];

			surface.Dispose();
		}
		private void SetButtonImages(Surface surface, Size buttonSize)
		{
			Surface[] surfs = SplitSurface("button", surface, buttonSize, 6, 6);

			Button = surfs[0];
			ButtonDefault = surfs[1];
			ButtonPressed = surfs[2];
			ButtonDisabled = surfs[3];
			ButtonHover = surfs[4];
			ButtonFocus = surfs[5];

			surface.Dispose();
		}

		private Surface[] SplitSurface(string name, Surface surface, Size subSurfaceSize,
			int requiredImages, int maxImages)
		{
			Point pt = new Point();
			PixelBuffer pixels = surface.ReadPixels();

			List<Surface> retval = new List<Surface>();

			for (int i = 0; i < maxImages; i++)
			{
				Surface surf = new Surface(subSurfaceSize);
				surf.WritePixels(pixels, new Rectangle(pt, subSurfaceSize), Point.Empty);

				retval.Add(surf);

				pt.X += subSurfaceSize.Width;
				if (pt.X == surface.SurfaceWidth)
				{
					pt.X = 0;
					pt.Y += subSurfaceSize.Height;
				}
				else if (pt.X > surface.SurfaceWidth)
				{
					throw new AgateGuiException(
						"Image for " + name +
						" does not have a width that is a multiple of " + subSurfaceSize.Width + ".");
				}

				if (pt.Y + subSurfaceSize.Height > surface.SurfaceHeight)
				{
					if (retval.Count < requiredImages)
						throw new AgateGuiException(
							"There are not enough subimages in the " + name + " image.");
				}
			}

			return retval.ToArray();
		}

		int mInsertionPointBlinkTime = 500;

		public int InsertionPointBlinkTime
		{
			get { return mInsertionPointBlinkTime; }
			set
			{
				if (value < 1)
					throw new ArgumentNullException();

				mInsertionPointBlinkTime = value;
			}
		}
		public FontSurface WidgetFont { get; set; }
		public FontSurface TitleFont { get; set; }
		public bool CenterTitle { get; set; }
		public int DropShadowSize { get; set; }

		public Color FontColor { get; set; }
		public Color FontColorDisabled { get; set; }
		public Surface WindowNoTitle { get; set; }
		public Surface WindowWithTitle { get; set; }
		public Surface WindowTitleBar { get; set; }
		public Rectangle WindowNoTitleStretchRegion { get; set; }
		public Rectangle WindowWithTitleStretchRegion { get; set; }
		public Rectangle WindowTitleBarStretchRegion { get; set; }

		public Surface CloseButton { get; set; }
		public Surface CloseButtonHover { get; set; }
		public Surface CloseButtonInactive { get; set; }

		public Rectangle ButtonStretchRegion { get; set; }
		public Surface Button { get; set; }
		public Surface ButtonDefault { get; set; }
		public Surface ButtonPressed { get; set; }
		public Surface ButtonDisabled { get; set; }
		public Surface ButtonHover { get; set; }
		public Surface ButtonFocus { get; set; }
		public int ButtonTextPadding { get; set; }
		public int ButtonMargin { get; set; }

		public Surface CheckBox { get; set; }
		public Surface CheckBoxDisabled { get; set; }
		public Surface CheckBoxCheck { get; set; }
		public Surface CheckBoxHover { get; set; }
		public Surface CheckBoxFocus { get; set; }
		public int CheckBoxSpacing { get; set; }
		public int CheckBoxMargin { get; set; }

		public Surface RadioButton { get; set; }
		public Surface RadioButtonDisabled { get; set; }
		public Surface RadioButtonCheck { get; set; }
		public Surface RadioButtonHover { get; set; }
		public Surface RadioButtonFocus { get; set; }
		public int RadioButtonSpacing { get; set; }
		public int RadioButtonMargin { get; set; }

		public Surface TextBox { get; set; }
		public Surface TextBoxDisabled { get; set; }
		public Surface TextBoxHover { get; set; }
		public Surface TextBoxFocus { get; set; }
		public Rectangle TextBoxStretchRegion { get; set; }
		public int TextBoxMargin { get; set; }

	}
}
