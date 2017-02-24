using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using Color = AgateLib.DisplayLib.Color;

namespace FontCreatorApp
{
	public partial class SaveFont : UserControl
	{
		AgateLib.DisplayLib.DisplayWindow wind;

		string text;
		private List<FontImageData> fontData;

		public SaveFont()
		{
			InitializeComponent();

			UpdateControls();

			//abcdefghijklmnopqrstuvwxyz
			//ABCDEFGHIJKLMNOPQRSTUVWXYZ
			//01234567890
			//!@#$%^&*(),<.>/?;:'"-_=+\|
			//¡¢£¤¥¦§¨©ª«¬­®¯°±²³µ¶·¸¹º»¼½¾¿À
			//ÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚ
			//ÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷ø
			//ùúûüýþ

			StringBuilder b = new StringBuilder();
			b.AppendLine("The quick brown fox jumped over the lazy dogs.");
			b.AppendLine("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS.");
			b.AppendLine("01234567890");
			b.AppendLine("AV AW VA VW Ta Te Ti Ty Tu Tü Të Tö");
			b.AppendLine("Tantalum");

			text = b.ToString();

			panel1.Paint += new PaintEventHandler(agateRenderTarget1_Paint);

		}

		public AgateLib.DisplayLib.Font AgateFont { get; set; }

		void agateRenderTarget1_Paint(object sender, PaintEventArgs e)
		{
			if (wind == null)
			{
				wind = AgateLib.DisplayLib.DisplayWindow.CreateFromControl(panel1);
			}


			Display.RenderTarget = wind.FrameBuffer;
			Display.BeginFrame();

			Display.Clear(Color.DarkRed);

			AgateFont.Color = Color.White;
			AgateFont.DrawText(text);

			Display.EndFrame();
		}

		public void SetFontData(List<FontImageData> tempFontData)
		{
			fontData = tempFontData;
		}

		public string ResourceFilename
		{
			get { return txtResources.Text; }
		}
		public string ImageFileRoot
		{
			get { return txtImage.Text; }
		}
		public string FontName
		{
			get { return txtFontName.Text; }
			set { txtFontName.Text = value; }
		}
		public bool ValidInput { get; private set; }

		private void btnBrowseResource_Click(object sender, EventArgs e)
		{
			if (dialogResources.ShowDialog(this) != DialogResult.OK)
				return;

			txtResources.Text = dialogResources.FileName;
		}
		private void btnBrowseImage_Click(object sender, EventArgs e)
		{
			if (dialogImage.ShowDialog(this) != DialogResult.OK)
				return;

			txtImage.Text = dialogImage.FileName;
		}

		private void txtResources_TextChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}
		private void txtImage_TextChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}
		private void txtFontName_TextChanged(object sender, EventArgs e)
		{
			UpdateControls();

			txtImage.Text = "Fonts/" + txtFontName.Text;
		}

		private void UpdateControls()
		{
			bool lastValue = ValidInput;
			DetemineValidInput();

			if (lastValue != ValidInput)
				OnValidInputChanged();
		}

		private void OnValidInputChanged()
		{
			if (ValidInputChanged != null)
				ValidInputChanged(this, EventArgs.Empty);
		}

		public event EventHandler ValidInputChanged;

		private void DetemineValidInput()
		{

			ValidInput = false;

			if (txtResources.Text == "") return;
			if (txtImage.Text == "") return;
			if (txtFontName.Text == "") return;

			ValidInput = true;
		}

		internal void ResetControls()
		{
			txtResources.Text = "";
			txtImage.Text = "";
			txtFontName.Text = "";
		}
	}
}
