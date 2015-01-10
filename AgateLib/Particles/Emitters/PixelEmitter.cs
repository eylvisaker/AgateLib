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

namespace AgateLib.Particles
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
		public PixelEmitter(Vector2 position, Color color)
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
		public PixelEmitter(Vector2 position, Color color, float emitLife)
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
		public PixelEmitter(Vector2 position, Color color, int maxParticles)
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
		public PixelEmitter(Vector2 position, Color color, int maxParticles, float emitLife)
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
