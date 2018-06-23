using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Platform.WinForms;
using AgateLib.Platform.WinForms.Controls;
using AgateLib.Quality;

namespace FontCreatorApp
{
	public partial class frmFontCreator : Form
	{
		int mCurrentPage;
		List<FontImageData> tempFontData = new List<FontImageData>();

		public frmFontCreator()
		{
			InitializeComponent();

			Icon = FormUtil.AgateLibIcon;
			CurrentPage = 1;
		}

		AgateLib.DisplayLib.Font AgateFont
		{
			get { return this.createFont1.FontCreator.Font; }
		}

		int CurrentPage
		{
			get { return mCurrentPage; }
			set
			{
				pnlCreateFont.Visible = false;
				pnlEditGlyphs.Visible = false;
				pnlSaveFont.Visible = false;

				Panel pnl = null;

				switch (value)
				{
					case 1:
						pnl = pnlCreateFont;
						break;

					case 2:
						CreateTempFontData();

						editGlyphs1.SetFontData(tempFontData);

						pnl = pnlEditGlyphs;
						break;

					case 3:
						pnl = pnlSaveFont;
						saveFont1.AgateFont = createFont1.FontCreator.Font;
						saveFont1.SetFontData(tempFontData);
						saveFont1.FontName = createFont1.FontCreator.Parameters.Family;

						break;

					default:
						throw new InvalidOperationException("Wrong page number!");
				}

				pnl.Dock = DockStyle.Fill;
				pnl.Visible = true;

				mCurrentPage = value;

				if (mCurrentPage == 1)
					btnPrevious.Enabled = false;
				else
					btnPrevious.Enabled = true;

				if (mCurrentPage == 3)
				{
					btnNext.Enabled = saveFont1.ValidInput;
					btnNext.Text = "Finish";
				}
				else
				{
					btnNext.Enabled = true;
					btnNext.Text = "Next >>";
				}
			}
		}

		private void CreateTempFontData()
		{
			ClearTempData();
			Require.True<InvalidOperationException>(tempFontData.Count == 0,
				"tempFontData should be empty after calling ClearTempData!");

			foreach (var fs in AgateFont.FontSurfaces)
			{
				string tempImage = Path.GetTempFileName() + ".png";
				var bfi = ((BitmapFontImpl)fs.Value.Impl);

				((Surface)bfi.Surface).SaveTo(tempImage);

				FontImageData fid = new FontImageData
				{
					Filename = tempImage,
					Metrics = bfi.FontMetrics,
					Settings = fs.Key,
				};

				tempFontData.Add(fid);
			}
		}

		public bool SaveFont()
		{
			return createFont1.FontCreator.SaveFont(
				saveFont1.ResourceFilename,
				saveFont1.FontName,
				saveFont1.ImageFileRoot);
		}

		private void btnPrevious_Click(object sender, EventArgs e)
		{
			CurrentPage--;

		}
		private void btnNext_Click(object sender, EventArgs e)
		{
			if (CurrentPage == 3)
			{
				if (SaveFont() == false)
					return;

				switch (MessageBox.Show(this,
					"Successfully saved font.  Create a new font?" + Environment.NewLine +
					"Click yes to start over, no to quit.", "Font Complete", 
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Information, 
					MessageBoxDefaultButton.Button2))
				{
					case DialogResult.Yes:
						saveFont1.ResetControls();
						CurrentPage = 1;
						break;

					case DialogResult.No:
						this.Close();
						break;
				}

				return;
			}

			CurrentPage++;

		}

		private void btnPrevious_MouseEnter(object sender, EventArgs e)
		{
			if (CurrentPage == 2)
				pnlWarning.Visible = true;
		}
		private void btnPrevious_MouseLeave(object sender, EventArgs e)
		{
			pnlWarning.Visible = false;
		}

		private void saveFont1_ValidInputChanged(object sender, EventArgs e)
		{
			btnNext.Enabled = saveFont1.ValidInput;
			btnNext.Text = "Finish";
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			ClearTempData();
		}

		private void ClearTempData()
		{
			foreach(var item in tempFontData)
			{
				File.Delete(item.Filename);
			}

			tempFontData.Clear();
		}
	}
}
