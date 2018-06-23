//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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

using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	/// <summary>
	/// Structure which contains information about collision between two polygons.
	/// </summary>
	public class ContactPoint
	{
		/// <summary>
		/// The first polygon.
		/// </summary>
		public Polygon FirstPolygon { get; set; }
		/// <summary>
		/// The second polygon.
		/// </summary>
		public Polygon SecondPolygon { get; set; }

		/// <summary>
		/// The point on the perimeter of the first polygon that contacts the second polygon.
		/// </summary>
		public Microsoft.Xna.Framework.Vector2 FirstPolygonContactPoint { get; set; }
		/// <summary>
		/// The point on the perimeter of the second polygon that contacts the first polygon.
		/// </summary>
		public Microsoft.Xna.Framework.Vector2 SecondPolygonContactPoint { get; set; }

		/// <summary>
		/// The penetration vector.
		/// </summary>
		public Microsoft.Xna.Framework.Vector2 PenetrationDepth { get; set; }

		/// <summary>
		/// True if the two polygons touch, false if they do not.
		/// </summary>
		public bool Contact { get; set; }

		/// <summary>
		/// The normal of the contact edge for the first polygon.
		/// </summary>
		public Microsoft.Xna.Framework.Vector2 FirstPolygonNormal { get; set; }

		/// <summary>
		/// The normal of the contact edge for the second polygon.
		/// </summary>
		public Microsoft.Xna.Framework.Vector2 SecondPolygonNormal { get; set; }

		/// <summary>
		/// If the two polygons do not touch, this is the closest distance between them.
		/// </summary>
		public float DistanceToContact { get; set; }
	}
}