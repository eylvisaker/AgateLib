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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	public class Resolution : IResolution
	{
		public Resolution(Size size, IRenderMode mode = null)
		{
			Size = size;
			RenderMode = mode ?? DisplayLib.RenderMode.RetainAspectRatio;
		}

		public Resolution(int width, int height, IRenderMode mode = null)
		{
			Size = new Size(width, height);
			RenderMode = mode;
		}

		public IResolution Clone(Size? newSize = null)
		{
			return new Resolution(newSize ?? Size, RenderMode);
		}

		public Size Size
		{
			get { return new Size(Width, Height); }
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		public int Width { get; set; }
		public int Height { get; set; }

		public IRenderMode RenderMode { get; set; }

		public override string ToString()
		{
			return $"{Size.Width} x {Size.Height} - {RenderMode?.ToString() ?? "None"}";
		}

	}

	public interface IResolution
	{
		Size Size { get; }

		int Width { get; }

		int Height { get; }

		IRenderMode RenderMode { get; }

		IResolution Clone(Size? newSize = null);
	}
}
