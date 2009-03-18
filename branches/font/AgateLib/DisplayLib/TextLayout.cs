using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
    public class TextLayout : List<LayoutItem>
    {
        public void DrawAll()
        {
            foreach (LayoutItem item in this)
            {
                item.Draw();
            }
        }
    }

    public abstract class LayoutItem
    {
        public abstract void Draw();
    }

    public class LayoutText : LayoutItem
    {
        public FontSurface Font { get; set; }
        public string Text { get; set; }
        public Point Location { get; set; }

        public override void Draw()
        {
            Font.DrawText(Location, Text);
        }
    }
    public class LayoutSurface : LayoutItem
    {
        public Surface Surface { get; set; }
        public Point Location { get; set; }
        public SurfaceState State { get; set; }

        public override void Draw()
        {
            this.Surface.Draw(Location, State);
        }
    }
}
