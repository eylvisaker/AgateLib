//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.UserInterface.Rendering
{
    public interface IDoubleBuffer
    {
        IUserInterfaceRenderContext PrepRenderState(IRenderElement widget, IUserInterfaceRenderContext renderContext);

        void CompleteRendering(Rectangle parentContentDest, IUserInterfaceRenderContext renderContext, IRenderElement widget);
    }

    [Transient]
    public class DoubleBuffer : IDoubleBuffer
    {
        private BlendState blendState = new BlendState
        {
            AlphaBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,

            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
        };

        public void CompleteRendering(Rectangle parentContentDest,
            IUserInterfaceRenderContext renderContext, IRenderElement widget)
        {
            var animation = widget.Display.Animation;
            var display = widget.Display;

            display.Animation.Buffer.RenderContext.SpriteBatch.End();

            // Now use the parent render target.
            renderContext.GraphicsDevice.SetRenderTarget(renderContext.RenderTarget);

            var screenRect = animation.AnimatedBorderRect;
            screenRect.X += parentContentDest.X;
            screenRect.Y += parentContentDest.Y;

            renderContext.SpriteBatch.Draw(
                animation.RenderTarget,
                screenRect,
                null,
                animation.Color,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                animation.LayerDepth);
        }

        public IUserInterfaceRenderContext PrepRenderState(IRenderElement widget, IUserInterfaceRenderContext renderContext)
        {
            var animation = widget.Display.Animation;
            var display = widget.Display;

            if (InitializeRenderTarget(display, renderContext.GraphicsDevice))
            {
                if (display.Animation.Buffer.RenderContext == null)
                {
                    display.Animation.Buffer.RenderContext = new UserInterfaceRenderContext(
                        (UserInterfaceRenderContext)renderContext,
                        new SpriteBatch(renderContext.GraphicsDevice),
                        animation.RenderTarget);
                }

                display.Animation.Buffer.RenderContext.RenderTarget = animation.RenderTarget;
            }

            var newRenderContext = display.Animation.Buffer.RenderContext;

            newRenderContext.GameTime = renderContext.GameTime;

            newRenderContext.GraphicsDevice.SetRenderTarget(animation.RenderTarget);
            newRenderContext.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            newRenderContext.GraphicsDevice.BlendState = blendState;

            newRenderContext.SpriteBatch.Begin();

            widget.Display.Animation.Buffer.ContentDestination = new Rectangle(
                display.Region.MarginToContentOffset.Left,
                display.Region.MarginToContentOffset.Top,
                widget.Display.Animation.RenderTarget.Width - display.Region.MarginToContentOffset.Right,
                widget.Display.Animation.RenderTarget.Height - display.Region.MarginToContentOffset.Bottom);

            return newRenderContext;
        }

        /// <summary>
        /// Checks if the render target needs to be initialized and does it. 
        /// Returns true if a new render target was created. False othewise.
        /// </summary>
        /// <param name="display"></param>
        /// <returns></returns>
        public bool InitializeRenderTarget(RenderElementDisplay display, GraphicsDevice graphicsDevice)
        {
            var animation = display.Animation;


            if (animation.RenderTarget == null ||
                animation.RenderTarget.Width != display.MarginRect.Width ||
                animation.RenderTarget.Height != display.MarginRect.Height)
            {
                animation.RenderTarget?.Dispose();

                var size = display.MarginRect;

                if (size.Width < 1) size.Width = 1;
                if (size.Height < 1) size.Height = 1;

                animation.RenderTarget = new RenderTarget2D(
                               graphicsDevice,
                               size.Width,
                               size.Height);

                return true;
            }

            return false;
        }

    }
}
