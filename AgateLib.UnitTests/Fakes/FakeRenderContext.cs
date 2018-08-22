using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AgateLib.Tests.Fakes
{
    public class FakeRenderContext : IUserInterfaceRenderContext
    {
        public FakeRenderContext()
        {
            Fonts = CommonMocks.FontProvider().Object;
        }

        public GraphicsDevice GraphicsDevice => null;

        public SpriteBatch SpriteBatch => null;

        public RenderTarget2D RenderTarget => null;

        public IFontProvider Fonts { get; set; }

        public GameTime GameTime { get; set; }

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        public IInstructions Instructions { get; set; }

        public Rectangle Area { get; set; }

        public Size GraphicsDeviceRenderTargetSize { get; set; }

        public IContentLayout CreateContentLayout(string text, ContentLayoutOptions contentLayoutOptions, bool localizeText = true)
        {

            return new ContentLayout(new[] {
                new ContentText(text, Fonts.Default, Vector2.Zero)
            });
        }

        public void DrawChild(Rectangle contentDest, IRenderElement child)
        {
        }

        public void DrawChildren(Rectangle contentDest, IEnumerable<IRenderElement> children)
        {
        }

        public void PrepDraw(GameTime time, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
        }

        public void InitializeUpdate(GameTime time)
        {
        }

        public void UpdateAnimation(IRenderElement element)
        {
            if (element.Display.Animation.State == AnimationState.TransitionIn)
                element.Display.Animation.State = AnimationState.Static;

            if (element.Display.Animation.State == AnimationState.TransitionOut)
                element.Display.Animation.State = AnimationState.Dead;
        }
    }
}
