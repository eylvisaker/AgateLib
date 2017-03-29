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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Resources.DataModel
{
	public class FontResource : IList<FontSurfaceResource>
	{
		List<FontSurfaceResource> fontSurfaces = new List<FontSurfaceResource>();

		public FontSurfaceResource this[int index]
		{
			get { return ((IList<FontSurfaceResource>)fontSurfaces)[index]; }
			set { ((IList<FontSurfaceResource>)fontSurfaces)[index] = value; }
		}

		public int Count => ((IList<FontSurfaceResource>)fontSurfaces).Count;

		public bool IsReadOnly => false;

		public void Add(FontSurfaceResource item)
		{
			((IList<FontSurfaceResource>)fontSurfaces).Add(item);
		}

		public void Clear()
		{
			((IList<FontSurfaceResource>)fontSurfaces).Clear();
		}

		public bool Contains(FontSurfaceResource item)
		{
			return ((IList<FontSurfaceResource>)fontSurfaces).Contains(item);
		}

		public void CopyTo(FontSurfaceResource[] array, int arrayIndex)
		{
			((IList<FontSurfaceResource>)fontSurfaces).CopyTo(array, arrayIndex);
		}

		public IEnumerator<FontSurfaceResource> GetEnumerator()
		{
			return ((IList<FontSurfaceResource>)fontSurfaces).GetEnumerator();
		}

		public int IndexOf(FontSurfaceResource item)
		{
			return ((IList<FontSurfaceResource>)fontSurfaces).IndexOf(item);
		}

		public void Insert(int index, FontSurfaceResource item)
		{
			((IList<FontSurfaceResource>)fontSurfaces).Insert(index, item);
		}

		public bool Remove(FontSurfaceResource item)
		{
			return ((IList<FontSurfaceResource>)fontSurfaces).Remove(item);
		}

		public void RemoveAt(int index)
		{
			((IList<FontSurfaceResource>)fontSurfaces).RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<FontSurfaceResource>)fontSurfaces).GetEnumerator();
		}

		internal void ApplyPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return;

			foreach(var fontSurface in this)
			{
				fontSurface.Image = $"{path}/{fontSurface.Image}";
			}
		}

	}
}
