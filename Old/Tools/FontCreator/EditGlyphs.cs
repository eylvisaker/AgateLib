using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Platform.WinForms;

namespace FontCreatorApp
{
	public partial class EditGlyphs : UserControl
	{
		Image image;
		FontMetrics font;
		int zoomTop, zoomLeft;
		float scale = 4f;
		bool dragSource;
		bool dragOverhang;
		int overHangRelative;
		private List<FontImageData> fontData;

		public EditGlyphs()
		{
			InitializeComponent();

			mouseLabel.Text = "";
		}

		private void SetFont(FontImageData item)
		{
			SetFont(item.Filename, item.Metrics);
		}

		private void SetFont(string tempImage, FontMetrics fontMetrics)
		{
			using (var stream = File.OpenRead(tempImage))
			{
				image = new Bitmap(stream);
			}

			font = fontMetrics;

			lstItems.Items.Clear();

			foreach (char key in font.Keys)
			{
				lstItems.Items.Add(key);
			}

			pctImage.Invalidate();
			pctZoom.Invalidate();
		}

		private void pctImage_Paint(object sender, PaintEventArgs e)
		{
			if (image == null)
				return;

			PaintFontImage(e.Graphics);
		}
		private void pctZoom_Paint(object sender, PaintEventArgs e)
		{
			if (lstItems.SelectedItem == null)
			{
				e.Graphics.Clear(Color.White);
				return;
			}

			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.Transform = new System.Drawing.Drawing2D.Matrix(scale, 0, 0, scale, -zoomLeft * scale, -zoomTop * scale);

			PaintFontImage(e.Graphics);
		}

		public void SetFontData(List<FontImageData> fontData)
		{
			this.fontData = fontData;

			fontDropDown.DropDownItems.Clear();

			foreach(var item in fontData)
			{
				var menuItem = new ToolStripMenuItem(item.Settings.ToString());
				menuItem.Tag = item;
				menuItem.Click += MenuItem_Click;

				fontDropDown.DropDownItems.Add(menuItem);
			}

			SetFont(fontData.First());
		}

		private void MenuItem_Click(object sender, EventArgs e)
		{
			var item = (FontImageData)((ToolStripMenuItem)sender).Tag;

			SetFont(item);
		}


		private void PaintFontImage(Graphics g)
		{
			g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, image.Width, image.Height));

