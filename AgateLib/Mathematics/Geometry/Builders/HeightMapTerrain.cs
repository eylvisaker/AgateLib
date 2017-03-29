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
	/// Constructs a terrain from a pixel buffer.
	/// </summary>
	public class HeightMapTerrain
	{
		PixelBuffer pixels;

		/// <summary>
		/// Construts a height map from a pixel buffer.
		/// </summary>
		/// <param name="pixels"></param>
		public HeightMapTerrain(PixelBuffer pixels)
		{
			this.pixels = pixels.Clone();
			Width = 1;
			Height = 1;
			MaxPeak = 1;

			VertexType = PositionTextureNormal.VertexLayout;
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

		/// <summary>
		/// Type of vertex used.
		/// </summary>
		public VertexLayout VertexType { get; set; }

		/// <summary>
		/// Create the vertex buffer.
		/// </summary>
		/// <returns></returns>
		public VertexBuffer CreateVertexBuffer()
		{
			Vector3[] vertices = new Vector3[pixels.Width * pixels.Height];
			Vector3[] normal = new Vector3[pixels.Width * pixels.Height];
			Vector2[] texture = new Vector2[pixels.Width * pixels.Height];

			short[] indices = new short[(pixels.Width - 1) * (pixels.Height - 1) * 6];

			FillVertices(vertices, normal, texture, indices);

			VertexBuffer result = new VertexBuffer(VertexType, vertices.Length);

			result.WriteVertexData(vertices);
			//result.WriteTextureCoords(texture);
			//result.WriteNormalData(normal);
			//result.WriteIndices(indices);

			return result;
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
