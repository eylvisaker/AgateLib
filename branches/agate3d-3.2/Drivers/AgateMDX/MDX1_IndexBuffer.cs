using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.ImplementationBase;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace AgateMDX
{
	class MDX1_IndexBuffer : IndexBufferImpl 
	{
		MDX1_Display mDisplay;
		IndexBufferType mType;
		int mCount;
		Direct3D.IndexBuffer mBuffer;

		public MDX1_IndexBuffer(MDX1_Display disp, IndexBufferType type, int count)
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

		private void CreateIndexBuffer()
		{
			int indexSize = 2;
			
			if (IndexType == IndexBufferType.Int32)
				indexSize = 4;

			mBuffer = new Microsoft.DirectX.Direct3D.IndexBuffer(
				mDisplay.D3D_Device.Device, 
				mCount * indexSize, 
				Microsoft.DirectX.Direct3D.Usage.WriteOnly,
				Microsoft.DirectX.Direct3D.Pool.Managed,
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
			mBuffer.SetData(indices, 0, Microsoft.DirectX.Direct3D.LockFlags.Discard);
		}

		public override void WriteIndices(short[] indices)
		{
			mBuffer.SetData(indices, 0, Microsoft.DirectX.Direct3D.LockFlags.Discard);
		}
	}
}
