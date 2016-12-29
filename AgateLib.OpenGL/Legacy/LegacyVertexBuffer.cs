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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL.Legacy
{
	public class LegacyVertexBuffer : VertexBufferImpl
	{
		IGL_Display mDisplay;
		GLDrawBuffer mDrawBuffer;

		int mVertexCount;
		int mVertexBufferID;

		VertexLayout mLayout;

		public LegacyVertexBuffer(VertexLayout layout, int count)
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

		public override void  Write<T>(T[] vertices)
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
			var primitiveType = SelectPrimitiveType();

			GL.DrawArrays(primitiveType, start, count);
		}
		public override void DrawIndexed(IndexBuffer indexbuffer, int start, int count)
		{
			GL_IndexBuffer gl_indexbuffer = (GL_IndexBuffer) indexbuffer.Impl;

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, gl_indexbuffer.BufferID);
			GL.IndexPointer(IndexPointerType.Short, 0, start);

			GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
			SetClientStates();

			var beginMode = SelectPrimitiveType();

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
					(IntPtr) mLayout.ElementByteIndex(VertexElement.Normal));
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

		private OpenTK.Graphics.OpenGL.PrimitiveType SelectPrimitiveType()
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
