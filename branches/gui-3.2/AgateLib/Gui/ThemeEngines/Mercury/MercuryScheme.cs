using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryScheme
	{
		int mInsertionPointBlinkTime = 500;
		MercuryWindow mWindow = new MercuryWindow();
		MercuryButton mButton = new MercuryButton();
		MercuryCheckBox mCheckBox = new MercuryCheckBox();
		MercuryCheckBox mRadioButton = new MercuryCheckBox();
		MercuryTextBox mTextBox = new MercuryTextBox();
		MercuryListBox mListBox = new MercuryListBox();

		private MercuryScheme()
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
			
			FontColor = Color.White;
			FontColorDisabled = Color.Gray;
			DropShadowSize = 10;

			SelectionFontColor = Color.Black;
			SelectionBackColor = Color.Yellow;

			Window.NoTitle = new Surface(files, "window_no_title.png");
			Window.WithTitle = new Surface(files, "window_client.png");
			Window.TitleBar = new Surface(files, "window_titlebar.png");
			Window.TitleBarStretchRegion = new Rectangle(6, 3, 52, 27);
			Window.NoTitleStretchRegion = new Rectangle(5, 5, 54, 54);
			Window.WithTitleStretchRegion = new Rectangle(7, 4, 50, 53);

			SetButtonImages(new Surface(files, "button.png"), new Size(64, 32));
			Button.StretchRegion = new Rectangle(6, 6, 52, 20);
			
			SetCheckBoxImages(new Surface(files, "checkbox.png"), new Size(16, 16));
			SetRadioButtonImages(new Surface(files, "radiobutton.png"), new Size(16, 16));

			Window.CloseButton = CheckBox.Image;
			Window.CloseButtonInactive = CheckBox.Disabled;
			Window.CloseButtonHover = CheckBox.Hover;

			SetTextBoxImages(new Surface(files, "textbox.png"), new Size(64, 16));
			TextBox.StretchRegion = new Rectangle(3, 3, 58, 10);

			SetListBoxImages(new Surface(files, "textbox.png"), new Size(64, 16));
			ListBox.StretchRegion = new Rectangle(3, 3, 58, 10);
		}

		private void SetListBoxImages(Surface surface, Size boxSize)
		{
			Surface[] surfs = SplitSurface("Listbox", surface, boxSize, 4, 4);

			ListBox.Image = surfs[0];
			ListBox.Disabled = surfs[1];
			ListBox.Hover = surfs[2];
			ListBox.Focus = surfs[3];

			surface.Dispose();
		}
		private void SetTextBoxImages(Surface surface, Size boxSize)
		{
			Surface[] surfs = SplitSurface("textbox", surface, boxSize, 4, 4);

			TextBox.Image = surfs[0];
			TextBox.Disabled = surfs[1];
			TextBox.Hover = surfs[2];
			TextBox.Focus = surfs[3];

			surface.Dispose();
		}
		private void SetCheckBoxImages(Surface surface, Size boxSize)
		{
			Surface[] surfs = SplitSurface("checkbox", surface, boxSize, 5, 5);

			CheckBox.Image = surfs[0];
			CheckBox.Disabled = surfs[1];
			CheckBox.Check = surfs[2];
			CheckBox.Hover = surfs[3];
			CheckBox.Focus = surfs[4];

			surface.Dispose();
		}
		private void SetRadioButtonImages(Surface surface, Size boxSize)
		{
			Surface[] surfs = SplitSurface("RadioButton", surface, boxSize, 5, 5);

			RadioButton.Image = surfs[0];
			RadioButton.Disabled = surfs[1];
			RadioButton.Check = surfs[2];
			RadioButton.Hover = surfs[3];
			RadioButton.Focus = surfs[4];

			surface.Dispose();
		}
		private void SetButtonImages(Surface surface, Size buttonSize)
		{
			Surface[] surfs = SplitSurface("button", surface, buttonSize, 6, 6);

			Button.Image = surfs[0];
			Button.Default = surfs[1];
			Button.Pressed = surfs[2];
			Button.Disabled = surfs[3];
			Button.Hover = surfs[4];
			Button.Focus = surfs[5];

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

		public int InsertionPointHeight
		{
			get { return WidgetFont.FontHeight - 2; }
		}
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
		public int DropShadowSize { get; set; }

		public Color FontColor { get; set; }
		public Color FontColorDisabled { get; set; }
		public Color SelectionFontColor { get; set; }
		public Color SelectionBackColor { get; set; }

		public MercuryWindow Window 
		{
			get { return mWindow; }
			set
			{
				if (value == null) throw new ArgumentNullException();
				mWindow = value;
			}
		}
		public MercuryButton Button
		{
			get { return mButton; }
			set
			{
				if (value == null) throw new ArgumentNullException();
				mButton = value;
			}
		}
		public MercuryTextBox TextBox
		{
			get { return mTextBox; }
			set
			{
				if (value == null) throw new ArgumentNullException();
				mTextBox = value;
			}
		}
		public MercuryListBox ListBox
		{
			get { return mListBox; }
			set
			{
				if (value == null) throw new ArgumentNullException();
				mListBox = value;
			}
		}
		public MercuryCheckBox CheckBox
		{
			get { return mCheckBox; }
			set
			{
				if (value == null) throw new ArgumentNullException();
				mCheckBox = value;
			}
		}
		public MercuryCheckBox RadioButton
		{
			get { return mRadioButton; }
			set
			{
				if (value == null) throw new ArgumentNullException();
				mRadioButton = value;
			}
		}
		
	}

	public class MercuryWindow
	{
		public Surface NoTitle { get; set; }
		public Surface WithTitle { get; set; }
		public Surface TitleBar { get; set; }
		public Rectangle NoTitleStretchRegion { get; set; }
		public Rectangle WithTitleStretchRegion { get; set; }
		public Rectangle TitleBarStretchRegion { get; set; }

		public bool CenterTitle { get; set; }

		public Surface CloseButton { get; set; }
		public Surface CloseButtonHover { get; set; }
		public Surface CloseButtonInactive { get; set; }

		public MercuryWindow()
		{
			CenterTitle = true;
		}
	}
	public class MercuryButton
	{
		public Rectangle StretchRegion { get; set; }
		public Surface Image { get; set; }
		public Surface Default { get; set; }
		public Surface Pressed { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public int TextPadding { get; set; }
		public int Margin { get; set; }
	}
	public class MercuryTextBox
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public Rectangle StretchRegion { get; set; }
		public int Margin { get; set; }

		public MercuryTextBox()
		{
			Margin = 3;
		}
	}
	public class MercuryListBox
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public Rectangle StretchRegion { get; set; }
		public int Margin { get; set; }

		public MercuryListBox()
		{
			Margin = 3;
		}
	}
	public class MercuryCheckBox
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Check { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public int Spacing { get; set; }
		public int Margin { get; set; }

		public MercuryCheckBox()
		{
			Spacing = 5;
			Margin = 2;
		}
	}
}
