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
using System.Linq;
using System.Collections.Generic;

using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Sprites;

namespace AgateLib.Particles
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
		public SpriteEmitter(Vector2 position) : this(position, 1f, 1000, 0) {}
		
		/// <summary>
		/// Constructs a SpriteEmitter with default values:
		/// maxParticle = 1000, emitSpriteKey = 0
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		public SpriteEmitter(Vector2 position, float emitLife) : this(position, emitLife, 1000, 0) {}
		
		/// <summary>
		/// Constructs a SpriteEmitter with default values:
		/// emitSpriteKey = 0
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		/// <param name="maxParticle"></param>
		public SpriteEmitter(Vector2 position, float emitLife, int maxParticle) : this(position, emitLife, maxParticle, 0) {}
		
		/// <summary>
		/// Constructs a SpriteEmitter.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		/// <param name="maxParticle"></param>
		/// <param name="emitSpriteKey"></param>
		public SpriteEmitter(Vector2 position, float emitLife, int maxParticle, int emitSpriteKey)
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
		public override void Draw ()
		{
			if(base.Particles.Count <= 0)
				return;
			
			foreach(Sprite s in mSprites)
			{
				s.AdvanceFrame();
			}
			
			foreach(SpriteParticle sp in base.Particles)
			{
				if(sp.Condition == Condition.Alive || sp.Condition == Condition.Frozen)
				{
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
		public override void Update (double time_ms)
		{
			time += (float)time_ms;
			float frequency = EmitFrequency*1000;
			
			while(time >= frequency)
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
				
				if(index > -1)
				{
					// Recycle a dead particle
					Particles[index].Acceleration = EmitAcceleration;
					(Particles[index] as SpriteParticle).SpriteKey = mEmitSpriteKey;
					(Particles[index] as SpriteParticle).Alpha = mEmitAlpha;
					Particles[index].Condition = Condition.Alive;
					Particles[index].Life = EmitLife;
					Particles[index].Position = Position;
					Particles[index].Velocity = EmitVelocity;
					NewRecyledParticle(Particles[index]);
				}
				else if(Particles.Count < Particles.Capacity)
				{
					// Add a new particle
					SpriteParticle sp = new SpriteParticle();
					sp.Acceleration = EmitAcceleration;
					sp.SpriteKey = mEmitSpriteKey;
					sp.Alpha = mEmitAlpha;
					sp.Condition = Condition.Alive;
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
			foreach(Sprite sp in mSprites)
			{
				sp.Update(time_ms);
			}
			
			// updates own position, particle positions and calls manipulators
			base.Update (time_ms);
		}
	}
}
