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
using System.Runtime.InteropServices;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using Direct3D = SlimDX.Direct3D9;
using AgateLib.DisplayLib;
using AgateLib.Geometry.VertexTypes;

namespace AgateSDX
{
	/// <summary>
	/// Perhaps at some point this should be converted to use a vertex buffer
	/// instead of a vertex array.
	/// </summary>
	public class DrawBuffer
	{
		const int vertPageSize = 1000;
		int pages = 1;

		D3DDevice mDevice;

		PositionTextureColor[] mVerts;
		short[] mIndices;

		int mVertPointer = 0;
		int mIndexPointer = 0;

		Texture mTexture;
		bool mAlphaBlend;

		public DrawBuffer(D3DDevice device)
		{
			mDevice = device;

			AllocateVerts();
		}

		private void AllocateVerts()
		{
			mVerts = new PositionTextureColor[vertPageSize * pages];
			mIndices = new short[vertPageSize / 2 * 3 * pages];
		}
		public void CacheDrawIndexedTriangles(PositionTextureColor[] verts, short[] indices,
			Texture texture, bool alphaBlend)
		{
			if (mTexture != texture || mAlphaBlend != alphaBlend)
			{
				Flush();

				mTexture = texture;
				mAlphaBlend = alphaBlend;
			}

			// increase the number of vertex pages if we don't have enough space.
			while (mVertPointer + verts.Length > mVerts.Length)
			{
				Flush();

				// this is an arbitrary cap on the size of the vertex array.
				if (pages < 32)
					pages++;

				AllocateVerts();
			}

			verts.CopyTo(mVerts, mVertPointer);

			for (int i = 0; i < indices.Length; i++)
				mIndices[i + mIndexPointer] = (short)(indices[i] + mVertPointer);

			mVertPointer += verts.Length;
			mIndexPointer += indices.Length;

		}

		public void Flush()
		{
			if (mVertPointer == 0)
				return;

			mDevice.SetDeviceStateTexture(mTexture);
			mDevice.AlphaBlend = mAlphaBlend;
			mDevice.AlphaArgument1 = TextureArgument.Texture;
			mDevice.AlphaArgument2 = TextureArgument.Diffuse;
			mDevice.AlphaOperation = TextureOperation.Modulate;

			mDevice.SetVertexDeclarationForSurfaces();

			SDX_Display display = (SDX_Display)Display.Impl;

			DoDraw(null);

			mVertPointer = 0;
			mIndexPointer = 0;

		}

		private void DoDraw(object ignored)
		{
			try
			{
				mDevice.Device.DrawIndexedUserPrimitives
					(Direct3D.PrimitiveType.TriangleList, 0, mVertPointer,
					 mIndexPointer / 3, mIndices, Format.Index16, mVerts,
					 Marshal.SizeOf(typeof(PositionTextureColor)));
			}
			catch { }
		}

	}
}
