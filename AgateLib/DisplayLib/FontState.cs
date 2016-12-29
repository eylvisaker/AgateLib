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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.DisplayLib.Cache;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which represents the state information used to draw texdt on the screen.
	/// </summary>
	public class FontState 
	{
		private OriginAlignment mAlignment = OriginAlignment.TopLeft;
		private Color mColor = Color.White;
		private double mScaleWidth = 1.0;
		private double mScaleHeight = 1.0;
		private PointF mLocation;
		private string mText = string.Empty;
		private FontStateCache mCache;
		private string mTransformedText;

		/// <summary>
		/// Gets or sets the text that is displayed when drawn.
		/// </summary>
		public string Text
		{
			get { return mText; }
			set
			{
				mText = value;
				mTransformedText = string.Empty;

				if (Cache != null)
					Cache.OnTextChanged(this);
			}
		}
		/// <summary>
		/// Gets the text which was transformed by the string transformer.
		/// </summary>
		public string TransformedText
		{
			get { return mTransformedText; }
			internal set { mTransformedText = value; }
		}
		/// <summary>
		/// Gets or sets the location where text is drawn.
		/// </summary>
		public PointF Location
		{
			get { return mLocation; }
			set
			{
				mLocation = value;

				if (Cache != null)
					Cache.OnLocationChanged(this);
			}
		}
		/// <summary>
		/// Sets how to interpret the point given to DrawText methods.
		/// </summary>
		public OriginAlignment DisplayAlignment
		{
			get { return mAlignment; }
			set
			{
				mAlignment = value;

				if (Cache != null)
					Cache.OnDisplayAlignmentChanged(this);
			}
		}
		/// <summary>
		/// Sets the color of the text to be drawn.
		/// </summary>
		public Color Color
		{
			get { return mColor; }
			set
			{
				mColor = value;

				if (Cache != null)
					Cache.OnColorChanged(this);
			}
		}
		/// <summary>
		/// Sets the alpha value of the text to be drawn.
		/// </summary>
		public double Alpha
		{
			get { return mColor.A / 255.0; }
			set
			{
				if (value < 0) value = 0;
				if (value > 1.0) value = 1.0;

				mColor = Color.FromArgb((int)(value * 255), mColor);

				if (Cache != null)
					Cache.OnColorChanged(this);
			}
		}
		/// <summary>
		/// Gets or sets the amount the width is scaled when the text is drawn.
		/// 1.0 is no scaling.
		/// </summary>
		public double ScaleWidth
		{
			get { return mScaleWidth; }
			set
			{
				mScaleWidth = value;

				if (Cache != null)
					Cache.OnScaleChanged(this);
			}
		}
		/// <summary>
		/// Gets or sets the amount the height is scaled when the text is drawn.
		/// 1.0 is no scaling.
		/// </summary>
		public double ScaleHeight
		{
			get { return mScaleHeight; }
			set
			{
				mScaleHeight = value;

				if (Cache != null)
					Cache.OnScaleChanged(this);
			}
		}
		/// <summary>
		/// This value is used by the implementation to optimize rendering this state object.
		/// Do not set this value unless you know what you are doing, or writing an implementation
		/// of FontSurfaceImpl.
		/// </summary>
		public FontStateCache Cache
		{
			get { return mCache; }
			set { mCache = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating how the font should be scaled when drawn.
		/// </summary>
		public InterpolationMode InterpolationHint { get; set; }
		#region --- ICloneable Members ---

		/// <summary>
		/// Returns a deep copy of the FontState object.
		/// </summary>
		/// <returns></returns>
		public FontState Clone()
		{
			FontState result = new FontState();

			result.mAlignment = mAlignment;
			result.mColor = mColor;
			result.mScaleWidth = mScaleWidth;
			result.mScaleHeight = mScaleHeight;
			result.mLocation = mLocation;
			result.mText = mText;
			result.InterpolationHint = InterpolationHint;

			if (mCache != null)
			{
				result.mCache = mCache.Clone();
			}

			return result;
		}

		#endregion
	}

}