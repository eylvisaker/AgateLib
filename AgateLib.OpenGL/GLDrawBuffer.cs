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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.VertexTypes;

using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL
{
	public abstract class GLDrawBuffer
	{
		public abstract void ResetTexture();
		public abstract void SetInterpolationMode(InterpolationMode mode);

		public abstract void AddQuad(int textureID, Color color, TextureCoordinates texCoord, RectangleF destRect);
		public abstract void AddQuad(int textureID, Color color, TextureCoordinates texCoord, PointF[] pts);
		public abstract void AddQuad(int textureID, Gradient color, TextureCoordinates texCoord, PointF[] pts);

		public abstract void Flush();
	}
}
