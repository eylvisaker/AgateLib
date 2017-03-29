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
using AgateLib.Platform;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{
	public interface IWidgetAnimator
	{
		IList<IWidgetAnimator> Children { get; }
		Rectangle ClientRect { get; set; }
		Gesture Gesture { get; set; }
		bool InTransition { get; }
		bool IsDead { get; set; }
		IWidgetAnimator Parent { get; set; }
		WidgetStyle Style { get; }
		bool Visible { get; set; }
		Widget Widget { get; }
		Rectangle WidgetRect { get; }
		IWidgetAnimator ParentCoordinateSystem { get; }

		void Update(ClockTimeSpan elapsed);
		Rectangle ClientToScreen(Rectangle rectangle, bool translateForScroll = true);
		Point ClientToScreen(Point translated, bool translateForScroll = true);
		Point ScreenToClient(Point translated);

		int X { get; set; }
		int Y { get; set; }
		int Width { get; set; }
		int Height { get; set; }
	}
}
