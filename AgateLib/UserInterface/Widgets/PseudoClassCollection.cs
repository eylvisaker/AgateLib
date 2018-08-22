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
using System.Collections.Generic;

namespace AgateLib.UserInterface
{
    public class PseudoClassCollection
    {
        private HashSet<string> values = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Adds or removes a pseudoclass if the flag passed is true.
        /// </summary>
        /// <param name="pseudoclass">The pseudoclass that should be present if value is true</param>
        /// <param name="value">Pass true to include the pseudoclass, false to disable it.</param>
        public void SetIf(string pseudoclass, bool value)
        {
            if (value)
                values.Add(pseudoclass.ToLowerInvariant());
            else
                values.Remove(pseudoclass);
        }

        public void Remove(string pseudoclass)
        {
            values.Remove(pseudoclass);
        }

        public void Add(string pseudoclass)
        {
            values.Add(pseudoclass);
        }

        public bool Contains(string pseudoclass)
        {
            return values.Contains(pseudoclass);
        }
    }
}
