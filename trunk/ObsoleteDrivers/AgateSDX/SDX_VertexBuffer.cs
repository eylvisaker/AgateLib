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
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using Direct3D = SlimDX.Direct3D9;

namespace AgateSDX
{
	class SDX_VertexBuffer : VertexBufferImpl
	{
		SDX_Display mDisplay;
		Direct3D.VertexBuffer mBuffer;
		Direct3D.VertexDeclaration mDeclaration;
		int mCount;
		VertexLayout mLayout;

		public SDX_VertexBuffer(SDX_Display display, VertexLayout layout, int vertexCount)
		{
			mDisplay = display;
			mCount = vertexCount;

			mDeclaration = CreateVertexDeclaration(mDisplay.D3D_Device.Device, layout);
			Direct3D.VertexFormat mFormats = CreateVertexFormat(layout);

			mLayout = layout;

			mBuffer = new SlimDX.Direct3D9.VertexBuffer(
				mDisplay.D3D_Device.Device,
				vertexCount * layout.VertexSize,
				SlimDX.Direct3D9.Usage.WriteOnly,
				mFormats,
				Direct3D.Pool.Managed);
		}

		public override VertexLayout VertexLayout
		{
			get { return mLayout; }
		}
		private Direct3D.VertexFormat CreateVertexFormat(VertexLayout layout)
		{
			Direct3D.VertexFormat retval = SlimDX.Direct3D9.VertexFormat.None;

			foreach (var element in layout)
			{
				switch (element.ElementType)
				{
					case VertexElement.Position:
						retval |= SlimDX.Direct3D9.VertexFormat.Position;
						break;

					case VertexElement.Texture:
						retval |= SlimDX.Direct3D9.VertexFormat.Texture0;
						break;

					case VertexElement.Texture1:
						retval |= SlimDX.Direct3D9.VertexFormat.Texture1;
						break;

					case VertexElement.Texture2:
						retval |= SlimDX.Direct3D9.VertexFormat.Texture2;
						break;

					case VertexElement.Texture3:
						retval |= SlimDX.Direct3D9.VertexFormat.Texture3;
						break;

					case VertexElement.DiffuseColor:
						retval |= SlimDX.Direct3D9.VertexFormat.Diffuse;
						break;
				}
			}

			return retval;
		}
		public static Direct3D.VertexDeclaration CreateVertexDeclaration(Direct3D.Device d3dDevice, VertexLayout layout)
		{
			List<Direct3D.VertexElement> formats = new List<Direct3D.VertexElement>();
			short loc = 0;

			for (int i = 0; i < layout.Count; i++)
			{
				var element = layout[i];

				Direct3D.VertexElement d3d_element = ConvertElement(element, ref loc);

				formats.Add(d3d_element);
			}

			formats.Add(Direct3D.VertexElement.VertexDeclarationEnd);

			return new Direct3D.VertexDeclaration(d3dDevice, formats.ToArray());
		}
		private static Direct3D.VertexElement ConvertElement(VertexElementDesc element, ref short loc)
		{
			Direct3D.DeclarationMethod declMethod = SlimDX.Direct3D9.DeclarationMethod.Default;
			Direct3D.DeclarationUsage declUsage;
			Direct3D.DeclarationType declType;

			int size = VertexLayout.SizeOf(element.DataType);

			switch(element.DataType)
			{
				case VertexElementDataType.Float1: 
					declType = SlimDX.Direct3D9.DeclarationType.Float1;
					break;
				case VertexElementDataType.Float2:
					declType = SlimDX.Direct3D9.DeclarationType.Float2;
					break;
				case VertexElementDataType.Float3:
					declType = SlimDX.Direct3D9.DeclarationType.Float3;
					break;
				case VertexElementDataType.Float4:
					declType = SlimDX.Direct3D9.DeclarationType.Float4;
					break;
				case VertexElementDataType.Int:
					declType = SlimDX.Direct3D9.DeclarationType.Color;
					break;
				default:
					throw new NotImplementedException(
						element.DataType.ToString() + " not implemented.");
			}

			switch(element.ElementType)
			{
				case VertexElement.Position:
					declUsage = SlimDX.Direct3D9.DeclarationUsage.Position;
					break;
				case VertexElement.Texture:
					declUsage = SlimDX.Direct3D9.DeclarationUsage.TextureCoordinate;
					break;
				case VertexElement.Normal:
					declUsage = SlimDX.Direct3D9.DeclarationUsage.Normal;
					break;
				case VertexElement.Tangent:
					declUsage = SlimDX.Direct3D9.DeclarationUsage.Tangent;
					break;
				case VertexElement.DiffuseColor:
					declUsage = SlimDX.Direct3D9.DeclarationUsage.Color;
					break;
				case VertexElement.Bitangent:
					declUsage = SlimDX.Direct3D9.DeclarationUsage.Binormal;
					break;
				default:
					throw new NotImplementedException(
						element.ElementType.ToString() + " not implemented.");
			}

			loc += (short)size;

			return new Direct3D.VertexElement(0, (short)(loc - size), declType, declMethod, declUsage, 0);
		}

