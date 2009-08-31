using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using AgateLib.ImplementationBase;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace AgateMDX
{
	class MDX1_VertexBuffer : VertexBufferImpl
	{
		MDX1_Display mDisplay;
		Direct3D.VertexBuffer mBuffer;
		Direct3D.VertexDeclaration mDeclaration;
		Direct3D.VertexFormats mFormats;
		int mCount;
		object data;
		VertexLayout mLayout;

		static StringBuilder b;

		public MDX1_VertexBuffer(MDX1_Display display, VertexLayout layout, int vertexCount)
		{
			mDisplay = display;
			mCount = vertexCount;

			b = new StringBuilder();

			mDeclaration = CreateVertexDeclaration(layout);
			mFormats = CreateVertexFormats(layout);

			System.Diagnostics.Debug.WriteLine(b.ToString());

			mLayout = layout;

			mBuffer = new Microsoft.DirectX.Direct3D.VertexBuffer(
				mDisplay.D3D_Device.Device,
				vertexCount * layout.VertexSize,
				Microsoft.DirectX.Direct3D.Usage.WriteOnly,
				mFormats,
				Direct3D.Pool.Managed);
		}

		public override VertexLayout VertexLayout
		{
			get { return mLayout; }
		}
		private Direct3D.VertexFormats CreateVertexFormats(VertexLayout layout)
		{
			Direct3D.VertexFormats retval = Microsoft.DirectX.Direct3D.VertexFormats.None;

			foreach (var element in layout)
			{
				switch (element.ElementType)
				{
					case VertexElement.Position:
						retval |= Microsoft.DirectX.Direct3D.VertexFormats.Position;
						break;

					case VertexElement.Texture:
						retval |= Microsoft.DirectX.Direct3D.VertexFormats.Texture0;
						break;

					case VertexElement.Texture1:
						retval |= Microsoft.DirectX.Direct3D.VertexFormats.Texture1;
						break;

					case VertexElement.Texture2:
						retval |= Microsoft.DirectX.Direct3D.VertexFormats.Texture2;
						break;

					case VertexElement.Texture3:
						retval |= Microsoft.DirectX.Direct3D.VertexFormats.Texture3;
						break;

					case VertexElement.DiffuseColor:
						retval |= Microsoft.DirectX.Direct3D.VertexFormats.Diffuse;
						break;
				}
			}

			return retval;
		}
		private Direct3D.VertexDeclaration CreateVertexDeclaration(VertexLayout layout)
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

			return new Direct3D.VertexDeclaration(
				mDisplay.D3D_Device.Device, formats.ToArray());
		}
		private Direct3D.VertexElement ConvertElement(VertexElementDesc element, ref short loc)
		{
			Direct3D.DeclarationMethod declMethod = Microsoft.DirectX.Direct3D.DeclarationMethod.Default;
			Direct3D.DeclarationUsage declUsage;
			Direct3D.DeclarationType declType;

			int size = VertexLayout.SizeOf(element.DataType);

			switch(element.DataType)
			{
				case VertexElementDataType.Float1: 
					declType = Microsoft.DirectX.Direct3D.DeclarationType.Float1;
					break;
				case VertexElementDataType.Float2:
					declType = Microsoft.DirectX.Direct3D.DeclarationType.Float2;
					break;
				case VertexElementDataType.Float3:
					declType = Microsoft.DirectX.Direct3D.DeclarationType.Float3;
					break;
				case VertexElementDataType.Float4:
					declType = Microsoft.DirectX.Direct3D.DeclarationType.Float4;
					break;
				default:
					throw new NotImplementedException(
						element.DataType.ToString() + " not implemented.");
			}

			switch(element.ElementType)
			{
				case VertexElement.Position:
					declUsage = Microsoft.DirectX.Direct3D.DeclarationUsage.Position;
					break;
				case VertexElement.Texture:
					declUsage = Microsoft.DirectX.Direct3D.DeclarationUsage.TextureCoordinate;
					break;
				case VertexElement.Normal:
					declUsage = Microsoft.DirectX.Direct3D.DeclarationUsage.Normal;
					break;
				case VertexElement.Tangent:
					declUsage = Microsoft.DirectX.Direct3D.DeclarationUsage.Tangent;
					break;
				case VertexElement.DiffuseColor:
					declUsage = Microsoft.DirectX.Direct3D.DeclarationUsage.Color;
					break;
				case VertexElement.Bitangent:
					declUsage = Microsoft.DirectX.Direct3D.DeclarationUsage.BiNormal;
					break;
				default:
					throw new NotImplementedException(
						element.ElementType.ToString() + " not implemented.");
			}

			b.AppendFormat("{0} {1} {2} {3}\n", declType, declUsage, loc, size);

			loc += (short)size;

			return new Direct3D.VertexElement(0, (short)(loc - size), declType, declMethod, declUsage, 0);
		}

		private Direct3D.PrimitiveType GetPrimitiveType(ref int vertexCount)
		{
			switch (this.PrimitiveType)
			{
				case PrimitiveType.TriangleFan:
					vertexCount = vertexCount - 2;
					return Microsoft.DirectX.Direct3D.PrimitiveType.TriangleFan;
				case PrimitiveType.TriangleList:
					vertexCount /= 3;
					return Microsoft.DirectX.Direct3D.PrimitiveType.TriangleList;
				case PrimitiveType.TriangleStrip:
					vertexCount = vertexCount - 2;
					return Microsoft.DirectX.Direct3D.PrimitiveType.TriangleStrip;
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


			mDisplay.D3D_Device.Device.SetStreamSource(0, mBuffer, 0);
			mDisplay.D3D_Device.Device.VertexDeclaration = mDeclaration;
			mDisplay.D3D_Device.Device.DrawPrimitives(primType, start, primitiveCount);
		}
		public override void DrawIndexed(IndexBuffer _indexbuffer, int start, int count)
		{
			int primitiveCount = count;
			MDX1_IndexBuffer indexbuffer = _indexbuffer.Impl as MDX1_IndexBuffer;

			// after calling GetPrimitiveType, primitiveCount is the number of primitives
			// instead of the number of vertices.
			Direct3D.PrimitiveType primType = GetPrimitiveType(ref primitiveCount);

			SetTextures();

			mDisplay.D3D_Device.AlphaArgument1 = Direct3D.TextureArgument.TextureColor;

			mDisplay.D3D_Device.VertexFormat = Microsoft.DirectX.Direct3D.VertexFormats.None;
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
					MDX1_Surface surf = (MDX1_Surface)Textures[i].Impl;

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
			mBuffer.SetData(vertices, 0, 0);
			data = vertices;
		}
	}
}
