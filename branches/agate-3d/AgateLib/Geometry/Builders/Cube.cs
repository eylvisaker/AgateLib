using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;

namespace AgateLib.Geometry.Builders
{
	public class CubeBuilder
	{
		public CubeBuilder()
		{
			Length = 1;
			VertexType = VertexLayout.PositionNormalTexture;
		}

		public float Length { get; set; }
		public Vector3 Location { get; set; }
		bool GenerateTextureCoords
		{
			get { return VertexType.Contains(VertexMemberUsage.Texture); }
		}
		bool GenerateNormals
		{
			get { return VertexType.Contains(VertexMemberUsage.Normal); }
		}
		bool GenerateTangent
		{
			get { return VertexType.Contains(VertexMemberUsage.Tangent); }
		}
		bool GenerateBitangent
		{
			get { return VertexType.Contains(VertexMemberUsage.Bitangent); }
		}


		public VertexLayout VertexType { get; set; }

		public VertexBuffer CreateVertexBuffer()
		{
			VertexBuffer retval = new VertexBuffer(VertexType, 24);

			retval.PrimitiveType = PrimitiveType.TriangleList;
			retval.WriteVertexData(GetVertexData());
			retval.WriteIndices(GetIndexData());

			if (GenerateTextureCoords)
				retval.WriteTextureCoords(GetTextureCoords());
			if (GenerateNormals)
				retval.WriteNormalData(GetNormals());
			if (GenerateTangent)
				retval.WriteAttributeData("tangent", GetTangent());
			if (GenerateBitangent)
				retval.WriteAttributeData("bitangent", GetBitangent());

			return retval;
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

		private Vector3[] GetBitangent()
		{
			Vector3[] retval = new Vector3[24];

			int i = 0;

			for (int sign = -1; sign >= 1; sign += 2)
			{
				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);

				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);

				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);
			}

			return retval;
		}
		private Vector3[] GetTangent()
		{
			Vector3[] retval = new Vector3[24];

			int i = 0;

			for (int sign = -1; sign <= 1; sign += 2)
				{
				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);

				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);

				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);

			}

			return retval;
		}
		private Vector3[] GetNormals()
		{
			Vector3[] retval = new Vector3[24];

			int i = 0;
			float length = Length / 2.0f;

			for (int sign = -1; sign <= 1; sign += 2)
			{
				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);
				retval[i++] = new Vector3(0, 0, sign);

				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);
				retval[i++] = new Vector3(0, sign, 0);

				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);
				retval[i++] = new Vector3(sign, 0, 0);
			}

			return retval;
		}
		protected virtual Vector3[] GetVertexData()
		{
			Vector3[] retval = new Vector3[24];

			int i = 0;
			float length = Length / 2.0f;

			for (int sign = -1; sign <= 1; sign += 2)
			{
				retval[i++] = new Vector3(length, length, sign * length);
				retval[i++] = new Vector3(length, -length, sign * length);
				retval[i++] = new Vector3(-length, length, sign * length);
				retval[i++] = new Vector3(-length, -length, sign * length);

				retval[i++] = new Vector3(length, sign * length, length);
				retval[i++] = new Vector3(length, sign * length, -length);
				retval[i++] = new Vector3(-length, sign * length, length);
				retval[i++] = new Vector3(-length, sign * length, -length);

				retval[i++] = new Vector3(sign * length, length, length);
				retval[i++] = new Vector3(sign * length, length, -length);
				retval[i++] = new Vector3(sign * length, -length, length);
				retval[i++] = new Vector3(sign * length, -length, -length);
			}

			for (i = 0; i < retval.Length; i++)
				retval[i] += Location;

			return retval;
		}
		protected virtual Vector2[] GetTextureCoords()
		{
			Vector2[] retval = new Vector2[24];

			int i = 0;
			for (int face = 0; face < 6; face++)
			{
				retval[i++] = new Vector2(0, 0);
				retval[i++] = new Vector2(0, 1);
				retval[i++] = new Vector2(1, 0);
				retval[i++] = new Vector2(1, 1);
			}

			return retval;
		}
	}
}