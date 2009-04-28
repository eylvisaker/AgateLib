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

		struct AttributeData
		{
			public string Name;
			public int BufferID;
			public VertexAttribPointerType Type;
			public int ComponentCount;
		}

		int mVertexCount, mIndexCount;
		int mVertexBufferID;
		int mIndexBufferID;
		int mTexCoordBufferID;
		int mNormalBufferID;
		List<AttributeData> mAttributeBuffers = new List<AttributeData>();

		VertexLayout layout;

		public GL_VertexBuffer(VertexLayout layout)
		{
			mDisplay = Display.Impl as GL_Display;
			mState = mDisplay.State;
			this.layout = layout;
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
		private int CreateIndexBuffer(short[] data)
		{
			int bufferID;
			GL.GenBuffers(1, out bufferID);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bufferID);

			unsafe
			{
				fixed (short* ptr = data)
				{
					GL.BufferData(
						BufferTarget.ElementArrayBuffer,
						(IntPtr)(data.Length * Marshal.SizeOf(typeof(short))),
						(IntPtr)ptr,
						BufferUsageHint.StaticDraw);
				}
			}

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
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
		public override void WriteIndices(short[] indices)
		{
			mIndexBufferID = CreateIndexBuffer(indices);
			mIndexCount = indices.Length;
		}
		public override void WriteAttributeData(string attributeName, Vector3[] data)
		{
			AttributeData d = new AttributeData { Name = attributeName };

			d.BufferID = CreateBuffer(data);
			d.Type = VertexAttribPointerType.Float;
			d.ComponentCount = 3;

			mAttributeBuffers.Add(d);
		}
		public override void Draw(int start, int count)
		{
			SetClientStates();
			BeginMode beginMode = SelectBeginMode();

			if (Indexed)
			{
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, mIndexBufferID);
				GL.IndexPointer(IndexPointerType.Short, 0, (IntPtr)start);

				GL.EnableClientState(EnableCap.VertexArray);
				GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
				GL.VertexPointer(3, VertexPointerType.Float, 0, (IntPtr)0);

				GL.DrawElements(beginMode, count, DrawElementsType.UnsignedShort, (IntPtr)0);
			}
			else
			{
				GL.EnableClientState(EnableCap.VertexArray);
				GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferID);
				GL.VertexPointer(3, VertexPointerType.Float, 0, (IntPtr)start);

				GL.DrawArrays(beginMode, start, count);
			}

		}

		private void SetAttributes()
		{
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
			}
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

			SetAttributes();
		}

		private void SetTextures()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.EnableClientState(EnableCap.TextureCoordArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, mTexCoordBufferID);
			GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);

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

		public override int IndexCount
		{
			get { return mIndexCount; }
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
			get { return mTexCoordBufferID != 0; }
		}
		public bool HasNormals
		{
			get { return mNormalBufferID != 0; }
		}
	}
}
