using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Algorithms.ThreeD
{
    public static class MapConversion
    {
        #region --- Normal Map conversion routines ---

        /// <summary>
        /// Creates a normal map by interpreting the pixel data in the passed buffer
        /// as a height map.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static PixelBuffer NormalMapFromHeightMap(PixelBuffer buffer)
        {
            return NormalMapFromHeightMap(buffer, 1.0f);
        }
        /// <summary>
        /// Creates a normal map by interpreting the pixel data in the passed buffer
        /// as a height map.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="normalStr">The weight for the x-y components of the normals in
        /// the normal map.  Smaller values make the normal map more subtle.
        /// Values in the range 0.5f to 3.0f are reasonable.</param>
        /// <returns></returns>
        public static PixelBuffer NormalMapFromHeightMap(PixelBuffer buffer, float normalStr)
        {
            int[,] heights = new int[buffer.Width, buffer.Height];

            for (int j = 0; j < buffer.Height; j++)
            {
                for (int i = 0; i < buffer.Width; i++)
                {
                    heights[i, j] = (int)(buffer.GetPixel(i, j).Intensity * short.MaxValue);
                }
            }

            PixelBuffer retval = new PixelBuffer(buffer.PixelFormat, buffer.Size);
            int[,] square = new int[3, 3];

            for (int j = 0; j < retval.Height; j++)
            {
                for (int i = 0; i < retval.Width; i++)
                {
                    GetSquare(square, heights, i, j);

                    // sobel operator:
                    int diffx = square[0, 0] - square[2, 0] +
                           2 * (square[0, 1] - square[2, 1]) +
                                square[0, 2] - square[2, 0];

                    int diffy = square[0, 0] + 2 * square[1, 0] + square[0, 2] +
                               -square[0, 2] - 2 * square[1, 2] - square[2, 2];

                    Vector3 vec = new Vector3(diffx / (float)short.MaxValue, diffy / (float)short.MaxValue, 0);
                    vec *= normalStr;
                    vec.Z = 1;

                    vec = vec.Normalize();
                    vec = vec / 2;
                    vec.X += 0.5f; vec.Y += 0.5f; vec.Z += 0.5f;
                    Color clr = Color.FromArgb((int)(vec.X * 255),
                                       (int)(vec.Y * 255),
                                       (int)(vec.Z * 255));

                    retval.SetPixel(i, j, clr);
                }
            }

            return retval;
        }

        private static void GetSquare(int[,] square, int[,] heights, int x, int y)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    int val = GetValueWrap(heights, x + i, y + j);

                    square[i + 1, j + 1] = val;
                }
            }
        }

        private static int GetValueWrap(int[,] heights, int x, int y)
        {
            if (x < 0) x += heights.GetUpperBound(0) + 1;
            if (y < 0) y += heights.GetUpperBound(1) + 1;
            if (x > heights.GetUpperBound(0)) x -= heights.GetUpperBound(0) + 1;
            if (y > heights.GetUpperBound(1)) y -= heights.GetUpperBound(1) + 1;

            return heights[x, y];
        }

        #endregion

    }
}
