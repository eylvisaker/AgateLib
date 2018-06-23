using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Fakes
{
    public class FakeRenderContext : IWidgetRenderContext
    {
        public FakeRenderContext()
        {
            Font = new Font(new FakeFontCore("FontX"));
        }

        public GraphicsDevice GraphicsDevice => null;

        public SpriteBatch SpriteBatch => null;

        public RenderTarget2D RenderTarget => null;

        public GameTime GameTime { get; set; }

        public IMenuIndicatorRenderer Indicator => null;

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        public IInstructions Instructions { get; set; }

        public Rectangle Area { get; set; }

        public Size GraphicsDeviceRenderTargetSize { get; set; }

        public Font Font { get; set; }

        public event Action<IWidget> BeforeUpdate;

        public void ApplyStyles(IEnumerable<IWidget> items, string defaultTheme)
        {
        }

        public IContentLayout CreateContentLayout(string text, ContentLayoutOptions contentLayoutOptions, bool localizeText = true)
        {

            return new ContentLayout(new[] {
                new ContentText(text, Font, Vector2.Zero)
            });
        }

        public void DrawChild(Point contentDest, IWidget child)
        {
        }

        public void DrawChildren(Point contentDest, IEnumerable<IWidget> children)
        {
        }

        public void DrawFocus(Rectangle destRect)
        {
        }

        public void EndWorkspace()
        {
        }

        public void BeginDraw(GameTime time, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
        }

        public void InitializeUpdate(GameTime time)
        {
        }

        public void Update(IWidget widget)
        {
            UserInterfaceRenderer?.UpdateAnimation(this, widget);

            BeforeUpdate?.Invoke(widget);

            widget.Update(this);
        }

        public void Update(IEnumerable<IWidget> items)
        {
            foreach (var item in items)
                Update(item);
        }

        public void DrawWorkspace(Workspace workspace, IEnumerable<IWidget> items)
        {
        }
    }
}
