using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.ImplementationBase;
using OpenTK.Graphics;

namespace AgateOTK
{
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

		private short[] ConvertToInt16(int[] indices)
		{
			short[] retval = new short[indices.Length];

			int i = 0;

			try
			{
				checked
				{
					for (i = 0; i < retval.Length; i++)
					{
						retval[i] = (short)indices[i];
					}
				}
			}
			catch (OverflowException ex)
			{
				throw new AgateLib.AgateException(ex, string.Format(
					"A 16 bit index buffer cannot contain values greater than {0}, " +
					"but there is a value of {1} at index {2}.",
					short.MaxValue, indices[i], i));
			}

			return retval;
		}

		unsafe public override void WriteIndices(int[] indices)
		{
			if (indices.Length != Count)
				throw new AgateLib.AgateException(
					"The size of the passed array must match the size of the index buffer.");

			if (mType == IndexBufferType.Int16)
			{
				WriteIndices(ConvertToInt16(indices));
				return;
			}

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
