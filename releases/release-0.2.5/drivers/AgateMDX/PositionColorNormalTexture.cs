using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace ERY.AgateLib.MDX
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColorNormalTexture
    {
        public float X, Y, Z;
        public float nx, ny, nz;
        public int Color;
        public float Tu, Tv;

        public static VertexFormats Format =
            VertexFormats.PositionNormal | VertexFormats.Diffuse | VertexFormats.Texture1;

        public PositionColorNormalTexture(float x, float y, float z, int color, float tu, float tv,
            float nx, float ny, float nz)
        {
            X = x;
            Y = y;
            Z = z;
            Color = color;
            Tu = tu;
            Tv = tv;
            this.nx = nx;
            this.ny = ny;
            this.nz = nz;
        }

        public override string ToString()
        {
            return string.Format("X: {0} Y: {1} Z: {2} Color: {3} Tu: {4}, Tv: {5}",
                X, Y, Z, Color, Tu, Tv);
        }
    }
}
