//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Cache;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Surface state data used for drawing.
	/// </summary>
	public class SurfaceState
	{
		private double mScaleWidth = 1.0;
		private double mScaleHeight = 1.0;
		private OriginAlignment mAlignment = OriginAlignment.TopLeft;
		private double mRotation = 0;
		private OriginAlignment mRotationSpot = OriginAlignment.Center;
		private PointF mRotationCenter = new PointF();
		private Gradient mGradient = new Gradient(Color.White);
		private DrawInstanceList mDrawInstances = new DrawInstanceList();

		/// <summary>
		/// Constructs a SurfaceState object.
		/// </summary>
		public SurfaceState()
		{
			mDrawInstances.Add(new SurfaceDrawInstance());
		}
		/// <summary>
		/// Performs a deep copy of this SurfaceState object.
		/// </summary>
		/// <returns></returns>
		public SurfaceState Clone()
		{
			SurfaceState result = new SurfaceState();
			CopyTo(result, true);

			return result;
		}
		/// <summary>
		/// Copies data from this SurfaceState to the destination state.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="copyDrawInstances"></param>
		public void CopyTo(SurfaceState target, bool copyDrawInstances)
		{
			target.ScaleWidth = mScaleWidth;
			target.ScaleHeight = mScaleHeight;
			target.DisplayAlignment = mAlignment;
			target.RotationAngle = mRotation;
			target.RotationCenter = mRotationSpot;
		    target.RotationCenterLocation = mRotationCenter;
			target.ColorGradient = mGradient;

			if (copyDrawInstances)
			{
				target.mDrawInstances.Clear();
				target.mDrawInstances.Capacity = mDrawInstances.Count;
				target.mDrawInstances.AddRange(mDrawInstances);
			}
		}

		/// <summary>
		/// Location the surface will be drawn.
		/// </summary>
		public DrawInstanceList DrawInstances
		{
			get { return mDrawInstances; }
			set { mDrawInstances = value; }
		}

		/// <summary>
		/// Alpha value for displaying this surface.
		/// Valid values range from 0.0 (completely transparent) to 1.0 (completely opaque).
		/// Internally stored as a byte, so granularity is only 1/255.0.
		/// If a gradient is used, getting this property returns the alpha value for the top left
		/// corner of the gradient.
		/// </summary>
		public double Alpha
		{
			get { return Color.A / 255.0; }
			set
			{
				mGradient.SetAlpha(value);
			}
		}
		/// <summary>
		/// Gets or sets the rotation angle in radians.
		/// Positive angles indicate rotation in the Counter-Clockwise direction.
		/// </summary>
		public double RotationAngle
		{
			get { return mRotation; }
			set { mRotation = value % (2 * Math.PI); }
		}
		/// <summary>
		/// Gets or sets the rotation angle in degrees.
		/// Positive angles indicate rotation in the Counter-Clockwise direction.
		/// </summary>
		public double RotationAngleDegrees
		{
			get { return RotationAngle * 180.0 / Math.PI; }
			set { RotationAngle = value * Math.PI / 180.0; }
		}
		/// <summary>
		/// Gets or sets the point on the surface which is used to rotate around.
		/// </summary>
		public OriginAlignment RotationCenter
		{
			get { return mRotationSpot; }
			set { mRotationSpot = value; }
		}
		/// <summary>
		/// Specifies the point in screen space the surface is rotated around.  This value
		/// is only used if RotationCenter is set to OriginAlignment.Specified.  Setting this
		/// value will set the RotationCenter to OriginAlignment.Specified.
		/// </summary>
		public PointF RotationCenterLocation
		{
			get { return mRotationCenter; }
			set
			{
				mRotationSpot = OriginAlignment.Specified;
				mRotationCenter = value;
			}
		}
		/// <summary>
		/// Gets or sets the point where the surface is aligned to when drawn.
		/// </summary>
		public OriginAlignment DisplayAlignment
		{
			get { return mAlignment; }
			set { mAlignment = value; }
		}

		/// <summary>
		/// Gets or sets the amount the width is scaled when this surface is drawn.
		/// 1.0 is no scaling.
		/// Scale values can be negative, this causes the surface to be mirrored
		/// in that direction.  This does not affect how the surface is aligned;
		/// eg. if DisplayAlignment is top-left and ScaleWidth &lt; 0, the surface 
		/// will still be drawn to the right of the point supplied to Draw(Point).
		/// </summary>
		public double ScaleWidth
		{
			get { return mScaleWidth; }
			set { mScaleWidth = value; }
		}
		/// <summary>
		/// Gets or sets the amount the height is scaled when this surface is drawn.
		/// 1.0 is no scaling.
		/// </summary>
		public double ScaleHeight
		{
			get { return mScaleHeight; }
			set { mScaleHeight = value; }
		}

		/// <summary>
		/// Gets or sets the multiplicative color for this surface.
		/// Setting this is equivalent to setting the ColorGradient property
		/// with a gradient with the same color in all corners.  If a gradient
		/// is being used, getting this property returns the top-left color in the gradient.
		/// </summary>
		public Color Color
		{
			get { return mGradient.TopLeft; }
			set { mGradient = new Gradient(value); }
		}
		/// <summary>
		/// Gets or sets the gradient for this surface.
		/// </summary>
		public Gradient ColorGradient
		{
			get { return mGradient; }
			set { mGradient = value; }
		}

		/// <summary>
		/// The cache is used by the display implementation to store data to improve performance
		/// when drawing with this state object.  Do not set this value unless you are writing 
		/// a driver or otherwise know what you are doing.
		/// </summary>
		public SurfaceStateCache Cache { get; set; }

		/// <summary>
		/// Increments the rotation angle of this surface.  Value supplied is in degrees.
		/// </summary>
		/// <param name="degrees"></param>
		public void IncrementRotationAngleDegrees(double degrees)
		{
			mRotation += degrees * Math.PI / 180.0;
		}

		/// <summary>
		/// Gets the size the surface would be drawn on screen, given the source size.
		/// </summary>
		/// <param name="surfaceSize"></param>
		/// <returns></returns>
		public SizeF GetDisplaySize(Size surfaceSize)
		{
			return new SizeF(
				(float)ScaleWidth * surfaceSize.Width,
				(float)ScaleHeight * surfaceSize.Height);
		}
		/// <summary>
		/// Gets the point where the surface would be rotated around,
		/// given the source size.
		/// </summary>
		/// <param name="displaySize"></param>
		/// <returns></returns>
		public PointF GetRotationCenter(SizeF displaySize)
		{
			if (RotationCenter == OriginAlignment.Specified)
				return RotationCenterLocation;
			else
				return Origin.CalcF(RotationCenter, displaySize);
		}

	}

	/// <summary>
	/// Class which represents a specific drawing instance of a surface.
	/// </summary>
	public struct SurfaceDrawInstance
	{
		/// <summary>
		/// Constructs a SurfaceDrawInstance object.
		/// </summary>
		/// <param name="location"></param>
		public SurfaceDrawInstance(Vector2f location) : this()
		{
			DestLocation = location;
		}

		/// <summary>
		/// Constructs a SurfaceDrawInstance object.
		/// </summary>
		/// <param name="location"></param>
		public SurfaceDrawInstance(Point location) : this()
		{
			DestLocation = (PointF)location;
		}

		/// <summary>
		/// Constructs a SurfaceDrawInstance object.
		/// </summary>
		/// <param name="location"></param>
		/// <param name="sourceRect"></param>
		public SurfaceDrawInstance(PointF location, Rectangle sourceRect) : this()
		{
			DestLocation = location;
			SourceRect = sourceRect;
		}

		/// <summary>
		/// The destination location for drawing.
		/// </summary>
		public Vector2f DestLocation { get; set; }
		/// <summary>
		/// If SourceRect is empty (all values are zero), then it is ignored.
		/// </summary>
		public Rectangle SourceRect { get; set; }

		/// <summary>
		/// Gets the actual source rectangle that should be used when drawing from the surface.
		/// </summary>
		/// <param name="surfaceSize"></param>
		/// <returns></returns>
		public Rectangle GetSourceRect(Size surfaceSize)
		{
			if (SourceRect == Rectangle.Empty)
				return new Rectangle(Point.Zero, surfaceSize);
			else
				return SourceRect;
		}
	}

	/// <summary>
	/// Class which contains a list of SurfaceDrawInstance objects.
	/// </summary>
	public class DrawInstanceList : List<SurfaceDrawInstance>
	{
		/// <summary>
		/// Sets the number of SurfaceDrawInstances that should be in the list.
		/// </summary>
		/// <param name="newCount"></param>
		public void SetCount(int newCount)
		{
			if (Count == newCount)
				return;

			while (Count < newCount)
				Add(new SurfaceDrawInstance());

			if (Count > newCount)
				RemoveRange(newCount, Count - newCount);
		}
	}
}
