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
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection;
using Microsoft.Xna.Framework;

namespace AgateLib.Display.Sprites
{
	public class CollisionRegion
	{
		Polygon mPoly;
		Polygon mTransformedPoly;

		public CollisionRegion(Polygon poly)
		{
			mPoly = poly;

			mTransformedPoly = mPoly.Clone();
		}

		public CollisionRegion(Rectangle rect)
		{
			mPoly = rect.ToPolygon();

			mTransformedPoly = mPoly.Clone();
		}

		public Polygon Polygon
		{
			get { return mPoly; }
		}

		public void SetTransform(Vector2 translation, bool fliphorizontal, bool flipvertical)
		{
			for (int i = 0; i < mPoly.Points.Count; i++)
			{
				var pt = mPoly.Points[i];

				if (fliphorizontal) pt.X *= -1;
				if (flipvertical) pt.Y *= -1;

				pt.X += translation.X;
				pt.Y += translation.Y;

				mTransformedPoly.Points[i] = pt;
			}
		}

		public bool FlipHorizontal { get; set; }
		public bool FlipVertical { get; set; }

		public RectangleF BoundingRect
		{
			get
			{
				var result = mTransformedPoly.BoundingRect;

				return result;
			}
		}

		/// <summary>
		/// Call this after calling SetTransformation.
		/// </summary>
		/// <param name="regionA"></param>
		/// <param name="regionB"></param>
		/// <returns></returns>
		public static bool DoRegionsIntersect(CollisionRegion regionA, CollisionRegion regionB)
		{
			return new CollisionDetector().DoPolygonsIntersect(regionA.mTransformedPoly, regionB.mTransformedPoly);
		}
	}
}
