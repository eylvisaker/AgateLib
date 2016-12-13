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
			get
			{
				return ((IList<FontSurfaceResource>)fontSurfaces)[index];
			}

			set
			{
				((IList<FontSurfaceResource>)fontSurfaces)[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IList<FontSurfaceResource>)fontSurfaces).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IList<FontSurfaceResource>)fontSurfaces).IsReadOnly;
			}
		}

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
	}
}
