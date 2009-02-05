using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
    public sealed class VertexBuffer
    {
        VertexBufferImpl impl;

        public VertexBuffer()
        {
            impl = Display.Impl.CreateVertexBuffer();
        }

        public void WriteVertexData(Vector3[] data)
        {
            impl.WriteVertexData(data);
        }
        public void WriteTextureCoords(Vector2[] texCoords)
        {
            impl.WriteTextureCoords(texCoords);
        }
        public void WriteNormalData(Vector3[] data)
        {
            impl.WriteNormalData(data);
        }

        public void Draw()
        {
            impl.Draw();
        }

        public PrimitiveType PrimitiveType
        {
            get { return impl.PrimitiveType; }
            set { impl.PrimitiveType = value; }
        }
        public Surface Texture
        {
            get { return impl.Texture; }
            set { impl.Texture = value; }
        }



    }
}
