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
using AgateLib.UserInterface.StyleModel;

namespace AgateLib.UserInterface.Layout.MetricsCalculators
{
	public class DefaultMetricsCalculator : IWidgetMetricsCalculator
	{
		public bool ComputeBoxSize(WidgetStyle widget, int? maxWidth, int? maxHeight)
		{
			var newContentSize = widget.Widget.ComputeSize(maxWidth - widget.BoxModel.Width, maxHeight - widget.BoxModel.Height);
			var newBoxSize = new Size(
				newContentSize.Width + widget.BoxModel.Width,
				newContentSize.Height + widget.BoxModel.Height);

			if (newBoxSize.Width > maxWidth)
				newBoxSize.Width = maxWidth.Value;
			if (newBoxSize.Height > maxHeight)
				newBoxSize.Height = maxHeight.Value;

			if (newBoxSize == widget.Metrics.BoxSize)
				return false;

			widget.Metrics.BoxSize = newBoxSize;

			return true;
		}

		public bool ComputeNaturalSize(WidgetStyle style)
		{
			style.Metrics.MinTotalSize = new Size(
				style.WidgetLayout.MinWidth ?? 0 + style.BoxModel.Margin.Left + style.BoxModel.Border.Right,
				style.WidgetLayout.MinHeight ?? 0 + style.BoxModel.Margin.Top + style.BoxModel.Border.Bottom);
			style.Metrics.MaxTotalSize = new Size(
				style.WidgetLayout.MaxWidth ?? int.MaxValue,
				style.WidgetLayout.MaxHeight ?? int.MaxValue);

			var size = style.Widget.ComputeSize(null, null);
			size.Width += style.BoxModel.Width;
			size.Height += style.BoxModel.Height;

			if (style.Metrics.NaturalBoxSize == size)
				return false;

			style.Metrics.NaturalBoxSize = size;

			return true;
		}
	}
}
