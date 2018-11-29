using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AgateLib.UserInterface
{
    public class Image : RenderElement<ImageProps>
    {
        public Image(ImageProps props) : base(props)
        {
        }

        public override string StyleTypeId => "image";

        private Size DestSize
        {
            get
            {
                if (Props.Image == null)
                    return new Size(1, 1);

                Size result;

                if (Props.SourceRect != null)
                {
                    result = Props.SourceRect.Value.Size;
                }
                else
                {
                    result = new Size(Props.Image.Width, Props.Image.Height);
                }

                result.Width = (int)Math.Ceiling(result.Width * Props.Scale.X);
                result.Height = (int)Math.Ceiling(result.Height * Props.Scale.Y);

                return result;
            }
        }

        public override Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext, Size maxSize)
        {
            return DestSize;
        }

        public override void DoLayout(IUserInterfaceRenderContext renderContext, Size size) { }

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            if (Props.Image == null)
                return;

            Rectangle destRect = new Rectangle(clientArea.Location, DestSize);

            if (destRect.Width > clientArea.Width)
                destRect.Width = clientArea.Width;
            if (destRect.Height > clientArea.Height)
                destRect.Height = clientArea.Height;

            if (Props.SourceRect != null)
            {
                renderContext.Draw(Props.Image, 
                                   destRect, 
                                   Props.SourceRect.Value, 
                                   Props.Color);
            }
            else
            {
                renderContext.Draw(Props.Image, 
                                   destRect, 
                                   Props.Color);
            }
        }
    }

    public class ImageProps : RenderElementProps
    {
        public Texture2D Image { get; set; }

        public Rectangle? SourceRect { get; set; }

        public Vector2 Scale { get; set; } = new Vector2(1, 1);

        public Color Color { get; set; } = Color.White;
    }
}
