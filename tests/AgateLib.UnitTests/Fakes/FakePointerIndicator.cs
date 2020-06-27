using AgateLib.Display;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AgateLib.Demo.Fakes
{
    public class FakePointer : ThemedCursor
    {
        public FakePointer() : base(null) { }

        private List<Rectangle> draws = new List<Rectangle>();

        public List<Rectangle> Draws => Draws;

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }
        public Vector2 Velocity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Vector2 Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool AllowInput { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Draw(ICanvas canvas, IRenderElement focusElement, Workspace activeWorkspace, Rectangle focusContentArea)
        {
            draws.Add(focusContentArea);
        }

        public void Draw(GameTime gameTime, ICanvas canvas)
        {
        }

        public void MoveToFocus(Workspace activeWorkspace, IRenderElement focusElement, Rectangle focusContentRect, Rectangle focusAnimatedContentRect)
        {
            draws.Add(focusContentRect);
        }

        public void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}
