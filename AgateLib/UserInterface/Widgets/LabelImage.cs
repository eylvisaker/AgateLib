//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets
{
	public class LabelImage : Container, ITextAlignment
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
			: this()
		{
			Text = text;
			Image = image;
		}

		public string Text { get { return mLabel.Text; } set { mLabel.Text = value; } }
		public IDrawable Image { get { return mImageBox.Image; } set { mImageBox.Image = value; } }

		public Label Label { get { return mLabel; } }
		public ImageBox ImageBox { get { return mImageBox; } }

		public OriginAlignment TextAlign { get { return Label.TextAlign; } set { Label.TextAlign = value; } }
	}
}
