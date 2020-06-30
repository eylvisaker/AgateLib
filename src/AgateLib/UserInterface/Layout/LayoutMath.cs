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

using AgateLib.Mathematics.Geometry;
using System;

namespace AgateLib.UserInterface.Layout
{
    /// <summary>
    /// Static class which provides some basic math utilities for doing widget layouts.
    /// </summary>
    public static class LayoutMath
    {
        /// <summary>
        /// Gets the maximum content size for an item, given the max dimensions of its parent.
        /// </summary>
        /// <param name="itemBox"></param>
        /// <param name="parentMaxWidth"></param>
        /// <param name="parentMaxHeight"></param>
        /// <returns></returns>
        public static Size ItemContentMaxSize(LayoutBox itemBox, Size parentMaxSize)
        {
            int itemMaxWidth = parentMaxSize.Width;
            int itemMaxHeight = parentMaxSize.Height;

            itemMaxWidth -= itemBox.Width;
            itemMaxHeight -= itemBox.Height;

            return new Size(itemMaxWidth, itemMaxHeight);
        }

        /// <summary>
        /// Constrains the content size of a render element.
        /// </summary>
        /// <param name="item">The item who's size is to be constrained.</param>
        /// <param name="size">The desired content size of the item.</param>
        /// <returns></returns>
        public static Size ConstrainSize(IRenderElement item, Size size)
        {
            ISizeConstraints constraints = item.Style.Size;

            if (constraints != null)
            {
                if (size.Width > constraints.MaxWidth)
                {
                    size.Width = constraints.MaxWidth.Value;
                }

                if (size.Height > constraints.MaxHeight)
                {
                    size.Height = constraints.MaxHeight.Value;
                }

                if (size.Width < constraints.MinWidth)
                {
                    size.Width = constraints.MinWidth;
                }

                if (size.Height < constraints.MinHeight)
                {
                    size.Height = constraints.MinHeight;
                }
            }

            var minSize = item.CalcMinContentSize(size.Width, size.Height);

            size.Width = Math.Max(size.Width, minSize.Width);
            size.Height = Math.Max(size.Height, minSize.Height);

            return size;
        }

        /// <summary>
        /// Reduces the value of maxSize based on the constraints provided. This
        /// will use width/height and maxwidth/maxheight from the constraints object
        /// and return 
        /// </summary>
        /// <param name="maxSize"></param>
        /// <param name="constraints"></param>
        /// <returns>The new maximum size of the widget.</returns>
        public static Size ConstrainMaxSize(Size maxSize, ISizeConstraints constraints)
        {
            if (constraints == null)
            {
                return maxSize;
            }

            if (maxSize.Width > constraints.MaxWidth)
            {
                maxSize.Width = constraints.MaxWidth.Value;
            }

            if (maxSize.Height > constraints.MaxHeight)
            {
                maxSize.Height = constraints.MaxHeight.Value;
            }

            return maxSize;
        }

        public static void CalcIdealSize(IRenderElement item, IUserInterfaceLayoutContext layoutContext, Size parentMaxSize)
        {
            Size maxSize = ConstrainMaxSize(parentMaxSize, item.Style.Size);

            Size idealSize = item.CalcIdealContentSize(layoutContext, maxSize);

            idealSize = ConstrainSize(item, idealSize);

            item.Display.Region.IdealContentSize = idealSize;
        }

        public static int? Scale(int? value, float amount)
        {
            if (value == null)
            {
                return null;
            }

            return Scale(value.Value, amount);
        }

        /// <summary>
        /// Scales an integer by a floating point amount.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static int Scale(int value, float amount)
            => (int)(value * amount);
    }
}