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
        public void WriteIndices(short[] indices)
        {
            impl.WriteIndices(indices);
        }

        public void Draw()
        {
            impl.Draw();
        }
        public void Draw(int vertexStart, int vertexCount)
        {
            impl.Draw(vertexStart, vertexCount);
        }
        public void DrawIndexed()
        {
            impl.DrawIndexed();
        }
        public void DrawIndexed(int indexStart, int indexCount)
        {
            impl.DrawIndexed(indexStart, indexCount);
        }

        public int VertexCount
        {
            get { return impl.VertexCount; }
        }
        public int IndexCount
        {
            get { return impl.IndexCount; }
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
