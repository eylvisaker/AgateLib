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
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.Particles.Emitters
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
		public SurfaceEmitter(Vector2f position) : this(position, 1f, 1000, 0) {}
		
		/// <summary>
		/// Constructs a SurfaceEmitter with default values:
		/// maxParticle = 1000, emitSurfaceKey = 0
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		public SurfaceEmitter(Vector2f position, float emitLife) : this(position, emitLife, 1000, 0) {}
		
		/// <summary>
		/// Constructs a SurfaceEmitter with default values:
		/// emitSurfaceKey = 0
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		/// <param name="maxParticle"></param>
		public SurfaceEmitter(Vector2f position, float emitLife, int maxParticle) : this(position, emitLife, maxParticle, 0) {}
		
		/// <summary>
		/// Constructs a SurfaceEmitter.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="emitLife"></param>
		/// <param name="maxParticle"></param>
		/// <param name="emitSurfaceKey"></param>
		public SurfaceEmitter(Vector2f position, float emitLife, int maxParticle, int emitSurfaceKey)
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
