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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL
{
	/// <summary>
	/// OpenGL 3.1 compatible.
	/// </summary>
	public class GL_IndexBuffer : IndexBufferImpl 
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

		public override void Dispose()
		{
			GL.DeleteBuffer(mBufferID);
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
