using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;

namespace AgateLib.Geometry.Builders
{
	public class HeightMapTerrain
	{
		PixelBuffer pixels;

		public HeightMapTerrain(PixelBuffer pixels)
		{
			this.pixels = pixels.Clone();
			Width = 1;
			Height = 1;
			MaxPeak = 1;

			VertexType = VertexLayout.PositionNormalTexture;
		}

		/// <summary>
		/// The x-y extent of the height map.
		/// </summary>
		public SizeF Size { get; set; }
		/// <summary>
		/// The x extent of the height map.
		/// </summary>
		public float Width
		{
			get { return Size.Width; }
			set { Size = new SizeF(value, Height); }
		}
		/// <summary>
		/// The y extent of the height map.
		/// </summary>
		public float Height
		{
			get { return Size.Height; }
			set { Size = new SizeF(Width, value); }
		}

		/// <summary>
		/// The z extent of the height map
		/// </summary>
		public float MaxPeak { get; set; }

		public VertexLayout VertexType { get; set; }

		public VertexBuffer CreateVertexBuffer()
		{
			Vector3[] vertices = new Vector3[pixels.Width * pixels.Height];
			Vector3[] normal = new Vector3[pixels.Width * pixels.Height];
			Vector2[] texture = new Vector2[pixels.Width * pixels.Height];

			short[] indices = new short[(pixels.Width - 1) * (pixels.Height - 1) * 6];

			FillVertices(vertices, normal, texture, indices);

			VertexBuffer retval = new VertexBuffer(VertexType, vertices.Length);

			retval.WriteVertexData(vertices);
			retval.WriteTextureCoords(texture);
			retval.WriteNormalData(normal);
			retval.WriteIndices(indices);

			return retval;
		}

		private void FillVertices(Vector3[] vertices, Vector3[] normal, Vector2[] texture, short[] indices)
		{
			for (int j = 0; j < pixels.Height; j++)
			{
				for (int i = 0; i < pixels.Width; i++)
				{
					Color clr = pixels.GetPixel(i, j);
					double peak = clr.Intensity;

					vertices[i + j * pixels.Width] = new Vector3(
						i / (float)pixels.Width * Width,
						j / (float)pixels.Height * Height,
						peak * MaxPeak);

					texture[i + j * pixels.Width] = new Vector2(
						i / (float)pixels.Width,
						j / (float)pixels.Height);
				}
			}

			int index = 0;
			for (int j = 0; j < pixels.Height - 1; j++)
			{
				for (int i = 0; i < pixels.Width - 1; i++)
				{
					indices[index++] = (short)(i + j * pixels.Width);
					indices[index++] = (short)(i + 1 + j * pixels.Width);
					indices[index++] = (short)(i + 1 + (j + 1) * pixels.Width);
					indices[index++] = (short)(i + j * pixels.Width);
					indices[index++] = (short)(i + 1 + (j + 1) * pixels.Width);
					indices[index++] = (short)(i + (j + 1) * pixels.Width);

					Vector3 n1 = Vector3.CrossProduct(
						vertices[(i + 1) + j * pixels.Width] - vertices[i + j * pixels.Width],
						vertices[(i + 1) + (j + 1) * pixels.Width] - vertices[i + j * pixels.Width]);

					Vector3 n2 = Vector3.CrossProduct(
						vertices[i + (j + 1) * pixels.Width] - vertices[i + j * pixels.Width],
						vertices[(i + 1) + (j + 1) * pixels.Width] - vertices[i + j * pixels.Width]);

					normal[i + j * pixels.Width] += n1;
					normal[i + 1 + j * pixels.Width] += n1;
					normal[i + 1 + (j + 1) * pixels.Width] += n1;

					normal[i + j * pixels.Width] += n2;
					normal[i + (j + 1) * pixels.Width] += n2;
					normal[i + 1 + (j + 1) * pixels.Width] += n2;
				}
			}

			for (int i = 0; i < normal.Length; i++)
				normal[i] = normal[i].Normalize();

			for (int j = 1; j < pixels.Height - 1; j++)
			{
				for (int i = 1; i < pixels.Width - 1; i++)
				{
					normal[i + j * pixels.Width] = new Vector3(
						i / (float)pixels.Width * Width,
						j / (float)pixels.Height * Height,
						pixels.GetPixel(i, j).Intensity * MaxPeak).Normalize();

				}
			}
		}
	}
}
