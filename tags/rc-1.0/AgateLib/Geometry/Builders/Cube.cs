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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.Geometry.Builders
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
			short[] retval = new short[36];

			int i = 0;
			short index = 0;
			for (int face = 0; face < 6; face++)
			{
				retval[i++] = index;
				retval[i++] = (short)(index + 1);
				retval[i++] = (short)(index + 2);
				retval[i++] = (short)(index + 1);
				retval[i++] = (short)(index + 2);
				retval[i++] = (short)(index + 3);

				index += 4;
			}

			return retval;
		}

		private void GenerateBitangentData(PositionTextureNTB[] vertices)
		{
			Vector3[] retval = new Vector3[24];

			int i = 0;

			for (int sign = -1; sign >= 1; sign += 2)
			{
				vertices[i++].Bitangent = new Vector3(0, sign, 0);
				vertices[i++].Bitangent = new Vector3(0, sign, 0);
				vertices[i++].Bitangent = new Vector3(0, sign, 0);
				vertices[i++].Bitangent = new Vector3(0, sign, 0);

				vertices[i++].Bitangent = new Vector3(sign, 0, 0);
				vertices[i++].Bitangent = new Vector3(sign, 0, 0);
				vertices[i++].Bitangent = new Vector3(sign, 0, 0);
				vertices[i++].Bitangent = new Vector3(sign, 0, 0);

				vertices[i++].Bitangent = new Vector3(0, 0, sign);
				vertices[i++].Bitangent = new Vector3(0, 0, sign);
				vertices[i++].Bitangent = new Vector3(0, 0, sign);
				vertices[i++].Bitangent = new Vector3(0, 0, sign);
			}
		}
		private void GenerateTangentData(PositionTextureNTB[] vertices)
		{
			int i = 0;

			for (int sign = -1; sign <= 1; sign += 2)
			{
				vertices[i++].Tangent = new Vector3(sign, 0, 0);
				vertices[i++].Tangent = new Vector3(sign, 0, 0);
				vertices[i++].Tangent = new Vector3(sign, 0, 0);
				vertices[i++].Tangent = new Vector3(sign, 0, 0);

				vertices[i++].Tangent = new Vector3(0, 0, sign);
				vertices[i++].Tangent = new Vector3(0, 0, sign);
				vertices[i++].Tangent = new Vector3(0, 0, sign);
				vertices[i++].Tangent = new Vector3(0, 0, sign);

				vertices[i++].Tangent = new Vector3(0, sign, 0);
				vertices[i++].Tangent = new Vector3(0, sign, 0);
				vertices[i++].Tangent = new Vector3(0, sign, 0);
				vertices[i++].Tangent = new Vector3(0, sign, 0);

			}
		}
		private void GenerateNormalData(PositionTextureNTB[] vertices)
		{
			int i = 0;
			float length = Length / 2.0f;

			for (int sign = -1; sign <= 1; sign += 2)
			{
				vertices[i++].Normal = new Vector3(0, 0, sign);
				vertices[i++].Normal = new Vector3(0, 0, sign);
				vertices[i++].Normal = new Vector3(0, 0, sign);
				vertices[i++].Normal = new Vector3(0, 0, sign);

				vertices[i++].Normal = new Vector3(0, sign, 0);
				vertices[i++].Normal = new Vector3(0, sign, 0);
				vertices[i++].Normal = new Vector3(0, sign, 0);
				vertices[i++].Normal = new Vector3(0, sign, 0);

				vertices[i++].Normal = new Vector3(sign, 0, 0);
				vertices[i++].Normal = new Vector3(sign, 0, 0);
				vertices[i++].Normal = new Vector3(sign, 0, 0);
				vertices[i++].Normal = new Vector3(sign, 0, 0);
			}
		}
		private void GenerateVertexData(PositionTextureNTB[] vertices)
		{
			int i = 0;
			float length = Length / 2.0f;

			for (int sign = -1; sign <= 1; sign += 2)
			{
				vertices[i++].Position = new Vector3(length, length, sign * length);
				vertices[i++].Position = new Vector3(length, -length, sign * length);
				vertices[i++].Position = new Vector3(-length, length, sign * length);
				vertices[i++].Position = new Vector3(-length, -length, sign * length);

				vertices[i++].Position = new Vector3(length, sign * length, length);
				vertices[i++].Position = new Vector3(length, sign * length, -length);
				vertices[i++].Position = new Vector3(-length, sign * length, length);
				vertices[i++].Position = new Vector3(-length, sign * length, -length);

				vertices[i++].Position = new Vector3(sign * length, length, length);
				vertices[i++].Position = new Vector3(sign * length, length, -length);
				vertices[i++].Position = new Vector3(sign * length, -length, length);
				vertices[i++].Position = new Vector3(sign * length, -length, -length);
			}

			for (i = 0; i < vertices.Length; i++)
				vertices[i].Position += Location;
		}
		private void GenerateTexCoordData(PositionTextureNTB[] vertices)
		{
			int i = 0;
			for (int face = 0; face < 6; face++)
			{
				vertices[i++].Texture = new Vector2(0, 0);
				vertices[i++].Texture = new Vector2(0, 1);
				vertices[i++].Texture = new Vector2(1, 0);
				vertices[i++].Texture = new Vector2(1, 1);
			}
		}
	}
}