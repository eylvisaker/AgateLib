using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using ERY.AgateLib;
using ERY.AgateLib.Geometry;

using OpenTK.OpenGL;
using Gl = OpenTK.OpenGL.GL;

namespace ERY.AgateLib.OpenGL
{
    class GLDrawBuffer
    {
        #region --- Private types for Vertex Arrays ---

        [StructLayout(LayoutKind.Sequential)]
        private struct TexCoord
        {
            public float u;
            public float v;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct VertexCoord
        {
            public float x;
            public float y;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct ColorCoord
        {
            public float r;
            public float g;
            public float b;
            public float a;

            public ColorCoord(Color clr)
            {
                r = clr.R / 255.0f;
                g = clr.G / 255.0f;
                b = clr.B / 255.0f;
                a = clr.A / 255.0f;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct NormalCoord
        {
            public float x;
            public float y;
            public float z;

            public NormalCoord(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
        #endregion

        GLState mState;

        TexCoord[] mTexCoords;
        ColorCoord[] mColorCoords;
        VertexCoord[] mVertexCoords;
        NormalCoord[] mNormalCoords;

        int mIndex;
        int mCurrentTexture;

        public GLDrawBuffer(GLState state)
        {
            mState = state;

            SetBufferSize(1000);
        }

        private void SetBufferSize(int size)
        {
            mTexCoords = new TexCoord[size];
            mColorCoords = new ColorCoord[size];
            mVertexCoords = new VertexCoord[size];
            mNormalCoords = new NormalCoord[size];

            mIndex = 0;
        }

        private void SetTexture(int textureID)
        {
            if (textureID == mCurrentTexture)
                return;

            Flush();

            mCurrentTexture = textureID;
        }

        public void AddQuad(int textureID, Color color, TextureCoordinates texCoord, RectangleF destRect)
        {
            PointF[] pt = new PointF[4];

            pt[0].X = destRect.Left;
            pt[0].Y = destRect.Top;

            pt[1].X = destRect.Right;
            pt[1].Y = destRect.Top;

            pt[2].X = destRect.Right;
            pt[2].Y = destRect.Bottom;

            pt[3].X = destRect.Left;
            pt[3].Y = destRect.Bottom;

            AddQuad(textureID, color, texCoord, pt);
        }
        public void AddQuad(int textureID, Color color, TextureCoordinates texCoord, PointF[] pt)
        {
            SetTexture(textureID);

            if (mIndex + 4 >= mVertexCoords.Length)
            {
                Flush();
                SetBufferSize(mVertexCoords.Length + 1000);
            }

            for (int i = 0; i < 4; i++)
            {
                mVertexCoords[mIndex + i].x = pt[i].X;
                mVertexCoords[mIndex + i].y = pt[i].Y;

                mColorCoords[mIndex + i] = new ColorCoord(color);

                mNormalCoords[mIndex + i].x = 0;
                mNormalCoords[mIndex + i].y = 0;
                mNormalCoords[mIndex + i].z = -1;

            }

            mTexCoords[mIndex].u = texCoord.Left;
            mTexCoords[mIndex].v = texCoord.Top;

            mTexCoords[mIndex + 1].u = texCoord.Right;
            mTexCoords[mIndex + 1].v = texCoord.Top;

            mTexCoords[mIndex + 2].u = texCoord.Right;
            mTexCoords[mIndex + 2].v = texCoord.Bottom;

            mTexCoords[mIndex + 3].u = texCoord.Left;
            mTexCoords[mIndex + 3].v = texCoord.Bottom;

            
            mIndex += 4;

        }

        public void Flush()
        {
            if (mIndex == 0)
                return;

            Gl.BindTexture(Enums.TextureTarget.TEXTURE_2D, mCurrentTexture);

            Gl.EnableClientState(Enums.EnableCap.TEXTURE_COORD_ARRAY);
            Gl.EnableClientState(Enums.EnableCap.COLOR_ARRAY);
            GL.EnableClientState(Enums.EnableCap.VERTEX_ARRAY);
            GL.EnableClientState(Enums.EnableCap.NORMAL_ARRAY);

            Gl.TexCoordPointer(2, Enums.TexCoordPointerType.FLOAT,
                               Marshal.SizeOf(typeof(TexCoord)), mTexCoords);
            Gl.ColorPointer(4, Enums.ColorPointerType.FLOAT,
                            Marshal.SizeOf(typeof(ColorCoord)), mColorCoords);
            Gl.VertexPointer(2, Enums.VertexPointerType.FLOAT,
                             Marshal.SizeOf(typeof(VertexCoord)), mVertexCoords);
            Gl.NormalPointer(Enums.NormalPointerType.FLOAT,
                             Marshal.SizeOf(typeof(NormalCoord)), mNormalCoords);
            Gl.DrawArrays(Enums.BeginMode.QUADS, 0, mIndex);

            mIndex = 0;
        }

        private void oldFlush()
        {

            Gl.BindTexture(Enums.TextureTarget.TEXTURE_2D, mCurrentTexture);

            Gl.Begin(Enums.BeginMode.QUADS);

            for (int i = 0; i < mIndex; i++)
            {
                Gl.Color4f(mColorCoords[i].r, mColorCoords[i].g, mColorCoords[i].b, mColorCoords[i].a);
                Gl.TexCoord2f(mTexCoords[i].u, mTexCoords[i].v);
                Gl.Vertex2f(mVertexCoords[i].x, mVertexCoords[i].y);
            }

            Gl.End();

            mIndex = 0;
        }
    }
}
