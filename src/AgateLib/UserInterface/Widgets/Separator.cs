using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// A render element that participates in layout but displays nothing. This is useful
    /// for creating some negative space between UI elements.
    /// </summary>
    public class Separator : RenderElement<SeparatorProps>
    {
        public Separator(SeparatorProps props = null) : base(props ?? new SeparatorProps())
        {
        }

        public override string StyleTypeId => "separator";

        public override Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize)
        {
            return new Size(Props.MinSize, Props.MinSize);
        }

        public override void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size)
        { }

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        { }
    }

    public class SeparatorProps : RenderElementProps
    {
        public int MinSize = 0;
    }
}
