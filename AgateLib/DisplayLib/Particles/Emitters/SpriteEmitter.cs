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

using System.Collections.Generic;
using AgateLib.DisplayLib.Particles.Particles;
using AgateLib.DisplayLib.Sprites;
using AgateLib.Mathematics;

namespace AgateLib.DisplayLib.Particles.Emitters
{
	/// <summary>
	/// Emitter class optimized for simulating sprite particles.
	/// </summary>
	public class SpriteEmitter : ParticleEmitter
	{
		private List<Sprite> mSprites = new List<Sprite>();

		private int mEmitSpriteKey;
		private double mEmitAlpha = 1d;

		private float time;

		/// <value>
		/// Gets or sets the alpha channel of emitting <see cref="SpriteParticle"/>s.
		/// </value>
		public double EmitAlpha
		{
			get { return mEmitAlpha; }
			set { mEmitAlpha = value; }
		}

		/// <value>
		/// Gets or sets the SpriteKey of emitting <see cref="SpriteParticle"/>s.
		/// </value>
		public int EmitSpriteKey
		{
			get { return mEmitSpriteKey; }
			set { mEmitSpriteKey = value; }
		}

		/// <summary>
		/// Constructs a SpriteEmitter with default values:
		/// EmitLife = 1f, maxParticle = 1000, emitSpriteKey = 0
		/// </summary>
		/// <param name="position"></param>
		public SpriteEmitter(Vector2f position) : this(position, 1f, 1000, 0) { }

		/// <summary>
		/// Constructs a SpriteEmitter with default values:
		/// maxParticle = 1000, emitSpriteKey = 0
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		public SpriteEmitter(Vector2f position, float emitLife) : this(position, emitLife, 1000, 0) { }

		/// <summary>
		/// Constructs a SpriteEmitter with default values:
		/// emitSpriteKey = 0
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		/// <param name="maxParticle"></param>
		public SpriteEmitter(Vector2f position, float emitLife, int maxParticle) : this(position, emitLife, maxParticle, 0) { }

		/// <summary>
		/// Constructs a SpriteEmitter.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		/// <param name="maxParticle"></param>
		/// <param name="emitSpriteKey"></param>
		public SpriteEmitter(Vector2f position, float emitLife, int maxParticle, int emitSpriteKey)
		{
			base.Position = position;
			base.EmitLife = emitLife;
			base.Particles = new List<Particle>(maxParticle);
			mEmitSpriteKey = emitSpriteKey;
		}

		/// <summary>
		/// Gets a stored <see cref="Sprite"/> by key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public Sprite GetSpriteByKey(int key)
		{
			return mSprites[key];
		}

		/// <summary>
		/// Adds a <see cref="Sprite"/> to the storage.
		/// </summary>
		/// <param name="sprite"></param>
		/// <returns>Returns the key related to the stored <see cref="Sprite"/>.</returns>
		public int AddSprite(Sprite sprite)
		{
			int index = mSprites.Count;
			mSprites.Insert(index, sprite);
			return index;
		}

		/// <summary>s
		/// Overridden Draw method.
		/// Draws each living particle.
		/// </summary>
		public override void Draw()
		{
			foreach (SpriteParticle sp in base.Particles)
			{
				if (sp.Condition == ParticleState.Alive || sp.Condition == ParticleState.Frozen)
				{
					mSprites[sp.SpriteKey].CurrentFrameIndex = sp.Frame;
					mSprites[sp.SpriteKey].Alpha = sp.Alpha;
					mSprites[sp.SpriteKey].Draw(sp.Position);
				}
			}
		}

		/// <summary>
		/// Overridden Update mehtod.
		/// Emits particles based on the frequency property.
		/// Updates the emitter position.
		/// Calls particle manipulators to manipulate particles.
		/// Updates the animation of each <see cref="Sprite"/>.
		/// </summary>
		/// <param name="time_ms">Time in milliseconds.</param>
		public override void Update(double time_ms)
		{
			time += (float)time_ms;
			float frequency = EmitFrequency * 1000;

			while (time >= frequency)
			{
				//int index = Particles.IndexOf(Particles.FirstOrDefault(pt => pt.IsAlive == false));
				int index = -1;
				for (int i = 0; i < Particles.Count; i++)
				{
					if (Particles[i].IsAlive == false)
					{
						index = i;
						break;
					}
				}

				if (index > -1)
				{
					// Recycle a dead particle
					Particles[index].Acceleration = EmitAcceleration;
					(Particles[index] as SpriteParticle).SpriteKey = mEmitSpriteKey;
					(Particles[index] as SpriteParticle).Alpha = mEmitAlpha;
					(Particles[index] as SpriteParticle).Frame = 0;
					(Particles[index] as SpriteParticle).FrameTime = 0d;
					Particles[index].Condition = ParticleState.Alive;
					Particles[index].Life = EmitLife;
					Particles[index].Position = Position;
					Particles[index].Velocity = EmitVelocity;
					NewRecyledParticle(Particles[index]);
				}
				else if (Particles.Count < Particles.Capacity)
				{
					// Add a new particle
					SpriteParticle sp = new SpriteParticle();
					sp.Acceleration = EmitAcceleration;
					sp.SpriteKey = mEmitSpriteKey;
					sp.Alpha = mEmitAlpha;
					sp.Frame = 0;
					sp.FrameTime = 0d;
					sp.Condition = ParticleState.Alive;
					sp.Life = EmitLife;
					sp.Position = Position;
					sp.Velocity = EmitVelocity;
					NewParticle(sp);
					Particles.Add(sp);
				}
				else
				{
					// No capacity left and no dead particles to recycle
					time = 0;
					break;
				}
				time -= frequency;
			}

			// Update animation
			foreach (SpriteParticle sp in Particles)
			{
				if (mSprites[sp.SpriteKey].Frames.Count <= 1)
				{
					sp.SpriteKey = 0;
					continue;
				}
				sp.FrameTime += time_ms / 1000;
				while (sp.FrameTime >= mSprites[sp.SpriteKey].TimePerFrame)
				{
					sp.FrameTime -= mSprites[sp.SpriteKey].TimePerFrame;
					if (mSprites[sp.SpriteKey].PlayReverse)
					{
						sp.Frame--;
						if (sp.Frame < 0)
							sp.Frame = mSprites[sp.SpriteKey].Frames.Count;
					}
					else
					{
						sp.Frame++;
						if (sp.Frame >= mSprites[sp.SpriteKey].Frames.Count)
							sp.Frame = 0;
					}
					// TODO: Handle AnimationType
				}
			}

			// updates own position, particle positions and calls manipulators
			base.Update(time_ms);
		}
	}
}
