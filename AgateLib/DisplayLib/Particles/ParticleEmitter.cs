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

using System;
using System.Collections.Generic;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;

namespace AgateLib.DisplayLib.Particles
{
	/// <summary>
	/// Base class for particle emitters.
	/// </summary>
	/// <example>This sample shows how to extend the <see cref="ParticleEmitter"/> class.
	/// <code>
	/// class TestClass : ParticleEmitter
	/// {
	///     public override void Draw ()
	/// 	{
	/// 		foreach(PixelParticle ptl in Particles)
	/// 		{
	/// 			Display.DrawEllipse(new Rectangle(0, 0, 2, 2), Color.White);
	/// 		}
	/// 	}
	/// }
	/// </code>
	/// </example>
	public abstract class ParticleEmitter : Particle
	{
		private List<Particle> mParticles = new List<Particle>();

		private float mEmitLife = 1f;

		// 1 = emit each second a particle
		// 2 = emit every two seconds
		private float mEmitFrequency = 1f;

		private Vector2f mEmitAcceleration = Vector2f.Zero;
		private Vector2f mEmitVelocity = Vector2f.Zero;

		/// <value>
		/// Gets or sets the particles.
		/// </value>
		public List<Particle> Particles
		{
			get { return mParticles; }
			set { mParticles = value; }
		}

		/// <value>
		/// Gets or sets the life of particles which will be emitted in future in seconds.
		/// </value>
		public float EmitLife
		{
			get { return mEmitLife; }
			set { mEmitLife = value; }
		}

		/// <value>
		/// Gets or sets the emit frequency in seconds.
		/// </value>
		public float EmitFrequency
		{
			get { return mEmitFrequency; }
			set { mEmitFrequency = value; }
		}

		/// <value>
		/// Gets or sets the emit acceleration.
		/// </value>
		public Vector2f EmitAcceleration
		{
			get { return mEmitAcceleration; }
			set { mEmitAcceleration = value; }
		}

		/// <value>
		/// Gets or sets the emit velocity.
		/// </value>
		public Vector2f EmitVelocity
		{
			get { return mEmitVelocity; }
			set { mEmitVelocity = value; }
		}

		/// <summary>
		/// Draws each particle.
		/// </summary>
		public abstract void Draw();

		/// <summary>
		/// Overridden update method.
		/// Updates and manipulates each particle.
		/// </summary>
		/// <param name="time_ms">
		/// Passed time in milliseconds since last update.
		/// </param>
		public override void Update(double time_ms)
		{
			if (OnUpdate != null)
				OnUpdate(new UpdateArgs(this, time_ms));

			foreach (Particle pt in Particles)
			{
				pt.Update(time_ms);
			}
			//Updates itself
			base.Update(time_ms);
		}

		/// <summary>
		/// Delegate type for handling particle events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public delegate void ParticleEventHandler(object sender, ParticleArgs args);
		/// <summary>
		/// Event raised when a new particle is created.
		/// </summary>
		public event ParticleEventHandler OnNewParticle;
		/// <summary>
		/// Event raised when a particle dies.
		/// </summary>
		public event ParticleEventHandler OnDeadParticle;
		/// <summary>
		/// Event raised when a particle is recycled.
		/// </summary>
		public event ParticleEventHandler OnRecyledParticle;

		/// <summary>
		/// Triggers a new <see cref="OnNewParticle"/> event.
		/// </summary>
		/// <param name="particle">
		/// A newly created particle.
		/// </param>
		protected void NewParticle(Particle particle)
		{
			if (OnNewParticle != null)
				OnNewParticle(this, new ParticleArgs(particle));
		}

		/// <summary>
		/// Triggers a new <see cref="OnDeadParticle"/> event.
		/// </summary>
		/// <param name="particle">
		/// Dead particle.
		/// </param>
		protected void NewDeadParticle(Particle particle)
		{
			if (OnDeadParticle != null)
				OnDeadParticle(this, new ParticleArgs(particle));
		}

		/// <summary>
		/// Triggers a new <see cref="OnRecyledParticle"/> event.
		/// </summary>
		/// <param name="particle">
		/// Recyled particle.
		/// </param>
		protected void NewRecyledParticle(Particle particle)
		{
			if (OnRecyledParticle != null)
				OnRecyledParticle(this, new ParticleArgs(particle));
		}

		/// <summary>
		/// Update event handler.
		/// </summary>
		public delegate void UpdateEventHandler(UpdateArgs args);
		/// <summary>
		/// The UpdateEvent is called then the ParticleEmitter begins to update.
		/// The event is called in <see cref="ParticleEmitter.Update"/>.
		/// </summary>
		public event UpdateEventHandler OnUpdate;

		/// <summary>
		/// Triggers a new <see cref="OnUpdate"/> event
		/// </summary>
		/// <param name="time_ms">
		/// Time in milliseconds.
		/// </param>
		protected void NewUpdate(double time_ms)
		{
			if (OnUpdate != null)
				OnUpdate(new UpdateArgs(this, time_ms));
		}
	}

	/// <summary>
	/// Particle event args.
	/// </summary>
	public class ParticleArgs : EventArgs
	{
		/// <summary>
		/// Constructs ParticleArgs.
		/// </summary>
		/// <param name="particle">Particle that changed condition.</param>
		public ParticleArgs(Particle particle)
		{
			mParticle = particle;
		}

		private Particle mParticle;
		/// <value>
		/// Particle that changed condition.
		/// </value>
		public Particle Particle
		{
			get { return mParticle; }
		}
	}

	/// <summary>
	/// Update event args.
	/// </summary>
	public class UpdateArgs : EventArgs
	{
		/// <summary>
		/// Constructs UpdateArgs.
		/// </summary>
		/// <param name="emitter">Emitter that triggered the update event.</param>
		/// <param name="time_ms">Passed time in milliseconds since last update.</param>
		public UpdateArgs(ParticleEmitter emitter, double time_ms)
		{
			mEmitter = emitter;
			mTime_ms = time_ms;
		}

		private ParticleEmitter mEmitter;
		/// <value>
		/// Emitter that triggered the update event.
		/// </value>
		public ParticleEmitter Emitter
		{
			get { return mEmitter; }
		}

		private double mTime_ms;
		/// <value>
		/// Passed time in milliseconds since last update.
		/// </value>
		public double Time_ms
		{
			get { return mTime_ms; }
		}
	}
}
