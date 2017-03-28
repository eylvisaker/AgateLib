//
//    Copyright (c) 2006-2017 Erik Ylvisaker, Marcel Hauf
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

using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.Particles.Particles
{
	/// <summary>
	/// A surface particle class.
	/// </summary>
	public class SurfaceParticle : Particle
	{
		private int mSurfaceKey;
		
		private double mAlpha = 1d;
		
		private SizeF mScale = new SizeF(1f, 1f);
		
		/// <value>
		/// Gets or sets the surface key.
		/// </value>
		public int SurfaceKey
		{
			get { return mSurfaceKey; }
			set { mSurfaceKey = value; }
		}
		
		/// <value>
		/// Gets or sets the alpha of the surface.
		/// </value>
		public double Alpha
		{
			get { return mAlpha; }
			set { mAlpha = value; }
		}
		
		/// <value>
		/// Gets or sets the scale width of the surface.
		/// </value>
		public float ScaleWidth
		{
			get { return mScale.Width; }
			set { mScale.Width = value; }
		}
		
		/// <value>
		/// Gets or sets the scale height of the surface.
		/// </value>
		public float ScaleHeight
		{
			get { return mScale.Height; }
			set { mScale.Height = value; }
		}
		
		/// <value>
		/// Gets or sets the scale.
		/// </value>
		public SizeF Scale
		{
			get { return  mScale; }
			set { mScale = value; }
		}
		
		/// <summary>
		/// Constructs a SurfaceParticle with a default SurfaceKey=0.
		/// </summary>
		public SurfaceParticle() : this(0) {}
		
		/// <summary>
		/// Constructs a SurfaceParticle.
		/// </summary>
		/// <param name="surfaceKey"></param>
		public SurfaceParticle(int surfaceKey)
		{
			SurfaceKey = surfaceKey;
		}
	}
}
