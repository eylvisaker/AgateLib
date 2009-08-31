﻿using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.Geometry.Builders
{
	public class CubeBuilder
	{
		VertexBuffer mVertices = null;
		IndexBuffer mIndices = null;

		public CubeBuilder()
		{
			Length = 1;
			VertexType = VertexTypes.PositionTextureNTB.VertexLayout;
		}

		public VertexBuffer VertexBuffer
		{
			get { return mVertices; }
		}
		public IndexBuffer IndexBuffer
		{
			get { return mIndices; }
		}
		
		public float Length { get; set; }
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

		public VertexLayout VertexType { get; set; }

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