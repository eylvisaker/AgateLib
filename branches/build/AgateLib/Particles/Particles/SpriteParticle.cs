//		The contents of this file are subject to the Mozilla Public License
//		Version 1.1 (the "License"); you may not use this file except in
//		compliance with the License. You may obtain a copy of the License at
//		http://www.mozilla.org/MPL/
//		
//		Software distributed under the License is distributed on an "AS IS"
//		basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//		License for the specific language governing rights and limitations
//		under the License.
//		
//		The Original Code is AgateLib.Particles.
//		
//		The Initial Developer of the Original Code is Marcel Hauf
//		Portions created by Marcel Hauf are Copyright (C) 2009.
//		All Rights Reserved.
//		
//		Contributor(s): Marcel Hauf.
//
using System;

namespace AgateLib.Particles
{
	/// <summary>
	/// A particle implementation for <see cref="Sprite"/>s.
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