			if (lstItems.SelectedIndex != -1)
			{
				GlyphMetrics metric = SelectedMetric;
				Rectangle r = metric.SourceRect.ToDrawing();

				
				Rectangle glyph = r;
				glyph.X += metric.LeftOverhang;
				glyph.Width -= metric.LeftOverhang;
				glyph.Width -= metric.RightOverhang;

				g.DrawRectangle(Pens.Red, r);
				g.DrawRectangle(Pens.Blue, glyph);
			}
			g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));

		}

		GlyphMetrics SelectedMetric
		{
			get
			{
				if (lstItems.SelectedItem == null)
					return null;

				char glyph = (char)lstItems.SelectedItem;
				GlyphMetrics metric = font[glyph];

				return metric;
			}
		}
		Rectangle? SelectedRect
		{
			get
			{
				if (lstItems.SelectedItem == null)
					return null;

				return font[(char)lstItems.SelectedItem].SourceRect.ToDrawing();
			}
		}

		private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			
			pctImage.Invalidate();
			pctZoom.Invalidate();

			if (lstItems.SelectedItem == null)
			{
				properties.SelectedObject = null;
				return;
			}
			else
			{
				properties.SelectedObject = font[(char)lstItems.SelectedItem];
			}

			SetZoomOrigin();

		}

		private void SetZoomOrigin()
		{
			pctZoom.Invalidate();

			if (SelectedMetric == null)
				return;

			int width = (int)(pctZoom.Width / scale);
			int height = (int)(pctZoom.Height / scale);

			Rectangle r = font[(char)lstItems.SelectedItem].SourceRect.ToDrawing();

			zoomLeft = (int)(r.X + r.Width / 2 - width / 2);
			zoomTop = (int)(r.Y + r.Height / 2 - height / 2);

			if (zoomLeft < 0) zoomLeft = 0;
			if (zoomTop < 0) zoomTop = 0;
			if (zoomLeft + width > image.Width) zoomLeft = image.Width - width;
			if (zoomTop + height > image.Height) zoomTop = image.Height - height;

		}

		private void properties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			pctImage.Invalidate();
			pctZoom.Invalidate();
		}

		Point LocalMouseCoord(Point zoomLocation)
		{
			Point retval = new Point((int)(zoomLocation.X / scale), (int)(zoomLocation.Y / scale));
			retval.X += zoomLeft;
			retval.Y += zoomTop;
			return retval;
		}
		private void pctZoom_MouseMove(object sender, MouseEventArgs e)
		{
			if (SelectedRect == null)
				return;

			Point local = LocalMouseCoord(e.Location);
			Rectangle r = SelectedRect.Value;

			if (dragSource)
			{
				int width = local.X - r.Left;

				SelectedMetric.Width = width;

				if (dragOverhang)
				{
					SelectedMetric.RightOverhang = width - overHangRelative;
				}

				pctZoom.Refresh();
				properties.Refresh();
			}
			else if (local.X == r.Right)
			{
				if (local.Y < r.Top + r.Height / 2)
				{
					Cursor.Current = Cursors.PanEast;
					mouseLabel.Text = "Change source rect but keep glyph size the same";
				}
				else
				{
					Cursor.Current = Cursors.SizeWE;
					mouseLabel.Text = "Change source rect and glyph size";
				}
			}
			else
			{
				Cursor.Current = Cursors.Arrow;
				mouseLabel.Text = "";
			}
		}

		private void pctZoom_MouseUp(object sender, MouseEventArgs e)
		{
			dragSource = false;
			dragOverhang = false;
		}

		private void pctZoom_MouseDown(object sender, MouseEventArgs e)
		{
			if (SelectedRect == null)
				return;

			Point local = LocalMouseCoord(e.Location);
			Rectangle r = SelectedRect.Value;

			if (local.X == r.Right)
			{
				if (local.Y < r.Top + r.Height / 2)
				{
					dragSource = true;
					dragOverhang = true;
					overHangRelative = SelectedMetric.Width - SelectedMetric.RightOverhang;
				}
				else
				{
					dragSource = true;
				}
			}
		}

		private void btnZoomIn_Click(object sender, EventArgs e)
		{
			scale += 1;

			btnZoomOut.Enabled = true;


			SetZoomOrigin();
			pctZoom.Invalidate();
		}

		private void btnZoomOut_Click(object sender, EventArgs e)
		{
			if (scale > 1)
				scale -= 1;
			else
			{
				scale = 1;
				btnZoomOut.Enabled = false;
			}

			SetZoomOrigin();
			pctZoom.Invalidate();
		}

		private void pctZoom_Resize(object sender, EventArgs e)
		{
			SetZoomOrigin();
			
		}

		private void pctZoom_MouseLeave(object sender, EventArgs e)
		{
			mouseLabel.Text = "Press + or - on numeric keypad to modify right overhang.";
		}

		private void lstItems_KeyDown(object sender, KeyEventArgs e)
		{
			if (lstItems.SelectedItem == null)
				return;

			bool accepted = false;

			if (e.KeyCode == Keys.Add)
			{
				SelectedMetric.RightOverhang++;

				accepted = true;
			}
			else if (e.KeyCode == Keys.Subtract)
			{
				SelectedMetric.RightOverhang--;

				accepted = true;
			}

			if (accepted)
			{
				pctZoom.Refresh();
				properties.Refresh();

				e.SuppressKeyPress = true;
			}
		}


	}
}