		private Direct3D.PrimitiveType GetPrimitiveType(ref int vertexCount)
		{
			switch (this.PrimitiveType)
			{
				case PrimitiveType.TriangleFan:
					vertexCount = vertexCount - 2;
					return SlimDX.Direct3D9.PrimitiveType.TriangleFan;
				case PrimitiveType.TriangleList:
					vertexCount /= 3;
					return SlimDX.Direct3D9.PrimitiveType.TriangleList;
				case PrimitiveType.TriangleStrip:
					vertexCount = vertexCount - 2;
					return SlimDX.Direct3D9.PrimitiveType.TriangleStrip;
				default:
					throw new NotImplementedException(this.PrimitiveType.ToString() + " not implemented.");
			}
		}

		public override void Draw(int start, int count)
		{
			int primitiveCount = count;

			// after calling GetPrimitiveType, primitiveCount is the number of primitives
			// instead of the number of vertices.
			Direct3D.PrimitiveType primType = GetPrimitiveType(ref primitiveCount);

			SetTextures();


			mDisplay.D3D_Device.Device.SetStreamSource(0, mBuffer, 0, VertexLayout.VertexSize);
			mDisplay.D3D_Device.Device.VertexDeclaration = mDeclaration;
			mDisplay.D3D_Device.Device.DrawPrimitives(primType, start, primitiveCount);
		}
		public override void DrawIndexed(IndexBuffer _indexbuffer, int start, int count)
		{
			int primitiveCount = count;
			SDX_IndexBuffer indexbuffer = _indexbuffer.Impl as SDX_IndexBuffer;

			// after calling GetPrimitiveType, primitiveCount is the number of primitives
			// instead of the number of vertices.
			Direct3D.PrimitiveType primType = GetPrimitiveType(ref primitiveCount);

			SetTextures();

			mDisplay.D3D_Device.AlphaArgument1 = Direct3D.TextureArgument.Texture;
			
			mDisplay.D3D_Device.Device.VertexDeclaration = mDeclaration;
			mDisplay.D3D_Device.Device.Indices = indexbuffer.DeviceIndexBuffer;
			mDisplay.D3D_Device.Device.SetStreamSource(0, mBuffer, 0, mLayout.VertexSize);
			mDisplay.D3D_Device.Device.DrawIndexedPrimitives(
				primType, 0, 0, indexbuffer.MaxIndex, start, primitiveCount);
		}
		
		private void SetTextures()
		{
			for (int i = 0; i < Textures.Count; i++)
			{
				if (Textures[i] == null)
					mDisplay.D3D_Device.SetDeviceStateTexture(null, i);
				else
				{
					SDX_Surface surf = (SDX_Surface)Textures[i].Impl;

					mDisplay.D3D_Device.SetDeviceStateTexture(surf.D3dTexture, i);
				}
			}
		}

		public override int VertexCount
		{
			get { return mCount; }
		}

		public override void Write<T>(T[] vertices) 
		{
			var stream = mBuffer.Lock(0, 0, 0);
			stream.WriteRange(vertices);
			mBuffer.Unlock();
		}
	}
}
