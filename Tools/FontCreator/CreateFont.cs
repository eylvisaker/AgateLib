using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AgateLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Platform.WinForms;
using System.Linq;

namespace FontCreator
{
	public partial class CreateFont : UserControl
	{
		FontBuilder builder;

		bool AnyDesignMode
		{
			get
			{
				Control p = this;

				do
				{
					if (p.Site != null && p.Site.DesignMode)
						return true;

					p = p.Parent;

				} while (p != null);

				return false;
			}
		}

		public CreateFont()
		{
			InitializeComponent();
		}

		public FontBuilder FontBuilder { get { return builder; } }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (AnyDesignMode)
				return;

			InitializeBuilder();
			InitializeSizeList();
			InitializeFontList();
			InitializeControls();
		}

		private void InitializeControls()
		{
			txtSampleText_TextChanged(null, null);

			foreach (BitmapFontEdgeOptions opt in
				Enum.GetValues(typeof(BitmapFontEdgeOptions)))
			{
				cboEdges.Items.Add(opt);
			}

			cboEdges.SelectedItem = BitmapFontEdgeOptions.IntensityAlphaWhite;

			cboBg.SelectedIndex = 0;
		}

		private void InitializeFontList()
		{
			int index = 0;

			List<string> fonts = new List<string>();
			foreach (FontFamily fam in FontFamily.Families)
			{
				fonts.Add(fam.Name);
			}
			fonts.Sort();

			foreach (string family in fonts)
			{
				if (family == "Bitstream Vera Sans" || family == "Arial")
					index = cboFamily.Items.Count;

				cboFamily.Items.Add(family);
			}


			cboFamily.SelectedIndex = index;
		}

		private void InitializeBuilder()
		{
			builder = new FontBuilder();

			builder.SetRenderTarget(renderTarget, zoomRenderTarget);
		}

		private void InitializeSizeList()
		{
			lstSizes.Items.Clear();
			lstSizes.Items.AddRange(builder.FontSizes.Cast<object>().ToArray());
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (builder == null)
				return;

			builder.Draw();
		}

		private void cboFamily_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.FontFamily = cboFamily.SelectedItem.ToString();
		}

		private void nudSize_ValueChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.FontSizes = lstSizes.Items.Cast<string>().Select(int.Parse).ToList();
		}

		private void chkBold_CheckedChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.Bold = chkBold.Checked;
		}
		private void chkItalic_CheckedChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.Italic = chkItalic.Checked;
		}
		private void chkUnderline_CheckedChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.Underline = chkUnderline.Checked;
		}
		private void chkStrikeout_CheckedChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.Strikeout = chkStrikeout.Checked;
		}
		private void txtSampleText_TextChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.Text = txtSampleText.Text;
		}

		private void nudTopMargin_ValueChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.TopMarginAdjust = (int)nudTopMargin.Value;
		}

		private void nudBottomMargin_ValueChanged(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.BottomMarginAdjust = (int)nudBottomMargin.Value;
		}
		private void renderTarget_Resize(object sender, EventArgs e)
		{
			if (builder == null)
				return;

			builder.Draw();
		}

		private void btnBorderColor_Click(object sender, EventArgs e)
		{
			colorDialog.Color = btnBorderColor.BackColor;

			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				btnBorderColor.BackColor = colorDialog.Color;

				builder.Options.BorderColor = ConvertColor(colorDialog.Color);
				builder.Options.BorderColor = AgateLib.Geometry.Color.FromArgb((int)nudOpacity.Value, builder.Options.BorderColor);

				builder.Options.CreateBorder = true;

				chkBorder.Checked = true;

				builder.CreateFont();
			}
		}
		private void nudOpacity_ValueChanged(object sender, EventArgs e)
		{
			builder.Options.BorderColor = AgateLib.Geometry.Color.FromArgb((int)nudOpacity.Value, builder.Options.BorderColor);

			if (chkBorder.Checked)
			{
				builder.CreateFont();
			}
		}

		private AgateLib.Geometry.Color ConvertColor(System.Drawing.Color clr)
		{
			return AgateLib.Geometry.Color.FromArgb(clr.R, clr.G, clr.B);
		}

		private void chkTextRenderer_CheckedChanged(object sender, EventArgs e)
		{
			builder.Options.UseTextRenderer = chkTextRenderer.Checked;
			builder.CreateFont();
		}

		private void cboBg_SelectedIndexChanged(object sender, EventArgs e)
		{
			builder.LightBackground = cboBg.SelectedIndex == 1;

		}

		private void btnDisplayColor_Click(object sender, EventArgs e)
		{
			colorDialog.Color = btnDisplayColor.BackColor;

			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				btnDisplayColor.BackColor = colorDialog.Color;

				builder.DisplayColor = ConvertColor(colorDialog.Color);
			}
		}

		private void chkBorder_CheckedChanged(object sender, EventArgs e)
		{
			builder.Options.CreateBorder = chkBorder.Checked;

			builder.CreateFont();
		}

		private void cboEdges_SelectedIndexChanged(object sender, EventArgs e)
		{
			builder.Options.EdgeOptions = (BitmapFontEdgeOptions)cboEdges.SelectedItem;

			builder.CreateFont();
		}

		private void chkMonospaceNumbers_CheckedChanged(object sender, EventArgs e)
		{
			builder.Options.MonospaceNumbers = chkMonospaceNumbers.Checked;

			builder.CreateFont();
		}

		private void nudNumberWidthAdjust_ValueChanged(object sender, EventArgs e)
		{
			builder.Options.NumberWidthAdjust = (int)nudNumberWidthAdjust.Value;

			builder.CreateFont();
		}

		private void renderTarget_MouseMove(object sender, MouseEventArgs e)
		{
			builder.ZoomLocation = e.Location.ToAgatePoint();
			builder.Draw();
		}

		private void nudDisplaySize_ValueChanged(object sender, EventArgs e)
		{
			builder.DisplaySize = (int)nudDisplaySize.Value;
		}

		private void chkDisplayBold_CheckedChanged(object sender, EventArgs e)
		{
			if (chkDisplayBold.Checked)
			{
				builder.DisplayStyle = AgateLib.DisplayLib.FontStyles.Bold;
			}
			else
			{
				builder.DisplayStyle = AgateLib.DisplayLib.FontStyles.None;
			}
		}

		private void btnGenerateFont_Click(object sender, EventArgs e)
		{
			builder.CreateFont();
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int item = (int)lstSizes.SelectedItem;

			builder.FontSizes.Remove(item);
			InitializeSizeList();
		}

		private void lstSizes_SelectedIndexChanged(object sender, EventArgs e)
		{
			removeToolStripMenuItem.Enabled = lstSizes.SelectedIndex >= 0;
		}

		private void txtAddSize_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				int value;
				if (int.TryParse(txtAddSize.Text, out value))
				{
					builder.FontSizes.Add(value);
					builder.FontSizes.Sort();

					InitializeSizeList();
					txtAddSize.Text = "";
				}
				else
				{
					System.Media.SystemSounds.Beep.Play();
				}
			}
		}
	}
}