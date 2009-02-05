using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;
using OpenTK.Graphics;
using AgateLib.DisplayLib;

namespace AgateOTK
{
    public class GL_VertexBuffer : VertexBufferImpl
    {
        GL_Display mDisplay;
        GLState mState;
        GL_Surface mTexture;

        int mVertexCount;
        int mVertexBufferID;
        int mTexCoordBufferID;
        int mNormalBufferID;

        public GL_VertexBuffer()
        {
            mDisplay = Display.Impl as GL_Display;
            mState = mDisplay.State;
        }

        public override Surface Texture
        {
            get { return base.Texture; }
            set
            {
                base.Texture = value;
                mTexture = (GL_Surface)value.Impl;
            }
        }
        // FYI: use BufferTarget.ElementArrayBuffer to bind to an index buffer.

        private int CreateBuffer(Vector3[] data)
        {
            int bufferID;
            GL.GenBuffers(1, out bufferID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);

            unsafe
            {
                fixed (Vector3* ptr = data)
                {
                    GL.BufferData(
                        BufferTarget.ArrayBuffer,
                        (IntPtr)(data.Length * Marshal.SizeOf(typeof(Vector3))),
                        (IntPtr)ptr,
                        BufferUsageHint.StaticDraw);
                }
            }
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return bufferID;
        }
        private int CreateBuffer(Vector2[] data)
        {
            int bufferID;
            GL.GenBuffers(1, out bufferID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);

            unsafe
            {
                fixed (Vector2* ptr = data)
                {
                    GL.BufferData(
                        BufferTarget.ArrayBuffer,
                        (IntPtr)(data.Length * Marshal.SizeOf(typeof(Vector2))),
                        (IntPtr)ptr,
                        BufferUsageHint.StaticDraw);
                }
            }
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); 
            return bufferID;
        }

        public override void WriteVertexData(Vector3[] data)
        {
            mVertexBufferID = CreateBuffer(data);
            mVertexCount = data.Length;
        }
        public override void WriteTextureCoords(Vector2[] texCoords)
        {
            mTexCoordBufferID = CreateBuffer(texCoords);
        }
        public override void WriteNormalData(Vector3[] data)
        {
            mNormalBufferID = CreateBuffer(data);
        }
        public override void Draw()
        {
            mState.SetGLColor(Color.White);

            BeginMode beginMode = SelectBeginMode();

            GL.EnableClientState(EnableCap.VertexArray);

            if (UseTexture)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.EnableClientState(EnableCap.TextureCoordArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, mTexCoordBufferID);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
                GL.BindTexture(TextureTarget.Texture2D, mTexture.GLTextureID);
            }
            else
            {
                GL.Disable(EnableCap.Texture2D);
                GL.DisableClientState(EnableCap.TextureCoordArray);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            if (HasNormals)
            {
                GL.EnableClientState(EnableCap.NormalArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, mNormalBufferID);
                GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
            }
            else
            {
                GL.DisableClientState(EnableCap.NormalArray);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
            
            GL.DrawArrays(beginMode, 0, mVertexCount);
        }

        private BeginMode SelectBeginMode()
        {
            BeginMode beginMode;
            switch (PrimitiveType)
            {
                case PrimitiveType.TriangleList: beginMode = BeginMode.Triangles; break;
                case PrimitiveType.TriangleFan: beginMode = BeginMode.TriangleFan; break;
                case PrimitiveType.TriangleStrip: beginMode = BeginMode.TriangleStrip; break;

                default:
                    throw new AgateException(string.Format(
                        "Unsupported PrimitiveType {0}", PrimitiveType));
            }
            return beginMode;
        }

        private static void CheckError()
        {
            ErrorCode err = GL.GetError();

            if (err != ErrorCode.NoError)
                System.Diagnostics.Debug.Print("Error: {0}", err);
        }

        bool UseTexture
        {
            get { return HasTextureCoords && mTexture != null; }
        }
        public bool HasTextureCoords
        {
            get { return mTexCoordBufferID != 0; }
        }
        public bool HasNormals
        {
            get { return mNormalBufferID != 0; }
        }
    }
}
