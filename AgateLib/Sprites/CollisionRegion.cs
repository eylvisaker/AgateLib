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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Sprites
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
			mPoly = Polygon.FromRect(rect);

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

		public Rectangle BoundingRect
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
			return Polygon.PolysIntersect(regionA.mTransformedPoly, regionB.mTransformedPoly);
		}

		public bool IsAlignedRect
		{
			get { return mPoly.IsAlignedRect; }
		}
	}
}
