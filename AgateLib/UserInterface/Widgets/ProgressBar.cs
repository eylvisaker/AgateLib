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
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class ProgressBar : Widget
	{
		public int Value { get; set; }
		public int Max { get; set; }
		public Gradient Gradient { get; set; }

		public ProgressBar()
		{
			Gradient = new Gradient(Color.White);
		}
		internal override Size ComputeSize(int? maxWidth, int? maxHeight)
		{
			Size result = new Size();

			result.Width = 40;

			return result;
		}
		public override void DrawImpl(Rectangle screenRect)
		{
			Rectangle destRect = screenRect;

			if (Max > 0)
			{
				double percentage = Value / (double)Max;

				int maxBarWidth = Width;
				int width = (int)(percentage * maxBarWidth);

				destRect.Width = width;

				//var grad = new Gradient(Gradient.TopLeft);
				//grad.TopRight = Gradient.Interpolate(width, 0);
				//grad.BottomRight = grad.TopRight;

				Display.Primitives.FillRect(Gradient.TopLeft, destRect);
			}
		}
	}
}
