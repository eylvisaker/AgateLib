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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
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
