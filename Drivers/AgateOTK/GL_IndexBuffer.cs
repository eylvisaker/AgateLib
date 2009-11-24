using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.ImplementationBase;
using OpenTK.Graphics.OpenGL;

namespace AgateOTK
{
	/// <summary>
	/// OpenGL 3.1 compatible.
	/// </summary>
	class GL_IndexBuffer : IndexBufferImpl 
	{
		int mCount;
		IndexBufferType mType;
		int mBufferID;

		public GL_IndexBuffer(IndexBufferType type, int count)
		{
			mCount = count;
			mType = type;

			CreateBuffer();

			System.Diagnostics.Debug.Print("Created {0} index buffer.", type);
		}

		private void CreateBuffer()
		{
			GL.GenBuffers(1, out mBufferID);
		}

		public override int Count
		{
			get { return mCount; }
		}
		public override IndexBufferType IndexType
		{
			get { return mType; }
		}
		public int BufferID
		{
			get { return mBufferID; }
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, mBufferID);
		}

		unsafe public override void WriteIndices(int[] indices)
		{
			if (indices.Length != Count)
				throw new AgateLib.AgateException(
					"The size of the passed array must match the size of the index buffer.");

			if (mType == IndexBufferType.Int16)
				throw new ArgumentException(
					"Cannot write 32-bit data to a 16-bit buffer.");

			Bind();

			fixed (int* ptr = indices)
			{
				GL.BufferData(
					BufferTarget.ElementArrayBuffer,
					(IntPtr)(indices.Length * Marshal.SizeOf(typeof(int))),
					(IntPtr)ptr,
					BufferUsageHint.StaticDraw);
			}
		}

		unsafe public override void WriteIndices(short[] indices)
		{
			if (indices.Length != Count)
				throw new AgateLib.AgateException(
					"The size of the passed array must match the size of the index buffer.");

			Bind();

			fixed (short* ptr = indices)
			{
				GL.BufferData(
					BufferTarget.ElementArrayBuffer,
					(IntPtr)(indices.Length * Marshal.SizeOf(typeof(short))),
					(IntPtr)ptr,
					BufferUsageHint.StaticDraw);
			}
		}

	}
}
