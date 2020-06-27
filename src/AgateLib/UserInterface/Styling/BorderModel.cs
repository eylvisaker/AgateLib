using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Styling
{
    public class BorderModel
    {
        public LayoutBox SizeLayout { get; set; }

        public BorderImageModel Image { get; set; }
    }

    public class BorderImageModel
    {
        public string Filename { get; set; }

        public BorderImageCorners OuterCorners { get; set; }
        public BorderImageCorners InnerCorners { get; set; }
        public BorderEdges Edges { get; set; }
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
