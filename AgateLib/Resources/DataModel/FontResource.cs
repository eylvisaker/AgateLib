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
