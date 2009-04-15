using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.DisplayLib.Cache;

namespace AgateLib.DisplayLib
{
	public class FontState : ICloneable
	{
		private OriginAlignment mAlignment = OriginAlignment.TopLeft;
		private Color mColor = Color.White;
		private double mScaleWidth = 1.0;
		private double mScaleHeight = 1.0;
		private PointF mLocation;
		private string mText;
		private FontStateCache mCache;

		/// <summary>
		/// Gets or sets the text that is displayed when drawn.
		/// </summary>
		public string Text
		{
			get { return mText; }
			set
			{
				mText = value;

				if (Cache != null)
					Cache.OnTextChanged(this);
			}
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

		#region --- ICloneable Members ---

		public FontState Clone()
		{
			FontState retval = new FontState();

			retval.mAlignment = mAlignment;
			retval.mColor = mColor;
			retval.mScaleWidth = mScaleWidth;
			retval.mScaleHeight = mScaleHeight;
			retval.mLocation = mLocation;
			retval.mText = mText;

			if (mCache != null)
			{
				retval.mCache = mCache.Clone();
			}

			return retval;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion
	}

}