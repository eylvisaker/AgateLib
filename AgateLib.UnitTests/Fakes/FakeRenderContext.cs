using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Content.LayoutItems;
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

        public Rectangle ScreenArea { get; set; }

        public Size GraphicsDeviceRenderTargetSize { get; set; }

        public IContentLayout CreateContentLayout(string text, ContentLayoutOptions contentLayoutOptions, bool localizeText = true)
        {
            return new ContentLayout(new[] {
                new ContentText(text, Fonts.Default),
            }, 10);
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

        public void Draw(Texture2D image, Rectangle destRect, Color color) { }
        public void Draw(Texture2D image, Rectangle destRect, Rectangle sourceRect, Color color)
        { }

        public void DrawText(Font font, Vector2 destination, string text)
        { }

        public void Draw(IContentLayout content, Vector2 destination)
        { }

        public void Draw(IContentLayout content, Rectangle destinationArea)
        { }
    }
}
