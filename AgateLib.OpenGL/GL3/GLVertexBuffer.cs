//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.VertexTypes;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL.GL3
{
	/// <summary>
	/// Not OpenGL 3.1 compatible.
	/// Need replacements for SetClientStates/IndexPointer/NormalPointer/VertexPointer
	/// </summary>
	public class GLVertexBuffer : VertexBufferImpl
	{
		IGL_Display mDisplay;
		GLDrawBuffer mDrawBuffer;

		int mVertexCount;
		int mVertexBufferID;

		VertexLayout mLayout;

		public GLVertexBuffer(VertexLayout layout, int count)
		{
			mDisplay = Display.Impl as IGL_Display;
			mDrawBuffer = mDisplay.DrawBuffer;
			mVertexCount = count;
			mLayout = layout;

			GL.GenBuffers(1, out mVertexBufferID);
			Debug.Print("Vertex buffer ID: {0}", mVertexBufferID);

		}

		public override void Dispose()
		{
			GL.DeleteBuffer(mVertexBufferID);
		}

		public override void Write<T>(T[] vertices)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
			int size = vertices.Length * Marshal.SizeOf(typeof(T));

			GCHandle h = new GCHandle();

			try
			{
				h = GCHandle.Alloc(vertices, GCHandleType.Pinned);

				IntPtr arrayptr = Marshal.UnsafeAddrOfPinnedArrayElement(vertices, 0);

				unsafe
				{
					byte* ptr = (byte*)arrayptr;

					GL.BufferData(
						BufferTarget.ArrayBuffer,
						(IntPtr)size,
						(IntPtr)ptr,
						BufferUsageHint.StaticDraw);
				}
			}
			finally
			{
				h.Free();
			}
		}

		public override void Draw(int start, int count)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
			SetClientStates();
			var beginMode = MapPrimitiveType();

			GL.DrawArrays(beginMode, start, count);
		}
		public override void DrawIndexed(IndexBuffer indexbuffer, int start, int count)
		{
			GL_IndexBuffer gl_indexbuffer = (GL_IndexBuffer)indexbuffer.Impl;

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, gl_indexbuffer.BufferID);
			GL.IndexPointer(IndexPointerType.Short, 0, start);

			GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
			SetClientStates();

			var beginMode = MapPrimitiveType();
			GL.DrawElements(beginMode, count, DrawElementsType.UnsignedShort, (IntPtr)0);
		}

		private void SetAttributes()
		{
			/*
			GlslShader shader = Display.Shader as GlslShader;
			if (shader == null)
				return;

			for (int i = 0; i < mAttributeBuffers.Count; i++)
			{
				if (shader.Attributes.Contains(mAttributeBuffers[i].Name) == false)
					continue;

				int size = mAttributeBuffers[i].ComponentCount;
				int shaderAttribIndex = shader.GetAttribLocation(mAttributeBuffers[i].Name);

				GL.EnableVertexAttribArray(shaderAttribIndex);
				GL.BindBuffer(BufferTarget.ArrayBuffer, mAttributeBuffers[i].BufferID);
				GL.VertexAttribPointer(shaderAttribIndex, size,
					mAttributeBuffers[i].Type,
					false, 0, IntPtr.Zero);
			}*/

		}

		public void SetGLColor(Color color)
		{
			GL.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}
		private void SetClientStates()
		{
			SetGLColor(Color.White);

			if (UseTexture)
				SetTextures();
			else
			{
				GL.Disable(EnableCap.Texture2D);
				GL.DisableClientState(ArrayCap.TextureCoordArray);
			}

			if (HasNormals)
			{
				GL.EnableClientState(ArrayCap.NormalArray);
				GL.NormalPointer(NormalPointerType.Float, mLayout.VertexSize,
					(IntPtr)mLayout.ElementByteIndex(VertexElement.Normal));
			}
			else
			{
				GL.DisableClientState(ArrayCap.NormalArray);
			}

			if (HasPositions)
			{
				GL.EnableClientState(ArrayCap.VertexArray);
				GL.VertexPointer(
					PositionSize / sizeof(float), VertexPointerType.Float, mLayout.VertexSize,
					mLayout.ElementByteIndex(VertexElement.Position));
			}

			GL.DisableClientState(ArrayCap.ColorArray);


			SetAttributes();
		}

		private void SetTextures()
		{
			GL.Enable(EnableCap.Texture2D);

			if (HasTextureCoords)
			{
				GL.EnableClientState(ArrayCap.TextureCoordArray);
				GL.TexCoordPointer(
						2, TexCoordPointerType.Float, mLayout.VertexSize,
						(IntPtr)mLayout.ElementByteIndex(VertexElement.Texture));
			}


			//GlslShader shader = Display.Shader as GlslShader;

			if (Textures.ActiveTextures > 1)
			{
				//for (int i = 0; i < Textures.Count; i++)
				//{
				//    GL.ActiveTexture((TextureUnit)(TextureUnit.Texture0 + i));

				//    Surface surf = Textures[i];

				//    if (surf != null)
				//    {
				//        GL_Surface gl_surf = (GL_Surface)Textures[i].Impl;

				//        GL.Enable(EnableCap.Texture2D);
				//        GL.BindTexture(TextureTarget.Texture2D, gl_surf.GLTextureID);

				//        if (shader != null)
				//        {
				//            if (i < shader.Sampler2DUniforms.Count)
				//            {
				//                shader.SetUniform(shader.Sampler2DUniforms[i], i);
				//            }
				//        }
				//    }
				//    else
				//    {
				//        GL.Disable(EnableCap.Texture2D);
				//        GL.BindTexture(TextureTarget.Texture2D, 0);
				//    }
				//}
			}
			else
			{
				GL.BindTexture(TextureTarget.Texture2D, ((IGL_Surface)Textures[0].Impl).GLTextureID);
			}
		}

		private OpenTK.Graphics.OpenGL.PrimitiveType MapPrimitiveType()
		{
			OpenTK.Graphics.OpenGL.PrimitiveType primitiveType;

			switch (PrimitiveType)
			{
				case AgateLib.DisplayLib.PrimitiveType.TriangleList: primitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles; break;
				case AgateLib.DisplayLib.PrimitiveType.TriangleFan: primitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.TriangleFan; break;
				case AgateLib.DisplayLib.PrimitiveType.TriangleStrip: primitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStrip; break;

				default:
					throw new AgateException(string.Format(
						"Unsupported PrimitiveType {0}", PrimitiveType));
			}

			return primitiveType;
		}

		private static void CheckError()
		{
			ErrorCode err = GL.GetError();

			if (err != ErrorCode.NoError)
				System.Diagnostics.Debug.Print("Error: {0}", err);
		}

		public override int VertexCount
		{
			get { return mVertexCount; }
		}

		bool UseTexture
		{
			get { return HasTextureCoords && Textures.ActiveTextures != 0; }
		}
		public bool HasTextureCoords
		{
			get { return mLayout.ContainsElement(VertexElement.Texture); }
		}
		public bool HasNormals
		{
			get { return mLayout.ContainsElement(VertexElement.Normal); }
		}
		public bool HasPositions
		{
			get { return mLayout.ContainsElement(VertexElement.Position); }
		}
		public int PositionSize
		{
			get
			{
				VertexElementDesc d = mLayout.GetElement(VertexElement.Position);

				return VertexLayout.SizeOf(d.DataType);
			}
		}

		public override VertexLayout VertexLayout
		{
			get { return mLayout; }
		}
	}
}
