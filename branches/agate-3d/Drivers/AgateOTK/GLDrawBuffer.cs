//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using AgateLib;
using AgateLib.Geometry;

using OpenTK.Graphics;

namespace AgateOTK
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
        public void ResetTexture()
        {
            Flush();

            mCurrentTexture = 0;
        }

        PointF[] cachePts = new PointF[4];

        public void AddQuad(int textureID, Color color, TextureCoordinates texCoord, RectangleF destRect)
        {
            PointF[] pt = cachePts;

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
        public void AddQuad(int textureID, Color color, TextureCoordinates texCoord, PointF[] pts)
        {
            AddQuad(textureID, new Gradient(color), texCoord, pts);
        }
        public void AddQuad(int textureID, Gradient color, TextureCoordinates texCoord, PointF[] pts)
        {
            SetTexture(textureID);

            if (mIndex + 4 >= mVertexCoords.Length)
            {
                Flush();
                SetBufferSize(mVertexCoords.Length + 1000);
            }

            for (int i = 0; i < 4; i++)
            {
                mVertexCoords[mIndex + i].x = pts[i].X;
                mVertexCoords[mIndex + i].y = pts[i].Y;

                mNormalCoords[mIndex + i].x = 0;
                mNormalCoords[mIndex + i].y = 0;
                mNormalCoords[mIndex + i].z = -1;

            }

            mTexCoords[mIndex].u = texCoord.Left;
            mTexCoords[mIndex].v = texCoord.Top;
            mColorCoords[mIndex] = new ColorCoord(color.TopLeft);

            mTexCoords[mIndex + 1].u = texCoord.Right;
            mTexCoords[mIndex + 1].v = texCoord.Top;
            mColorCoords[mIndex+1] = new ColorCoord(color.TopRight);

            mTexCoords[mIndex + 2].u = texCoord.Right;
            mTexCoords[mIndex + 2].v = texCoord.Bottom;
            mColorCoords[mIndex+2] = new ColorCoord(color.BottomRight);

            mTexCoords[mIndex + 3].u = texCoord.Left;
            mTexCoords[mIndex + 3].v = texCoord.Bottom;
            mColorCoords[mIndex+3] = new ColorCoord(color.BottomLeft);

            
            mIndex += 4;

        }

        public void Flush()
        {
            if (mIndex == 0)
                return;

            GL.BindTexture(TextureTarget.Texture2D, mCurrentTexture);

            GL.EnableClientState(EnableCap.TextureCoordArray);
            GL.EnableClientState(EnableCap.ColorArray);
            GL.EnableClientState(EnableCap.VertexArray);
            GL.EnableClientState(EnableCap.NormalArray);

            GL.TexCoordPointer(2, TexCoordPointerType.Float,
                               Marshal.SizeOf(typeof(TexCoord)), mTexCoords);
            GL.ColorPointer(4, ColorPointerType.Float,
                            Marshal.SizeOf(typeof(ColorCoord)), mColorCoords);
            GL.VertexPointer(2, VertexPointerType.Float,
                             Marshal.SizeOf(typeof(VertexCoord)), mVertexCoords);
            GL.NormalPointer(NormalPointerType.Float,
                             Marshal.SizeOf(typeof(NormalCoord)), mNormalCoords);
            GL.DrawArrays(BeginMode.Quads, 0, mIndex);

            mIndex = 0;
        }

    }
}
