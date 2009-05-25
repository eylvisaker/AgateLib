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
		
		private float mEmitScaleWidth = 1f;
		private float mEmitScaleHeight = 1f;
		
		/// <value>
		/// Gets or sets the scale width of emitting particles.
		/// </value>
		public float EmitScaleWidth
		{
			get { return mEmitScaleWidth; }
			set { mEmitScaleWidth = value; }
		}
		
		/// <value>
		/// Gets or sets the scale height of emitting particles.
		/// </value>
		public float EmitScaleHeight
		{
			get { return mEmitScaleHeight; }
			set { mEmitScaleHeight = value; }
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
		/// Emits particles based on the frequenzy property.
		/// Updates the emitter position.
		/// Calls particle manipulators to manipulate particles.
		/// </summary>
		/// <param name="time_ms">Time in milliseconds.</param>
		public override void Update (double time_ms)
		{
			time += (float)time_ms;
			float frequenzy = EmitFrequenzy*1000;
			
			while(time >= frequenzy)
			{
				int index = Particles.IndexOf(Particles.FirstOrDefault(pt => pt.IsALive == false));
				if(index > -1)
				{
					// Recycle a dead particle
					Particles[index].Acceleration = EmitAcceleration;
					(Particles[index] as SurfaceParticle).SurfaceKey = mEmitSurfaceKey;
					(Particles[index] as SurfaceParticle).Alpha = mEmitAlpha;
					(Particles[index] as SurfaceParticle).ScaleHeight = mEmitScaleHeight;
					(Particles[index] as SurfaceParticle).ScaleWidth = mEmitScaleWidth;
					Particles[index].Condition = Condition.ALive;
					Particles[index].Life = EmitLife;
					Particles[index].Position = Position;
					Particles[index].Velocity = EmitVelocity;
				}
				else if(Particles.Count < Particles.Capacity)
				{
					// Add a new particle
					SurfaceParticle sp = new SurfaceParticle();
					sp.Acceleration = EmitAcceleration;
					sp.SurfaceKey = mEmitSurfaceKey;
					sp.Alpha = mEmitAlpha;
					sp.ScaleHeight = mEmitScaleHeight;
					sp.ScaleWidth = EmitScaleWidth;
					sp.Condition = Condition.ALive;
					sp.Life = EmitLife;
					sp.Position = Position;
					sp.Velocity = EmitVelocity;
					Particles.Add(sp);
				}
				else
				{
					// No capacity left and no dead particles to recycle
					time = 0;
					break;
				}
				
				time -= frequenzy;
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
				if(sp.Condition == Condition.ALive || sp.Condition == Condition.Frozen)
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
