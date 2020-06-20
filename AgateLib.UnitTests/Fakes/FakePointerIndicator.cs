using AgateLib.Display;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AgateLib.Tests.Fakes
{
    public class FakeFocusIndicator : IFocusIndicator
    {
        private List<Rectangle> draws = new List<Rectangle>();

        public List<Rectangle> Draws => Draws;

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        public void DrawFocus(ICanvas canvas, IRenderElement focusElement, Workspace activeWorkspace, Rectangle focusContentArea)
        {
            draws.Add(focusContentArea);
        }
    }
}
