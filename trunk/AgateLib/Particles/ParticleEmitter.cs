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
using System.Collections.Generic;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Particles
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

		private Vector2 mEmitAcceleration = Vector2.Empty;
		private Vector2 mEmitVelocity = Vector2.Empty;

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
		public Vector2 EmitAcceleration
		{
			get { return mEmitAcceleration; }
			set { mEmitAcceleration = value; }
		}

		/// <value>
		/// Gets or sets the emit velocity.
		/// </value>
		public Vector2 EmitVelocity
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
