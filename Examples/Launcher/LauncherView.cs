using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examples.Launcher
{
	public partial class LauncherView : Form, ILauncherView
	{
		class ListBoxItem
		{
			public string Name { get; set; }

			public ExampleItem Example { get; set; }
		}

		ExampleCategories categories;
		Font boldFont;

		public LauncherView()
		{
			InitializeComponent();

			boldFont = new Font(lstExamples.Font, FontStyle.Bold);
		}

		public event EventHandler<ExampleEventArgs> SelectedExampleChanged;
		public event EventHandler<ExampleEventArgs> LaunchExample;

		public string Arguments => cboArgs.Text;

		public ExampleCategories Categories
		{
			get { return categories; }
			set
			{
				categories = value;

				UpdateCategoryListBox();
			}
		}

		public Image Image
		{
			get { return pctImage.Image; }
			set
			{
				pctImage.Image = value;

				if (value != null)
				{
					if (value.Width > pctImage.Width ||
					    value.Height > pctImage.Height)
					{
						pctImage.SizeMode = PictureBoxSizeMode.Zoom;
					}
					else
					{
						pctImage.SizeMode = PictureBoxSizeMode.CenterImage;
					}
				}
			}
		}

		private ListBoxItem ListBoxItemAt(int index)
		{
			return (ListBoxItem)lstExamples.Items[index];
		}

		private void UpdateCategoryListBox()
		{
			int selection = -1;

			lstExamples.Items.Clear();

			foreach (var category in categories.Keys)
			{
				lstExamples.Items.Add(new ListBoxItem { Name = category });

				foreach (var example in categories[category].Keys)
				{
					var item = categories[category][example];

					lstExamples.Items.Add(new ListBoxItem
					{
						Name = item.Name,
						Example = item
					});

					if (selection == -1)
						selection = lstExamples.Items.Count - 1;
				}
			}

			lstExamples.SelectedIndex = selection;
		}

		private void lstExamples_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();

			if (e.Index == -1)
			{
				return;
			}
			ListBoxItem item = ListBoxItemAt(e.Index);

			using (var brush = new SolidBrush(e.ForeColor))
			{
				var font = item.Example != null ? lstExamples.Font : boldFont;
				var text = item.Example != null ? $"    {item.Name}" : item.Name;

				e.Graphics.DrawString(text, font, brush, e.Bounds);
			}

			if (e.Index == lstExamples.SelectedIndex)
				e.DrawFocusRectangle();
		}

		private void lstExamples_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			var size = e.Graphics.MeasureString(ListBoxItemAt(e.Index).Name, boldFont);

			e.ItemHeight = (int)size.Height;
			e.ItemWidth = (int)size.Width;
		}

		private void lstExamples_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstExamples.SelectedIndex == -1)
			{
				btnLaunch.Enabled = false;

				SelectedExampleChanged?.Invoke(this, new ExampleEventArgs(null));
				return;
			}

			var item = ListBoxItemAt(lstExamples.SelectedIndex);

			SelectedExampleChanged?.Invoke(this, new ExampleEventArgs(item.Example));

			btnLaunch.Enabled = item.Example != null;
		}

		private void btnLaunch_Click(object sender, EventArgs e)
		{
			LaunchSelectedExample();
		}

		private void lstExamples_DoubleClick(object sender, EventArgs e)
		{
			LaunchSelectedExample();
		}

		private void LaunchSelectedExample()
		{
			if (lstExamples.SelectedIndex == -1)
				return;

			var item = ListBoxItemAt(lstExamples.SelectedIndex);

			if (item.Example == null)
				return;

			LaunchExample?.Invoke(this, new ExampleEventArgs(item.Example));
		}
	}
}
