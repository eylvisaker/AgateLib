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
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry.VertexTypes;

namespace AgateLib.Mathematics.Geometry.Builders
{
	/// <summary>
	/// Constructs a cube.
	/// </summary>
	public class CubeBuilder
	{
		VertexBuffer mVertices = null;
		IndexBuffer mIndices = null;

		/// <summary>
		/// Constructs a cube builder.
		/// </summary>
		public CubeBuilder()
		{
			Length = 1;
			VertexType = VertexTypes.PositionTextureNTB.VertexLayout;
		}

		/// <summary>
		/// Gets the created vertex buffer.
		/// </summary>
		public VertexBuffer VertexBuffer
		{
			get { return mVertices; }
		}
		/// <summary>
		/// Gets the created index buffer.
		/// </summary>
		public IndexBuffer IndexBuffer
		{
			get { return mIndices; }
		}
		
		/// <summary>
		/// Length of the cube.
		/// </summary>
		public float Length { get; set; }

		/// <summary>
		/// Position of the center of the cube.
		/// </summary>
		public Vector3 Location { get; set; }

		bool GenerateTextureCoords
		{
			get { return VertexType.ContainsElement(VertexElement.Texture); }
		}
		bool GenerateNormals
		{
			get { return VertexType.ContainsElement(VertexElement.Normal); }
		}
		bool GenerateTangent
		{
			get { return VertexType.ContainsElement(VertexElement.Tangent); }
		}
		bool GenerateBitangent
		{
			get { return VertexType.ContainsElement(VertexElement.Bitangent); }
		}

		/// <summary>
		/// Vertex type to use when generating the cube.
		/// </summary>
		public VertexLayout VertexType { get; set; }
		/// <summary>
		/// Creates the vertex buffer and index buffer using the settings of the CubeBuilder.
		/// </summary>
		public void CreateVertexBuffer()
		{
			PositionTextureNTB[] vertices = new PositionTextureNTB[24];

			mVertices = new VertexBuffer(VertexType, 24);
			mIndices = new IndexBuffer(IndexBufferType.Int16, 36);

			mVertices.PrimitiveType = PrimitiveType.TriangleList;

			GenerateVertexData(vertices);
			GenerateNormalData(vertices);
			GenerateTexCoordData(vertices);
			GenerateTangentData(vertices);
			GenerateBitangentData(vertices);

			mVertices.WriteVertexData(vertices);
			mIndices.WriteIndices(GetIndexData());

		}


		private short[] GetIndexData()
		{
			short[] result = new short[36];

			int i = 0;
			short index = 0;
			for (int face = 0; face < 6; face++)
			{
				result[i++] = index;
				result[i++] = (short)(index + 1);
				result[i++] = (short)(index + 2);
				result[i++] = (short)(index + 1);
				result[i++] = (short)(index + 2);
				result[i++] = (short)(index + 3);

				index += 4;
			}

			return result;
		}

		private void GenerateBitangentData(PositionTextureNTB[] vertices)
		{
			Vector3[] result = new Vector3[24];

			int i = 0;

			for (int sign = -1; sign >= 1; sign += 2)
			{
				vertices[i++].Bitangent = new Vector3f(0, sign, 0);
				vertices[i++].Bitangent = new Vector3f(0, sign, 0);
				vertices[i++].Bitangent = new Vector3f(0, sign, 0);
				vertices[i++].Bitangent = new Vector3f(0, sign, 0);

				vertices[i++].Bitangent = new Vector3f(sign, 0, 0);
				vertices[i++].Bitangent = new Vector3f(sign, 0, 0);
				vertices[i++].Bitangent = new Vector3f(sign, 0, 0);
				vertices[i++].Bitangent = new Vector3f(sign, 0, 0);

				vertices[i++].Bitangent = new Vector3f(0, 0, sign);
				vertices[i++].Bitangent = new Vector3f(0, 0, sign);
				vertices[i++].Bitangent = new Vector3f(0, 0, sign);
				vertices[i++].Bitangent = new Vector3f(0, 0, sign);
			}
		}

