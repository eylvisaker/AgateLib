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
using System.Text;

namespace AgateLib.UserInterface.Content
{
    /// <summary>
    /// Class which decorates an IContentLayoutEngine, using a ITextRepository object to perform localization.
    /// </summary>
    public class LocalizedContentLayoutEngine : IContentLayoutEngine
    {
        private readonly IContentLayoutEngine baseEngine;
        private readonly ITextRepository textRepo;

        public LocalizedContentLayoutEngine(IContentLayoutEngine baseEngine, ITextRepository textRepo)
        {
            this.baseEngine = baseEngine;
            this.textRepo = textRepo;
        }

        public void AddCommand(string name, IContentCommand command)
        {
            baseEngine.AddCommand(name, command);
        }

        /// <summary>
        /// Creates an IContentLayout object for the specified text.
        /// </summary>
        /// <param name="text">The text to layout. If localizeText is true, this text will be used
        /// as a key in the lookup table for the current language.</param>
        /// <param name="contentLayoutOptions">Options for content layout.</param>
        /// <param name="localizeText">If true, the text will be localized. Defaults to true.</param>
        /// <returns></returns>
        public IContentLayout LayoutContent(
            string text,
            ContentLayoutOptions contentLayoutOptions,
            bool localizeText = true)
        {
            if (localizeText)
                text = textRepo.Lookup(text);

            return baseEngine.LayoutContent(text, contentLayoutOptions);
        }
    }
}
