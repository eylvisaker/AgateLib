//     ``The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    /// <summary>
    /// SizeF structure.
    /// </summary>
    public struct SizeF
    {
        float width, height;

        /// <summary>
        /// Constructs a SizeF structure.
        /// </summary>
        /// <param name="pt"></param>
        public SizeF(PointF pt)
        {
            width = pt.X;
            height = pt.Y;
        }
        /// <summary>
        /// Constructs a SizeF structure.
        /// </summary>
        /// <param name="size"></param>
        public SizeF(System.Drawing.SizeF size)
        {
            this.width = size.Width;
            this.height = size.Height;
        }
        /// <summary>
        /// Constructs a SizeF structure.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public SizeF(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// True if width and height are zero.
        /// </summary>
        public bool IsEmpty
        {
            get { return width == 0 && height == 0; }
        }
        /// <summary>
        /// Empty SizeF structure.
        /// </summary>
        public static readonly SizeF Empty = new SizeF(0, 0);
    }
}
