using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace AgateMDX
{
	class MDX1_VertexBuffer : VertexBufferImpl
	{
		MDX1_Display mDisplay;

		public MDX1_VertexBuffer(MDX1_Display display)
		{
			mDisplay = display;
		}


		public override void WriteNormalData(Vector3[] data)
		{
			throw new NotImplementedException();
		}
		public override void WriteTextureCoords(Vector2[] texCoords)
		{
			throw new NotImplementedException();
		}
		public override void WriteVertexData(Vector3[] data)
		{
			throw new NotImplementedException();
		}
		public override void WriteIndices(short[] indices)
		{
			throw new NotImplementedException();
		}

		public override void Draw(int vertexStart, int vertexCount)
		{
			throw new NotImplementedException();
		}


		public override int IndexCount
		{
			get { return 0; }
		}

		public override int VertexCount
		{
			get { return 0; }
		}


		public override void WriteAttributeData(string attributeName, Vector3[] data)
		{
			throw new NotImplementedException();
		}
	}
}
