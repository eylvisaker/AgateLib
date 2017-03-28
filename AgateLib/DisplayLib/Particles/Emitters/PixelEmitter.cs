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
	/// A pixel particle emitter.
	/// Optimized for pixel rendering.
	/// </summary>
	public class PixelEmitter : ParticleEmitter
	{
		private Color mEmitColor = Color.White;

		private float time = 0f;

		private Surface mSurface;

		/// <value>
		/// Gets or sets the emit color.
		/// </value>
		public Color EmitColor
		{
			get { return mEmitColor; }
			set { mEmitColor = value; }
		}

		/// <value>
		/// Gets or sets the pixel size.
		/// </value>
		public Size PixelSize
		{
			get { return mSurface.DisplaySize; }
			set { mSurface = new Surface(FillSurface(value, Color.White)); }
		}

		/// <summary>
		/// Constructs a pixel particle emitter.
		/// </summary>
		/// <param name="color">Emit color.</param>
		/// <param name="position">Position of the emitter.</param>
		public PixelEmitter(Vector2f position, Color color)
		{
			Position = position;
			mEmitColor = color;
			mSurface = new Surface(FillSurface(new Size(1, 1), Color.White));
		}

		private static PixelBuffer FillSurface(Size size, Color color)
		{
			PixelBuffer pb = new PixelBuffer(PixelFormat.ARGB8888, size);
			for (int i = 0; i < pb.Width; i++)
			{
				for (int ii = 0; ii < pb.Height; ii++)
				{
					pb.SetPixel(i, ii, color);
				}
			}
			return pb;
		}

		/// <summary>
		/// Constructs a pixel particle emitter.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="color"></param>
		/// <param name="emitLife"></param>
		public PixelEmitter(Vector2f position, Color color, float emitLife)
			: this(position, color)
		{
			base.EmitLife = emitLife;
		}

		/// <summary>
		/// Constructs a pixel particle emitter.
		/// </summary>
		/// <param name="position">Position of the emitter.</param>
		/// <param name="color">Emit color.</param>
		/// <param name="maxParticles">Maximum amount of particles.</param>
		public PixelEmitter(Vector2f position, Color color, int maxParticles)
			: this(position, color)
		{
			Particles = new List<Particle>(maxParticles);
		}

		/// <summary>
		/// Constructs a pixel particle emitter.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="color"></param>
		/// <param name="maxParticles"></param>
		/// <param name="emitLife"></param>
		public PixelEmitter(Vector2f position, Color color, int maxParticles, float emitLife)
			: this(position, color, maxParticles)
		{
			base.EmitLife = emitLife;
		}

		/// <summary>s
		/// Overridden Draw method.
		/// Draws each living particle.
		/// </summary>
		public override void Draw()
		{
			foreach (PixelParticle ptl in Particles)
			{
				if (ptl.Condition == ParticleState.Alive || ptl.Condition == ParticleState.Frozen)
				{
					mSurface.Color = ptl.Color;
					mSurface.Draw(ptl.Position);
				}
			}
		}

		/// <summary>
		/// Overridden Update mehtod.
		/// Emits particles based on the frequency property.
		/// Updates the emitter position.
		/// Calls particle manipulators to manipulate particles.
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
					(Particles[index] as PixelParticle).Color = EmitColor;
					Particles[index].Condition = ParticleState.Alive;
					Particles[index].Life = EmitLife;
					Particles[index].Position = Position;
					Particles[index].Velocity = EmitVelocity;
					NewRecyledParticle(Particles[index]);
				}
				else if (Particles.Count < Particles.Capacity)
				{
					// Add a new particle
					PixelParticle pp = new PixelParticle(EmitColor);
					pp.Acceleration = EmitVelocity;
					pp.Color = EmitColor;
					pp.Condition = ParticleState.Alive;
					pp.Life = EmitLife;
					pp.Position = Position;
					pp.Velocity = EmitVelocity;
					NewParticle(pp);
					Particles.Add(pp);
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
			base.Update(time_ms);
		}
	}
}
