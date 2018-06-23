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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.UserInterface
{
    public class SizeMetrics
    {
        /// <summary>
        /// Input to the ComputeSizeMetrics method.
        /// The maximum size of the parent's content area. The widget must be smaller than this or
        /// it can't be entirely displayed within its parent, even with scrolling.
        /// This is only set by the parent before calling ComputeSizeMetrics.
        /// </summary>
        [Obsolete("This value is now passed into the ComputeIdealSize method.")]
        public Size ParentMaxSize
        {
            get => new Size(ParentMaxWidth, ParentMaxHeight);
            set
            {
                ParentMaxWidth = value.Width;
                ParentMaxHeight = value.Height;
            }
        }

        /// <summary>
        /// Input to the ComputeSizeMetrics method.
        /// The maximum width of the parent's content area. The widget must be smaller than this or
        /// it can't be entirely displayed within its parent, even with scrolling.
        /// This is only set by the parent before calling ComputeSizeMetrics.
        /// </summary>
        public int ParentMaxWidth { get; set; }

        /// <summary>
        /// Input to the ComputeSizeMetrics method.
        /// The maximum height of the parent's content area. The widget must be smaller than this or
        /// it can't be entirely displayed within its parent, even with scrolling.
        /// This is only set by the parent before calling ComputeSizeMetrics.
        /// </summary>
        public int ParentMaxHeight { get; set; }

        /// <summary>
        /// Output of the the ComputeSizeMetrics method.
        /// Gets the ideal content size given the constraints.
        /// </summary>
        public Size IdealContentSize { get; set; }

        /// <summary>
        /// The minimum size of the control. It will not be sized smaller than this, even 
        /// if it is too large for its parent's content area.
        /// </summary>
        public Size MinimumSize { get; set; }

        /// <summary>
        /// The maximum size of the control. It will not be sized larger than this.
        /// </summary>
        public Size MaximumSize { get; set; } = new Size(int.MaxValue, int.MaxValue);

        /// <summary>
        /// Computes the actual content size of the widget, given the constraints and its ideal 
        /// content size.
        /// </summary>
        /// <returns></returns>
        public Size ComputeContentSize()
        {
            var result = IdealContentSize;

            result.Width = Math.Max(result.Width, MinimumSize.Width);
            result.Height = Math.Max(result.Height, MinimumSize.Height);

            result.Width = Math.Min(result.Width, MaximumSize.Width);
            result.Height = Math.Min(result.Height, MaximumSize.Height);

            return result;
        }

    }
}
