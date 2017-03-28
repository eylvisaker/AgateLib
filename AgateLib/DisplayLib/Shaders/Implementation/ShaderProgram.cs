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

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	/*
	public abstract class ShaderProgram
	{
		public abstract PixelShader PixelShader { get; }
		public abstract VertexShader VertexShader { get; }

		public abstract void SetUniform(string name, params float[] v);
		public abstract void SetUniform(string name, params int[] v);
		public abstract void SetUniform(string name, Matrix4 matrix);

		public void SetUniform(string name, Vector2 v)
		{
			SetUniform(name, v.X, v.Y);
		}
		public void SetUniform(string name, Vector3 v)
		{
			SetUniform(name, v.X, v.Y, v.Z);
		}
		public void SetUniform(string name, Vector4 v)
		{
			SetUniform(name, v.X, v.Y, v.Z, v.W);
		}
		public void SetUniform(string name, Color color)
		{
			SetUniform(name, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		public VertexLayout VertexDefinition { get; set; }

		public abstract void Render(RenderHandler handler, object obj);
	}

	public delegate void RenderHandler(object obj);
	 * */
}
