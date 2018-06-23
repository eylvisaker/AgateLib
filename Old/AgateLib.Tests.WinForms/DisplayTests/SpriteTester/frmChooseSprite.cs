using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AgateLib.Resources;

namespace AgateLib.Tests.DisplayTests.SpriteTester
{
	public partial class frmChooseSprite : Form
	{
		AgateResourceManager resources;

		public frmChooseSprite()
		{
			InitializeComponent();
		}

		public string SelectedSprite
		{
			get
			{
				return lstSprites.SelectedItem.ToString();
			}
		}
		public DialogResult ShowDialog(IWin32Window owner, AgateResourceManager resources)
		{
			this.resources = resources;

			lstSprites.Items.Clear();

			foreach (var sprite in resources.Sprites)
			{
				lstSprites.Items.Add(sprite);
			}

			return ShowDialog(owner);
		}
	}
}
