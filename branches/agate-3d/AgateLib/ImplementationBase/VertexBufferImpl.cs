using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.ImplementationBase
{
    public abstract class VertexBufferImpl
    {
        public abstract void WriteVertexData(Vector3[] data);
        public abstract void WriteTextureCoords(Vector2[] texCoords);
        public abstract void WriteNormalData(Vector3[] data);

        public virtual PrimitiveType PrimitiveType { get; set; }
        public virtual Surface Texture { get; set; }

        public virtual void Draw()
        {
            Draw(0, VertexCount);
        }
        public abstract void Draw(int vertexStart, int vertexCount);
        public virtual void DrawIndexed()
        {
            DrawIndexed(0, IndexCount);
        }
        public abstract void DrawIndexed(int indexStart, int indexCount);

        public abstract void WriteIndices(short[] indices);


        public abstract int VertexCount { get; }
        public abstract int IndexCount { get; }

    }
}
