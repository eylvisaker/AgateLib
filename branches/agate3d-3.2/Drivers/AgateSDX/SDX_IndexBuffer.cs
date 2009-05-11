using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.ImplementationBase;
using Direct3D = SlimDX.Direct3D9;

namespace AgateSDX
{
	class SDX_IndexBuffer : IndexBufferImpl 
	{
		SDX_Display mDisplay;
		IndexBufferType mType;
		int mCount;
		Direct3D.IndexBuffer mBuffer;
		int maxIndex;

		public SDX_IndexBuffer(SDX_Display disp, IndexBufferType type, int count)
		{
			mDisplay = disp;
			mType = type;
			mCount = count;

			CreateIndexBuffer();
		}

		public Direct3D.IndexBuffer DeviceIndexBuffer
		{
			get { return mBuffer; }
		}
		public int MaxIndex
		{
			get { return maxIndex; }
		}

		private void CreateIndexBuffer()
		{
			int indexSize = 2;
			
			if (IndexType == IndexBufferType.Int32)
				indexSize = 4;

			mBuffer = new SlimDX.Direct3D9.IndexBuffer(
				mDisplay.D3D_Device.Device, 
				mCount * indexSize, 
				SlimDX.Direct3D9.Usage.WriteOnly,
				SlimDX.Direct3D9.Pool.Managed,
				IndexType == IndexBufferType.Int16);
		}

		public override int Count
		{
			get { return mCount; }
		}

		public override AgateLib.DisplayLib.IndexBufferType IndexType
		{
			get { return mType; }
		}

		public override void WriteIndices(int[] indices)
		{
			var data = mBuffer.Lock(0, 0, 0);
			data.WriteRange(indices);
			mBuffer.Unlock();

			maxIndex = indices.Max();
		}

		public override void WriteIndices(short[] indices)
		{
			var data = mBuffer.Lock(0, 0, 0);
			data.WriteRange(indices);
			mBuffer.Unlock();

			maxIndex = indices.Max();
		}
	}
}
