using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.OpenGL
{
    /// <summary>
    /// Structure to contain source texture coordinates for drawing quads.
    /// </summary>
    public struct TextureCoordinates
    {
        public TextureCoordinates(float left, float top, float right, float bottom)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }
        public float Top;
        public float Bottom;
        public float Left;
        public float Right;
    }
}
