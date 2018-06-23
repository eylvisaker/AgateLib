using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using ManualTests.AgateLib.UserInterface.FF6;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ManualTests.AgateLib.Fakes
{
    public class FakePointerIndicator : PointerIndicator
    {
        List<Rectangle> draws = new List<Rectangle>();

        public FakePointerIndicator(Texture2D texture) : base(texture) { }

        public List<Rectangle> Draws => Draws;

        protected override void DrawPointer(IWidgetRenderContext renderContext, Rectangle pointerDest)
        {
            draws.Add(pointerDest);
        }
    }
}
