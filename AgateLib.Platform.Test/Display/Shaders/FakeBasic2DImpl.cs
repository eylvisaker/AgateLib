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
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.Test.Display.Shaders
{
	public class FakeBasic2DImpl : Basic2DImpl
	{
		public override void SetVariable(string name, params float[] v)
		{
			
		}

		public override void SetVariable(string name, params int[] v)
		{
			
		}

		public override void SetVariable(string name, Matrix4x4 matrix)
		{
			
		}

		public override void SetVariable(string name, Color color)
		{
			
		}

		public override int Passes { get; } = 1;

		public override void Begin()
		{
			
		}

		public override void BeginPass(int passIndex)
		{
			
		}

		public override void EndPass()
		{
			
		}

		public override void End()
		{
			
		}

		public override Rectangle CoordinateSystem { get; set; }
	}
}