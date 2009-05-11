using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using AgateLib.ImplementationBase;
using Direct3D = SlimDX.Direct3D9;

namespace AgateMDX
{
	class SDX_VertexBuffer : VertexBufferImpl
	{
		SDX_Display mDisplay;
		Direct3D.VertexBuffer mBuffer;
		Direct3D.VertexDeclaration mDeclaration;
		Direct3D.VertexFormat mFormats;
		int mCount;
		object data;
		VertexLayout mLayout;

		static StringBuilder b;

		public SDX_VertexBuffer(SDX_Display display, VertexLayout layout, int vertexCount)
		{
			mDisplay = display;
			mCount = vertexCount;

			b = new StringBuilder();

			mDeclaration = CreateVertexDeclaration(layout);
			mFormats = CreateVertexFormat(layout);

			System.Diagnostics.Debug.WriteLine(b.ToString());

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

					case VertexElement.Color:
						retval |= SlimDX.Direct3D9.VertexFormat.Diffuse;
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
				case VertexElement.Color:
					declUsage = SlimDX.Direct3D9.DeclarationUsage.Color;
					break;
				case VertexElement.Bitangent:
					declUsage = SlimDX.Direct3D9.DeclarationUsage.Binormal;
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

			mDisplay.D3D_Device.VertexFormat = SlimDX.Direct3D9.VertexFormat.None;
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
