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

using AgateLib.Geometry;
using AgateLib.DisplayLib;

namespace AgateLib.Particles
{
	/// <summary>
	/// A surface emitter.
	/// Optimized for emitting and rendering surface based particles.
	/// </summary>
	public class SurfaceEmitter : ParticleEmitter
	{
		private List<Surface> mSurfaces = new List<Surface>();
		
		private int mEmitSurfaceKey;
		
		private float time;
		
		private double mEmitAlpha = 1d;
		
		private SizeF mEmitScale = new SizeF(1f, 1f);
		
		/// <value>
		/// Gets or sets the scale width of emitting particles.
		/// </value>
		public float EmitScaleWidth
		{
			get { return mEmitScale.Width; }
			set { mEmitScale.Width = value; }
		}
		
		/// <value>
		/// Gets or sets the scale height of emitting particles.
		/// </value>
		public float EmitScaleHeight
		{
			get { return mEmitScale.Height; }
			set { mEmitScale.Height = value; }
		}
		
		/// <value>
		/// Gets or sets the scale of emitting particles.
		/// </value>
		public SizeF EmitScale
		{
			get { return mEmitScale; }
			set { mEmitScale = value; }
		}
				
		/// <value>
		/// Gets or sets the alpha channel of emitting particles.
		/// </value>
		public double EmitAlpha
		{
			get { return EmitAlpha; }
			set { mEmitAlpha = value; }
		}
		
		/// <value>
		/// Gets or sets the SurfaceKey of the emitting particles.
		/// </value>
		public int EmitSurfaceKey
		{
			get { return mEmitSurfaceKey; }
			set { mEmitSurfaceKey = value; }
		}
		
		/// <summary>
		/// Constructs a SurfaceEmitter with default values:
		/// EmitLife = 1f, maxParticle = 1000, emitSurfaceKey = 0
		/// </summary>
		/// <param name="position"></param>
		public SurfaceEmitter(Vector2 position) : this(position, 1f, 1000, 0) {}
		
		/// <summary>
		/// Constructs a SurfaceEmitter with default values:
		/// maxParticle = 1000, emitSurfaceKey = 0
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		public SurfaceEmitter(Vector2 position, float emitLife) : this(position, emitLife, 1000, 0) {}
		
		/// <summary>
		/// Constructs a SurfaceEmitter with default values:
		/// emitSurfaceKey = 0
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		/// <param name="maxParticle"></param>
		public SurfaceEmitter(Vector2 position, float emitLife, int maxParticle) : this(position, emitLife, maxParticle, 0) {}
		
		/// <summary>
		/// Constructs a SurfaceEmitter.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		/// <param name="maxParticle"></param>
		/// <param name="emitSurfaceKey"></param>
		public SurfaceEmitter(Vector2 position, float emitLife, int maxParticle, int emitSurfaceKey)
		{
			base.Position = position;
			base.EmitLife = emitLife;
			base.Particles = new List<Particle>(maxParticle);
			mEmitSurfaceKey = emitSurfaceKey;
		}
		
		/// <summary>
		/// Gets surface by key from storage.
		/// </summary>
		/// <param name="key">Surface key</param>
		/// <returns></returns>
		public Surface GetSurfaceByKey(int key)
		{
			return mSurfaces[key];
		}
		
		/// <summary>
		/// Adds a surface to the storage.
		/// </summary>
		/// <param name="image"></param>
		/// <returns>Returns the key where the surface is stored.</returns>
		public int AddSurface(Surface image)
		{
			int index = mSurfaces.Count;
			mSurfaces.Insert(index, image);
			return index;
		}
		
		/// <summary>
		/// Overridden Update mehtod.
		/// Emits particles based on the frequency property.
		/// Updates the emitter position.
		/// Calls particle manipulators to manipulate particles.
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
					(Particles[index] as SurfaceParticle).SurfaceKey = mEmitSurfaceKey;
					(Particles[index] as SurfaceParticle).Alpha = mEmitAlpha;
					(Particles[index] as SurfaceParticle).Scale = mEmitScale;
					Particles[index].Condition = ParticleState.Alive;
					Particles[index].Life = EmitLife;
					Particles[index].Position = Position;
					Particles[index].Velocity = EmitVelocity;
					NewRecyledParticle(Particles[index]);
				}
				else if(Particles.Count < Particles.Capacity)
				{
					// Add a new particle
					SurfaceParticle sp = new SurfaceParticle();
					sp.Acceleration = EmitAcceleration;
					sp.SurfaceKey = mEmitSurfaceKey;
					sp.Alpha = mEmitAlpha;
					sp.Scale = mEmitScale;
					sp.Condition = ParticleState.Alive;
					sp.Life = EmitLife;
					sp.Position = Position;
					sp.Velocity = EmitVelocity;
					NewRecyledParticle(sp);
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
			
			// updates own position, particle positions and calls manipulators
			base.Update (time_ms);
		}
		
		/// <summary>s
		/// Overridden Draw method.
		/// Draws each living particle.
		/// </summary>
		public override void Draw ()
		{
			foreach(SurfaceParticle sp in Particles)
			{
				if(sp.Condition == ParticleState.Alive || sp.Condition == ParticleState.Frozen)
				{
					mSurfaces[sp.SurfaceKey].ScaleHeight = sp.ScaleHeight;
					mSurfaces[sp.SurfaceKey].ScaleWidth = sp.ScaleWidth;
					mSurfaces[sp.SurfaceKey].Alpha = sp.Alpha;
					mSurfaces[sp.SurfaceKey].Draw(sp.Position);
				}
			}
		}	
	}
}
