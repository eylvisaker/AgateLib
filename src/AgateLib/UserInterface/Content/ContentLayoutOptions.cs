﻿//
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

using AgateLib.Display;

namespace AgateLib.UserInterface.Content
{
    public class ContentLayoutOptions
    {
        /// <summary>
        /// Font to use for text.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Default font to use for text.
        /// </summary>
        public Font DefaultFont { get; set; }

        /// <summary>
        /// If Font is null, then the content layout engine will look up a font described by 
        /// the FontLookup property, if it is not null.
        /// </summary>
        public FontStyleProperties FontLookup { get; set; }

        /// <summary>
        /// Defaults to true. Set this to false to prevent images from being scaled
        /// to match the height of the text.
        /// </summary>
        public bool ScaleImages { get; set; } = true;
    }
}
