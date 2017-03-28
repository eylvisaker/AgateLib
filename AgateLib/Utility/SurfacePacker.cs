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
using System.Text;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Utility
{
	using DisplayLib;

	/// <summary>
	/// Class which handles packing of surfaces into a large surface.
	/// 
	/// This class was introduced to allow render state changes to be minimized
	/// by making everything use the same texture object in memory.
	/// </summary>
	public class SurfacePacker
	{
		/// <summary>
		/// Holds a destination rectangle and object to go in it.
		/// </summary>
		/// <typeparam name="T">Type of object to put in rectangle.</typeparam>
		public class RectHolder<T> : IComparable<RectHolder<T>>
		{
			private Rectangle mRect;
			private T mTag;

			internal RectHolder(Rectangle rect, T tag)
			{
				this.Rect = rect;
				this.Tag = tag;
			}

			/// <summary>
			/// Gets or sets the destination rectangle.
			/// </summary>
			public Rectangle Rect
			{
				get { return mRect; }
				set { mRect = value; }
			}
			/// <summary>
			/// Gets or sets the object.
			/// </summary>
			public T Tag
			{
				get { return mTag; }
				set { mTag = value; }
			}
			#region IComparable<RectHolder> Members

			/// <summary>
			/// Compares two RectHolder objects for sorting purposes.
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			public int CompareTo(RectHolder<T> other)
			{
				int a1 = Math.Max(Rect.Width, Rect.Height);
				int a2 = Math.Min(Rect.Width, Rect.Height);
				int b1 = Math.Max(other.Rect.Width, other.Rect.Height);
				int b2 = Math.Min(other.Rect.Width, other.Rect.Height);

				// return values are reversed, because we want largest first.

				if (a1 < b1)
					return 1;
				else if (a1 > b1)
					return -1;

				if (a2 < b2)
					return 1;
				else if (a2 > b2)
					return -1;

				// ok, max and min dimensions are the same.
				// make tall rectangles go before short ones.
				if (Rect.Height < other.Rect.Height)
					return 1;
				else if (Rect.Height > other.Rect.Height)
					return -1;
				else
					return 0;

			}

			#endregion
		}
		/// <summary>
		/// Class which takes a bunch of rectangles and organizes them to 
		/// all fit within a large rectangle.
		/// </summary>
		/// <typeparam name="T">The type of objects which will be added to the rectangles.
		/// This is so that the RectPacker can keep track of which rectangles go with
		/// which objects.</typeparam>
		public class RectPacker<T> : IEnumerable<RectHolder<T>>
		{

			List<RectHolder<T>> mRects = new List<RectHolder<T>>();
			List<RectHolder<T>> mQueue = new List<RectHolder<T>>();
			double mPixelsUsedPercentage = 0.0;
			Size mContainerSize;

			/// <summary>
			/// Creates a RectPacker object of the specified size.
			/// </summary>
			/// <param name="containerSize"></param>
			public RectPacker(Size containerSize)
			{
				mContainerSize = containerSize;
			}
			/// <summary>
			/// Adds a used rectangle to the RectPacker.
			/// This throws an exception if pixels in the rectangle specified 
			/// are already used.
			/// </summary>
			/// <param name="rect"></param>
			/// <param name="tag"></param>
			public void AddRect(Rectangle rect, T tag)
			{
				if (IsRectFree(rect))
				{
					mRects.Add(new RectHolder<T>(rect, tag));

					mPixelsUsedPercentage += rect.Width * rect.Height /
						(double)(mContainerSize.Width * mContainerSize.Height);
				}
				else
					throw new AgateException("Attempted to add a rectangle which was not free.");
			}
			private void AddRect(RectHolder<T> rectHolder)
			{
				if (IsRectFree(rectHolder.Rect))
				{
					mRects.Add(rectHolder);

					mPixelsUsedPercentage += rectHolder.Rect.Width * rectHolder.Rect.Height /
						(double)(mContainerSize.Width * mContainerSize.Height);
				}
				else
					throw new AgateException("Attempted to add a rectangle which was not free.");
			}
			/// <summary>
			/// Finds an empty space of the specified size.
			/// 
			/// Returns true if there was an empty space of that size available.
			/// </summary>
			/// <param name="size"></param>
			/// <param name="rect"></param>
			/// <returns></returns>
			public bool FindEmptySpace(Size size, out Rectangle rect)
			{
				Point pt = Point.Zero;
				int nextScanLine = mContainerSize.Height;

				while (pt.Y < mContainerSize.Height)
				{
					Rectangle containing;

					if (FindContainingRect(pt, out containing))
					{
						pt.X = containing.Right + 1;

						if (containing.Bottom + 1 < nextScanLine)
							nextScanLine = containing.Bottom + 1;
					}
					else
					{
						if (IsRectFree(pt, size))
						{
							rect = new Rectangle(pt, size);
							return true;
						}
						else
							pt.X += size.Width;
					}

					// go to next line when we reach the end
					if (pt.X + size.Width >= mContainerSize.Width)
					{
						if (nextScanLine == pt.Y)
							nextScanLine++;

						pt.X = 0;
						pt.Y = nextScanLine;
						nextScanLine = mContainerSize.Height;

					}

				}

				rect = Rectangle.Empty;
				return false;
			}

			private bool IsRectFree(Point pt, Size size)
			{
				return IsRectFree(new Rectangle(pt, size));
			}
			private bool IsRectFree(Rectangle myrect)
			{
				if (myrect.Right >= mContainerSize.Width ||
					myrect.Bottom >= mContainerSize.Height)
					return false;

				foreach (RectHolder<T> rect in mRects)
				{
					if (rect.Rect.IntersectsWith(myrect))
						return false;
				}

				return true;
			}

			private bool FindContainingRect(Point pt, out Rectangle containing)
			{
				for (int i = 0; i < mRects.Count; i++)
				{
					if (mRects[i].Rect.Contains(pt))
					{
						containing = mRects[i].Rect;
						return true;
					}
				}

				containing = Rectangle.Empty;
				return false;
			}
			/// <summary>
			/// Gets or sets the size of the container to fit all the
			/// rects into.
			/// </summary>
			public Size ContainerSize
			{
				get { return mContainerSize; }
				set { mContainerSize = value; }
			}
			/// <summary>
			/// Returns what percentage of the container is used up.
			/// </summary>
			public double PixelsUsedPercentage
			{
				get { return mPixelsUsedPercentage; }
			}
			/// <summary>
			/// Adds an object of the specified size to the queue.
			/// </summary>
			/// <param name="size"></param>
			/// <param name="tag"></param>
			public void QueueObject(Size size, T tag)
			{
				mQueue.Add(new RectHolder<T>(new Rectangle(Point.Zero, size), tag));
			}
			/// <summary>
			/// Clears the queue.
			/// </summary>
			public void ClearQueue()
			{
				mQueue.Clear();
			}
			/// <summary>
			/// Packs all the objects in the queue to the container.
			/// Sorts them first, to optimize coverage.
			/// </summary>
			public void AddQueue()
			{
				mQueue.Sort();

				for (int i = 0; i < mQueue.Count; i++)
				{
					Rectangle rect;

					if (FindEmptySpace(mQueue[i].Rect.Size, out rect))
					{
						mQueue[i].Rect = rect;
						AddRect(mQueue[i]);
					}

				}
			}
			/// <summary>
			/// Returns a collection of all objects in the queue which have
			/// not been added to the container rect.
			/// </summary>
			/// <returns></returns>
			public ICollection<T> UnusuedQueueObjects()
			{
				List<T> objs = new List<T>();

				foreach (RectHolder<T> h in mQueue)
					objs.Add(h.Tag);

				return objs;
			}

			#region --- IEnumerable<RectHolder> Members ---

			/// <summary>
			/// Enumerates through the packed rectangles.
			/// </summary>
			/// <returns></returns>
			public IEnumerator<RectHolder<T>> GetEnumerator()
			{
				return mRects.GetEnumerator();
			}

			#endregion
			#region --- IEnumerable Members ---

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			#endregion
		}

		/// <summary>
		/// Class which creates the final packed surface.
		/// </summary>
		public class PackedSurface
		{
			RectPacker<Surface> mRects;
			Surface mPackingSurface;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="size"></param>
			public PackedSurface(Size size)
			{
				mRects = new RectPacker<Surface>(size);
			}

			/// <summary>
			/// 
			/// </summary>
			public RectPacker<Surface> RectPacker
			{
				get { return mRects; }
				set { mRects = value; }
			}


			internal void Build()
			{
				mPackingSurface = Display.BuildPackedSurface(mRects.ContainerSize, RectPacker);


				//mPackingSurface.SaveTo("testpackedsurface.png", ImageFileFormat.Png);

			}
		}


		List<Surface> mSurfQueue = new List<Surface>();
		List<PackedSurface> mPackedSurfaces = new List<PackedSurface>();

		internal SurfacePacker()
		{

		}

		/// <summary>
		/// Clears the list of surfaces that should be packed.
		/// </summary>
		public void ClearQueue()
		{
			mSurfQueue.Clear();
		}
		/// <summary>
		/// Adds a surface to the list of surfaces that should be packed.
		/// </summary>
		/// <param name="surf"></param>
		public void QueueSurface(Surface surf)
		{
			mSurfQueue.Add(surf);
		}
		/// <summary>
		/// Packs all the surfaces in the list.
		/// </summary>
		public void PackQueue()
		{
			Size size = Display.Caps.MaxSurfaceSize;

			// Cap the size to avoid running out of memory. 
			// An ATI Radeon 7850 will apparently report a max surface size of 16384x16384, which would take
			// a GB of memory for the surface. We shouldn't preallocate that much space.
			if (size.Width > 2048) size.Width = 2048;
			if (size.Height > 2048) size.Height = 2048;

			PackedSurface packedSurf = new PackedSurface(size);

			foreach (Surface surf in mSurfQueue)
			{
				packedSurf.RectPacker.QueueObject(surf.SurfaceSize, surf);
			}

			mSurfQueue.Clear();

			// fit all those little surfaces in.
			packedSurf.RectPacker.AddQueue();

			packedSurf.Build();
		}


	}

}