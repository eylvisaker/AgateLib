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

        #endregion

        GLState mState;

        TexCoord[] mTexCoords;
        ColorCoord[] mColorCoords;
        VertexCoord[] mVertexCoords;

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

            Gl.TexCoordPointer(2, Enums.TexCoordPointerType.FLOAT,
                               Marshal.SizeOf(typeof(TexCoord)), mTexCoords);
            Gl.ColorPointer(4, Enums.ColorPointerType.FLOAT,
                            Marshal.SizeOf(typeof(ColorCoord)), mColorCoords);
            Gl.VertexPointer(2, Enums.VertexPointerType.FLOAT,
                             Marshal.SizeOf(typeof(VertexCoord)), mVertexCoords);

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
