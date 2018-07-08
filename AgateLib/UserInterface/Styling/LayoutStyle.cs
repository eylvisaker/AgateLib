using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Styling
{
    public class LayoutStyle
    {
        public static bool Equals(LayoutStyle a, LayoutStyle b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a.PositionType != b.PositionType) return false;

            return true;
        }

        /// <summary>
        /// Gets or sets how the item is positioned.
        /// </summary>
        public PositionType PositionType { get; set; }
    }

    public enum PositionType
    {
        /// <summary>
        /// Static - the item is always positioned in the normal flow.
        /// </summary>
        Static,

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        Relative,

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        Fixed,

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        Absolute,

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        Sticky, 

        Default = Static,
    }
}
