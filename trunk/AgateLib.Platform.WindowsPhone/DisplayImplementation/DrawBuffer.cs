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
using AgateLib.DisplayLib;
using AgateLib.Geometry.VertexTypes;
using Texture2D = SharpDX.Direct3D11.Texture2D;
using SharpDX.Direct3D11;
using SharpDX.SimpleInitializer;

namespace AgateLib.Platform.WindowsPhone.DisplayImplementation
{
	/// <summary>
	/// Perhaps at some point this should be converted to use a vertex buffer
	/// instead of a vertex array.
	/// </summary>
	public class DrawBuffer
	{
		const int vertPageSize = 1000;
		int pages = 1;

		SharpDXContext mContext;
		D3DDevice mDevice;

		PositionTextureColor[] mVerts;
		short[] mIndices;

		int mVertPointer = 0;
		int mIndexPointer = 0;

		Texture2D mTexture;
		ShaderResourceView mTextureView;
		bool mAlphaBlend;

		SharpDX.Direct3D11.Buffer mVertexBuffer;
		SharpDX.Direct3D11.Buffer mIndexBuffer;
		SharpDX.Direct3D11.InputLayout mVertexLayout;
		VertexBufferBinding mVertexBinding;

		public DrawBuffer(D3DDevice device, SharpDXContext context)
		{
			mContext = context;
			mDevice = device;

			mContext.DeviceReset += Context_DeviceReset;

			AllocateVerts();

			
		}

		void Context_DeviceReset(object sender, SharpDX.SimpleInitializer.DeviceResetEventArgs e)
		{
			AllocateHardwareResources();
		}

		public void AllocateHardwareResources()
		{
			if (mDevice.Device == null)
				return;

			if (mVertexLayout != null)
			{
				mVertexLayout.Dispose();
				mVertexBuffer.Dispose();
				mIndexBuffer.Dispose();
			}

			mVertexLayout = new SharpDX.Direct3D11.InputLayout(
				mDevice.Device, (byte[])Shaders.ShaderResources.ResourceManager.GetObject("Basic2Dvert"),
				new[]           {
                new SharpDX.Direct3D11.InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32A32_Float, 0, 0),
                new SharpDX.Direct3D11.InputElement("TEXCOORD", 0, SharpDX.DXGI.Format.R32G32_Float, 16, 0),
				new SharpDX.Direct3D11.InputElement("COLOR", 0, SharpDX.DXGI.Format.R8G8B8A8_UInt, 0)
            });

			var layout = PositionTextureColor.VertexLayout;

			mVertexBuffer = new SharpDX.Direct3D11.Buffer(
				mDevice.Device,
				new BufferDescription(mVerts.Length * layout.VertexSize,
					ResourceUsage.Dynamic,
					BindFlags.VertexBuffer,
					CpuAccessFlags.Write,
					ResourceOptionFlags.None,
					layout.VertexSize));

			mIndexBuffer = new SharpDX.Direct3D11.Buffer(
				mDevice.Device,
				new BufferDescription(mIndices.Length * 2,
					ResourceUsage.Dynamic,
					BindFlags.IndexBuffer,
					CpuAccessFlags.Write,
					ResourceOptionFlags.None,
					2));

			mVertexBinding = new VertexBufferBinding(mVertexBuffer, layout.VertexSize, 0);
		}

		private void AllocateVerts()
		{
			mVerts = new PositionTextureColor[vertPageSize * pages];
			mIndices = new short[vertPageSize / 2 * 3 * pages];

			AllocateHardwareResources();
		}

		public void CacheDrawIndexedTriangles(
			PositionTextureColor[] verts, short[] indices,
			Texture2D texture, ShaderResourceView textureView, bool alphaBlend)
		{
			if (mTexture != texture || mAlphaBlend != alphaBlend)
			{
				Flush();

				mTexture = texture;
				mTextureView = textureView;
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

			mDevice.AlphaBlend = mAlphaBlend;
			//mDevice.AlphaArgument1 = TextureArgument.Texture;
			//mDevice.AlphaArgument2 = TextureArgument.Diffuse;
			//mDevice.AlphaOperation = TextureOperation.Modulate;

			//mDevice.SetVertexDeclarationForSurfaces();

			SDX_Display display = (SDX_Display)Display.Impl;

			DoDraw(null);

			mVertPointer = 0;
			mIndexPointer = 0;

		}

		private void DoDraw(object ignored)
		{
			//mDevice.DeviceContext.OutputMerger.SetTargets(this.parentContext.DepthStencilView, this.parentContext.BackBufferView);

			mDevice.DeviceContext.UpdateSubresource(mVerts, mVertexBuffer);
			mDevice.DeviceContext.UpdateSubresource(mIndices, mIndexBuffer);

			mDevice.DeviceContext.InputAssembler.SetVertexBuffers(0, mVertexBinding);
			mDevice.DeviceContext.InputAssembler.SetIndexBuffer(mIndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
			mDevice.DeviceContext.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

			mDevice.DeviceContext.PixelShader.SetShaderResource(0, mTextureView);
			mDevice.DeviceContext.DrawIndexed(mIndexPointer, 0, 0);

			try
			{
				//mDevice.Device.DrawIndexedUserPrimitives
				//	(Direct3D.PrimitiveType.TriangleList, 0, mVertPointer,
				//	 mIndexPointer / 3, mIndices, Format.Index16, mVerts,
				//	 Marshal.SizeOf(typeof(PositionTextureColor)));
			}
			catch { }
		}

	}
}
