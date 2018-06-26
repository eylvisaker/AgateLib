using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class FlexContainer : RenderElement<FlexContainerProps>
    {
        public FlexContainer(FlexContainerProps props) : base(props)
        {

        }

        public override Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            throw new NotImplementedException();
        }

        public override void Draw(IWidgetRenderContext renderContext, Point offset)
        {
            throw new NotImplementedException();
        }
    }

    public class FlexContainerProps : RenderElementProps
    {
        public IEnumerable<IRenderElement> Children { get; set; }
    }
}
