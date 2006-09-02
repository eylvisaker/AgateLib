using System;
using System.Collections.Generic;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using CustomVertex = Microsoft.DirectX.Direct3D.CustomVertex;

namespace ERY.AgateLib.MDX
{
    public class DrawBuffer
    {
        const int vertPageSize = 1000;
        int pages = 1;

        D3DDevice mDevice;

        CustomVertex.TransformedColoredTextured []mVerts;
        short[] mIndices;

        int mVertPointer = 0;
        int mIndexPointer = 0;

        Texture mTexture;
        bool mAlphaBlend;

        public DrawBuffer(D3DDevice device)
        {
            mDevice = device;

            AllocateVerts();
        }

        private void AllocateVerts()
        {
            mVerts = new CustomVertex.TransformedColoredTextured[vertPageSize * pages];
            mIndices = new short[vertPageSize / 2 * 3 * pages];
        }
        public void CacheDrawIndexedTriangles(CustomVertex.TransformedColoredTextured[] verts, short[] indices,
            Texture texture, bool alphaBlend)
        {
            if (mTexture != texture || mAlphaBlend != alphaBlend)
            {
                Flush();

                mTexture = texture;
                mAlphaBlend = alphaBlend;
            }

            // increase the number of vertex pages if we don't have enough space.
            if (mVertPointer + verts.Length > mVerts.Length)
            {
                Flush();

                if (pages < 32)
                    pages++;

                AllocateVerts();
            }

            verts.CopyTo(mVerts, mVertPointer);

            for (int i = 0; i < indices.Length; i++)
                mIndices[i + mIndexPointer] = (short)( indices[i] + mVertPointer);

            mVertPointer += verts.Length;
            mIndexPointer += indices.Length;

        }

        public void Flush()
        {
            if (mVertPointer == 0)
                return;

            mDevice.SetDeviceStateTexture(mTexture);
            mDevice.AlphaBlend = mAlphaBlend;
            mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;

            mDevice.Device.DrawIndexedUserPrimitives
                (PrimitiveType.TriangleList, 0, mVertPointer, mIndexPointer / 3, mIndices, true, mVerts);
            
            mVertPointer = 0;
            mIndexPointer = 0;

        }

    }
}
