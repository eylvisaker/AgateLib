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

using AgateLib.DisplayLib.Particles.Emitters;

namespace AgateLib.DisplayLib.Particles.Particles
{
	/// <summary>
	/// A particle implementation for <see cref="AgateLib.Sprites.Sprite"/>s.
	/// </summary>
	public class SpriteParticle : Particle
	{
		private int mSpriteKey;
		private double mAlpha = 1d;
		private int mFrame = 0;
		private double mFrameTime = 0d;
		
		/// <value>
		/// Gets or sets the sprite key.
		/// </value>
		public int SpriteKey
		{
			get { return mSpriteKey; }
			set { mSpriteKey = value; }
		}
		
		/// <value>
		/// Gets or sets the alpha of the surface.
		/// </value>
		public double Alpha
		{
			get { return mAlpha; }
			set { mAlpha = value; }
		}
		
		/// <summary>
		/// Gets or sets the current frame of the sprite.
		/// </summary>
		public int Frame
		{
			get { return mFrame; }
			set { mFrame = value; }
		}
		
		internal double FrameTime
		{
			get { return mFrameTime; }
			set { mFrameTime = value; }
		}
		
		/// <summary>
		/// Constructs a SpriteParticle with the default value 0 for the spriteKey.
		/// </summary>
		public SpriteParticle() : this(0) {}
		
		/// <summary>
		/// Constructs a SpriteParticle.
		/// </summary>
		/// <param name="spriteKey">Key of a sprite in a <see cref="SpriteEmitter"/>.</param>
		public SpriteParticle(int spriteKey)
		{
			mSpriteKey = spriteKey;
		}
		
		/// <summary>
		/// Constructs a SpriteParticle.
		/// </summary>
		/// <param name="spriteKey">Key of a sprite in a <see cref="SpriteEmitter"/></param>
		/// <param name="alpha">Alpha channel.</param>
		public SpriteParticle(int spriteKey, double alpha) : this(spriteKey)
		{
			mAlpha = alpha;
		}
		
		/// <summary>
		/// Constructs a SpriteParticle.
		/// </summary>
		/// <param name="spriteKey">Key of a sprite in a <see cref="SpriteEmitter"/></param>
		/// <param name="alpha">Alpha channel.</param>
		/// <param name="frame">Frame of the sprite.</param>
		public SpriteParticle(int spriteKey, double alpha, int frame) : this(spriteKey, alpha)
		{
			mFrame = frame;
		}
	}
}
