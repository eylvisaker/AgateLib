using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets
{
	public class LabelImage : Container
	{
		Label mLabel;
		ImageBox mImageBox;

		public LabelImage()
		{
			mLabel = new Label();
			mImageBox = new ImageBox();

			Children.Add(mImageBox);
			Children.Add(mLabel);
		}
		public LabelImage(string text, IDrawable image)
			:this()
		{
			Text = text;
			Image = image;
		}

		public string Text { get { return mLabel.Text; } set { mLabel.Text = value; } }
		public IDrawable Image { get { return mImageBox.Image; } set { mImageBox.Image = value; } }

		public Label Label { get { return mLabel; } }
		public ImageBox ImageBox { get { return mImageBox; } }
	}
}
