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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Styling.Themes
{
    /// <summary>
    /// Represents a read-only collection of themes. 
    /// Implements the IReadOnlyDictionary&lt;string, ITheme&gt;
    /// interface.
    /// </summary>
    public interface IThemeCollection : IReadOnlyDictionary<string, ITheme> { }

    /// <summary>
    /// Represents a collection of themes. 
    /// Implements the IDictionary&lt;string, ITheme&gt;
    /// interface.
    /// </summary>
    public class ThemeCollection : IThemeCollection, IDictionary<string, ITheme>
    {
        private readonly Dictionary<string, ITheme> themes = new Dictionary<string, ITheme>();

        public ITheme this[string key] { get => ((IDictionary<string, ITheme>)themes)[key]; set => ((IDictionary<string, ITheme>)themes)[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, ITheme>)themes).Keys;

        public ICollection<ITheme> Values => ((IDictionary<string, ITheme>)themes).Values;

        public int Count => ((IDictionary<string, ITheme>)themes).Count;

        public bool IsReadOnly => ((IDictionary<string, ITheme>)themes).IsReadOnly;

        IEnumerable<string> IReadOnlyDictionary<string, ITheme>.Keys => Keys;

        IEnumerable<ITheme> IReadOnlyDictionary<string, ITheme>.Values => Values;

        public void Add(string key, ITheme value)
        {
            ((IDictionary<string, ITheme>)themes).Add(key, value);
        }

        public void Add(KeyValuePair<string, ITheme> item)
        {
            ((IDictionary<string, ITheme>)themes).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<string, ITheme>)themes).Clear();
        }

        public bool Contains(KeyValuePair<string, ITheme> item)
        {
            return ((IDictionary<string, ITheme>)themes).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, ITheme>)themes).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, ITheme>[] array, int arrayIndex)
        {
            ((IDictionary<string, ITheme>)themes).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, ITheme>> GetEnumerator()
        {
            return ((IDictionary<string, ITheme>)themes).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, ITheme>)themes).Remove(key);
        }

        public bool Remove(KeyValuePair<string, ITheme> item)
        {
            return ((IDictionary<string, ITheme>)themes).Remove(item);
        }

        public bool TryGetValue(string key, out ITheme value)
        {
            return ((IDictionary<string, ITheme>)themes).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, ITheme>)themes).GetEnumerator();
        }
    }
}
