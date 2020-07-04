using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Styling
{
    public class BorderModel
    {
        public LayoutBox SizeLayout { get; set; }

        public LayoutBox Overhang { get; set; }

        public BorderImageModel Image { get; set; }
    }

    public class BorderImageModel
    {
        public string Filename { get; set; }

        /// <summary>
        /// Source rectangles for outer corners.
        /// </summary>
        public BorderImageCorners OuterCorners { get; set; }
        /// <summary>
        /// Source rectangles for inner corners.
        /// </summary>
        public BorderImageCorners InnerCorners { get; set; }

        /// <summary>
        /// Source rectangles for straight edges.
        /// </summary>
        public BorderEdges Edges { get; set; }

        /// <summary>
        /// Source rectangles for ends.
        /// </summary>
        public BorderEdges Ends { get; set; }
    }

    public class BorderImageCorners
    {
        public Rectangle TopLeft { get; set; }
        public Rectangle TopRight { get; set; }
        public Rectangle BottomLeft { get; set; }
        public Rectangle BottomRight { get; set; }
    }

    public class BorderEdges
    {
        public ImageScale ImageScale { get; set; }
        public Rectangle Top { get; set; }
        public Rectangle Left { get; set; }
        public Rectangle Right { get; set; }
        public Rectangle Bottom { get; set; }
    }
}
