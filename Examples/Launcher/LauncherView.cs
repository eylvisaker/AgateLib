﻿using System;
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

		public ExampleCategories Categories
		{
			get { return categories; }
			set
			{
				categories = value;

				UpdateCategoryListBox();
			}
		}

		private ListBoxItem ListBoxItemAt(int index)
		{
			return (ListBoxItem)lstExamples.Items[index];
		}

		private void UpdateCategoryListBox()
		{
			foreach (var category in categories.Keys)
			{
				lstExamples.Items.Add(new ListBoxItem { Name = category });

				foreach (var example in categories[category].Keys)
				{
					lstExamples.Items.Add(new ListBoxItem
					{
						Name = example,
						Example = categories[category][example]
					});
				}
			}
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
	}
}
