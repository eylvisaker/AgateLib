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
        public abstract void Draw();

        public virtual PrimitiveType PrimitiveType { get; set; }
        public virtual Surface Texture { get; set; }

    }
}
