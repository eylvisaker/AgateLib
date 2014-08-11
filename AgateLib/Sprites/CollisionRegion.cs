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
				var retval = mTransformedPoly.BoundingRect;

				return retval;
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
