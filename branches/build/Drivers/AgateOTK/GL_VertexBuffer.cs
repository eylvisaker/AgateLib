﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using AgateLib.ImplementationBase;
using OpenTK.Graphics;

namespace AgateOTK
{
	public class GL_VertexBuffer : VertexBufferImpl
	{
		GL_Display mDisplay;
		GLState mState;

		struct AttributeData
		{
			public string Name;
			public int BufferID;
			public VertexAttribPointerType Type;
			public int ComponentCount;
		}

		int mVertexCount;
		int mVertexBufferID;

		List<AttributeData> mAttributeBuffers = new List<AttributeData>();

		VertexLayout mLayout;

		public GL_VertexBuffer(VertexLayout layout, int count)
		{
			mDisplay = Display.Impl as GL_Display;
			mState = mDisplay.State;
			mVertexCount = count;
			mLayout = layout;

			GL.GenBuffers(1, out mVertexBufferID);
		}

		public override void  Write<T>(T[] vertices)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
			int size = vertices.Length * Marshal.SizeOf(typeof(T));

			GCHandle h = GCHandle.Alloc(vertices, GCHandleType.Pinned);

			IntPtr arrayptr = Marshal.UnsafeAddrOfPinnedArrayElement(vertices, 0);

			unsafe
			{
				byte* ptr = (byte*)arrayptr;

				GL.BufferData(
					BufferTarget.ArrayBuffer,
					(IntPtr)(vertices.Length * Marshal.SizeOf(typeof(T))),
					(IntPtr)ptr,
					BufferUsageHint.StaticDraw);
			}

			h.Free();

		}

		public override void Draw(int start, int count)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
			SetClientStates();
			BeginMode beginMode = SelectBeginMode();

			GL.DrawArrays(beginMode, start, count);
		}
		public override void DrawIndexed(IndexBuffer indexbuffer, int start, int count)
		{
			GL_IndexBuffer gl_indexbuffer = (GL_IndexBuffer) indexbuffer.Impl;

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, gl_indexbuffer.BufferID);
			GL.IndexPointer(IndexPointerType.Short, 0, (IntPtr)start);

			GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
			SetClientStates();

			BeginMode beginMode = SelectBeginMode();
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
			throw new NotImplementedException();

		}

		private void SetClientStates()
		{
			mState.SetGLColor(Color.White);

			if (UseTexture)
				SetTextures();
			else
			{
				GL.Disable(EnableCap.Texture2D);
				GL.DisableClientState(EnableCap.TextureCoordArray);
			}

			if (HasNormals)
			{
				GL.EnableClientState(EnableCap.NormalArray);
				GL.NormalPointer(NormalPointerType.Float, mLayout.VertexSize, 
					(IntPtr) mLayout.ElementByteIndex(VertexElement.Normal));
			}
			else
			{
				GL.DisableClientState(EnableCap.NormalArray);
			}

			if (HasPositions)
			{
				GL.EnableClientState(EnableCap.VertexArray);
				GL.VertexPointer(
					PositionSize / sizeof(float), VertexPointerType.Float, mLayout.VertexSize,
					(IntPtr)mLayout.ElementByteIndex(VertexElement.Position));
			}

			SetAttributes();
		}

		private void SetTextures()
		{
			GL.Enable(EnableCap.Texture2D);

			if (HasTextureCoords)
			{
				GL.EnableClientState(EnableCap.TextureCoordArray);
				GL.TexCoordPointer(
						2, TexCoordPointerType.Float, mLayout.VertexSize,
						(IntPtr)mLayout.ElementByteIndex(VertexElement.Texture));
			}

			/*
			GlslShader shader = Display.Shader as GlslShader;

			if (Textures.ActiveTextures > 1)
			{
				for (int i = 0; i < Textures.Count; i++)
				{
					GL.ActiveTexture((TextureUnit)(TextureUnit.Texture0 + i));

					Surface surf = Textures[i];

					if (surf != null)
					{
						GL_Surface gl_surf = (GL_Surface)Textures[i].Impl;

						GL.Enable(EnableCap.Texture2D);
						GL.BindTexture(TextureTarget.Texture2D, gl_surf.GLTextureID);

						if (shader != null)
						{
							if (i < shader.Sampler2DUniforms.Count)
							{
								shader.SetUniform(shader.Sampler2DUniforms[i], i);
							}
						}
					}
					else
					{
						GL.Disable(EnableCap.Texture2D);
						GL.BindTexture(TextureTarget.Texture2D, 0);
					}
				}
			}
			else
			{
				GL.BindTexture(TextureTarget.Texture2D, ((GL_Surface)Textures[0].Impl).GLTextureID);
			}
			 * */
			throw new NotImplementedException();
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