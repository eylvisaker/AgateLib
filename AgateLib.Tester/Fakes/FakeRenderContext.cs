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

        public IFontProvider Fonts => null;

        public GameTime GameTime { get; set; }

        public IMenuIndicatorRenderer Indicator => null;

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        public IInstructions Instructions { get; set; }

        public Rectangle Area { get; set; }

        public Size GraphicsDeviceRenderTargetSize { get; set; }

        [Obsolete("The fake font provider should be implemented.")]
        public Font Font { get; set; }


        public event Action<IRenderElement> BeforeUpdate;

        public void ApplyStyles(IEnumerable<IRenderWidget> items, string defaultTheme)
        {
        }

        public IContentLayout CreateContentLayout(string text, ContentLayoutOptions contentLayoutOptions, bool localizeText = true)
        {

            return new ContentLayout(new[] {
                new ContentText(text, Font, Vector2.Zero)
            });
        }

        public void DrawChild(Rectangle contentDest, IRenderElement child)
        {
        }

        public void DrawChildren(Rectangle contentDest, IEnumerable<IRenderElement> children)
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

        public void Update(IRenderWidget widget)
        {
            throw new NotImplementedException();

            //UserInterfaceRenderer?.UpdateAnimation(this, widget);

            //BeforeUpdate?.Invoke(widget);

            //widget.Update(this);
        }

        public void Update(IEnumerable<IRenderWidget> items)
        {
            foreach (var item in items)
                Update(item);
        }

        public void DrawWorkspace(Workspace workspace, IEnumerable<IRenderWidget> items)
        {
        }

        public void DrawWorkspace(Workspace workspace, VisualTree visualTree)
        {
        }

        public void UpdateAnimation(IRenderElement x)
        {
            throw new NotImplementedException();
        }
    }
}
