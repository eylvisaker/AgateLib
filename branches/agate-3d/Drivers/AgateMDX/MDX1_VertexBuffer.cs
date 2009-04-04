using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace AgateMDX
{
    class MDX1_VertexBuffer : VertexBufferImpl 
    {
        MDX1_Display mDisplay;

        public MDX1_VertexBuffer(MDX1_Display display)
        {
            mDisplay = display;
        }


        public override void WriteNormalData(Vector3[] data)
        {
        }
        public override void WriteTextureCoords(Vector2[] texCoords)
        {
        }
        public override void WriteVertexData(Vector3[] data)
        {
        }
        public override void WriteIndices(short[] indices)
        {
        }

        public override void Draw(int vertexStart, int vertexCount)
        {
        }

        public override void DrawIndexed(int indexStart, int indexCount)
        {
        }

        public override int IndexCount
        {
            get { return 0; }
        }

        public override int VertexCount
        {
            get { return 0; }
        }

    }
}
