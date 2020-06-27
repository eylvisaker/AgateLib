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

using System.Collections;
using System.Collections.Generic;

namespace AgateLib.Display.BitmapFont.Model
{
    public class FontVariations : IList<FontVariationData>
    {
        private List<FontVariationData> fontSurfaces = new List<FontVariationData>();

        public FontVariationData this[int index]
        {
            get => ((IList<FontVariationData>)fontSurfaces)[index];
            set => ((IList<FontVariationData>)fontSurfaces)[index] = value;
        }

        public int Count => ((IList<FontVariationData>)fontSurfaces).Count;

        public bool IsReadOnly => false;

        public void Add(FontVariationData item)
        {
            ((IList<FontVariationData>)fontSurfaces).Add(item);
        }

        public void Clear()
        {
            ((IList<FontVariationData>)fontSurfaces).Clear();
        }

        public bool Contains(FontVariationData item)
        {
            return ((IList<FontVariationData>)fontSurfaces).Contains(item);
        }

        public void CopyTo(FontVariationData[] array, int arrayIndex)
        {
            ((IList<FontVariationData>)fontSurfaces).CopyTo(array, arrayIndex);
        }

        public IEnumerator<FontVariationData> GetEnumerator()
        {
            return ((IList<FontVariationData>)fontSurfaces).GetEnumerator();
        }

        public int IndexOf(FontVariationData item)
        {
            return ((IList<FontVariationData>)fontSurfaces).IndexOf(item);
        }

        public void Insert(int index, FontVariationData item)
        {
            ((IList<FontVariationData>)fontSurfaces).Insert(index, item);
        }

        public bool Remove(FontVariationData item)
        {
            return ((IList<FontVariationData>)fontSurfaces).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<FontVariationData>)fontSurfaces).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<FontVariationData>)fontSurfaces).GetEnumerator();
        }

        internal void ApplyPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            foreach (var fontSurface in this)
            {
                fontSurface.Image = $"{path}/{fontSurface.Image}";
            }
        }

    }
}