		private void GenerateTangentData(PositionTextureNTB[] vertices)
		{
			int i = 0;

			for (int sign = -1; sign <= 1; sign += 2)
			{
				vertices[i++].Tangent = new Vector3f(sign, 0, 0);
				vertices[i++].Tangent = new Vector3f(sign, 0, 0);
				vertices[i++].Tangent = new Vector3f(sign, 0, 0);
				vertices[i++].Tangent = new Vector3f(sign, 0, 0);

				vertices[i++].Tangent = new Vector3f(0, 0, sign);
				vertices[i++].Tangent = new Vector3f(0, 0, sign);
				vertices[i++].Tangent = new Vector3f(0, 0, sign);
				vertices[i++].Tangent = new Vector3f(0, 0, sign);

				vertices[i++].Tangent = new Vector3f(0, sign, 0);
				vertices[i++].Tangent = new Vector3f(0, sign, 0);
				vertices[i++].Tangent = new Vector3f(0, sign, 0);
				vertices[i++].Tangent = new Vector3f(0, sign, 0);

			}
		}
		private void GenerateNormalData(PositionTextureNTB[] vertices)
		{
			int i = 0;
			float length = Length / 2.0f;

			for (int sign = -1; sign <= 1; sign += 2)
			{
				vertices[i++].Normal = new Vector3f(0, 0, sign);
				vertices[i++].Normal = new Vector3f(0, 0, sign);
				vertices[i++].Normal = new Vector3f(0, 0, sign);
				vertices[i++].Normal = new Vector3f(0, 0, sign);

				vertices[i++].Normal = new Vector3f(0, sign, 0);
				vertices[i++].Normal = new Vector3f(0, sign, 0);
				vertices[i++].Normal = new Vector3f(0, sign, 0);
				vertices[i++].Normal = new Vector3f(0, sign, 0);

				vertices[i++].Normal = new Vector3f(sign, 0, 0);
				vertices[i++].Normal = new Vector3f(sign, 0, 0);
				vertices[i++].Normal = new Vector3f(sign, 0, 0);
				vertices[i++].Normal = new Vector3f(sign, 0, 0);
			}
		}
		private void GenerateVertexData(PositionTextureNTB[] vertices)
		{
			int i = 0;
			float length = Length / 2.0f;

			for (int sign = -1; sign <= 1; sign += 2)
			{
				vertices[i++].Position = new Vector3f(length, length, sign * length);
				vertices[i++].Position = new Vector3f(length, -length, sign * length);
				vertices[i++].Position = new Vector3f(-length, length, sign * length);
				vertices[i++].Position = new Vector3f(-length, -length, sign * length);

				vertices[i++].Position = new Vector3f(length, sign * length, length);
				vertices[i++].Position = new Vector3f(length, sign * length, -length);
				vertices[i++].Position = new Vector3f(-length, sign * length, length);
				vertices[i++].Position = new Vector3f(-length, sign * length, -length);

				vertices[i++].Position = new Vector3f(sign * length, length, length);
				vertices[i++].Position = new Vector3f(sign * length, length, -length);
				vertices[i++].Position = new Vector3f(sign * length, -length, length);
				vertices[i++].Position = new Vector3f(sign * length, -length, -length);
			}

			var location = (Vector3f) Location;
			for (i = 0; i < vertices.Length; i++)
				vertices[i].Position += location;
		}
		private void GenerateTexCoordData(PositionTextureNTB[] vertices)
		{
			int i = 0;
			for (int face = 0; face < 6; face++)
			{
				vertices[i++].Texture = new Vector2f(0, 0);
				vertices[i++].Texture = new Vector2f(0, 1);
				vertices[i++].Texture = new Vector2f(1, 0);
				vertices[i++].Texture = new Vector2f(1, 1);
			}
		}
	}
}