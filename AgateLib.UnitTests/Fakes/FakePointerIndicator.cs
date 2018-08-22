using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.Tests.UserInterface.FF6;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.Tests.Fakes
{
    public class FakeFocusIndicator : IFocusIndicator
    {
        List<Rectangle> draws = new List<Rectangle>();

        public List<Rectangle> Draws => Draws;

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        public void DrawFocus(SpriteBatch spriteBatch, IRenderElement focusElement, Rectangle focusContentArea)
        {
            draws.Add(focusContentArea);
        }
    }
}
