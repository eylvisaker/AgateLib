using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib.Sprites;

namespace PackedSpriteCreator
{
    class SpriteRenderer
    {
        public SpriteRenderer()
        {
            BackgroundColor = Color.Black;
        }
        public Color BackgroundColor { get; set; }

        public void DrawSprite(FrameBuffer renderTarget, Sprite sprite)
        {
            if (renderTarget == null)
                return;

            if (sprite != null)
                sprite.Update();

            Display.RenderTarget = renderTarget;
            Display.BeginFrame();
            Display.Clear(BackgroundColor);

            if (sprite != null)
            {
                Point center = new Point(renderTarget.Width / 2, renderTarget.Height / 2);

                sprite.DisplayAlignment = OriginAlignment.Center;
                sprite.Draw(center);
            }

            Display.EndFrame();
            Core.KeepAlive();
        }
    }
}
