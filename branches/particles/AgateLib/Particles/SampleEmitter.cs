
using System;
using System.Collections.Generic;

using AgateLib.Geometry;

namespace AgateLib.Particles
{
	/// <summary>
	/// A particle emitter which emits copies of sample particles.
	/// </summary>
	public class SampleEmitter : ParticleEmitter
	{
		private int lastParticle = 0;
		private Random ran;
		
		private List<Particle> mSampleParticles;
		
		private EmitOrder mEmitOrder = EmitOrder.None;
		
		/// <value>
		/// Gets or sets the sample particles.
		/// </value>
		public List<Particle> SampleParticles
		{
			get { return mSampleParticles; }
			set { mSampleParticles = value; }
		}
		
		/// <value>
		/// Gets or sets the emit order.
		/// </value>
		public EmitOrder EmitOrder
		{
			get { return mEmitOrder; }
			set { mEmitOrder = value; }
		}
		
		/// <summary>
		/// Constructs a SampleEmitter with a sequenz emitting order
		/// and a default frequenzy.
		/// </summary>
		/// <param name="position"></param>
		public SampleEmitter(Vector2 position)
		{
			mSampleParticles = new List<Particle>();
			mEmitOrder = EmitOrder.Sequenz;
			base.Position = position;		
			ran = new Random();
		}
		
		/// <summary>
		/// Overridden Update method.
		/// Emits new sample particle copies.
		/// Updates and manipulates each particle.
		/// </summary>
		/// <param name="time_ms">Passed time in milliseconds since last update.</param>
		public override void Update (float time_ms)
		{
			if(mSampleParticles.Count == 0)
			{
				base.Update(time_ms);
				return;
			}
			
			// TODO: add emit frequenzy
			switch(mEmitOrder)
			{
			case EmitOrder.None:
				break;
				
			case EmitOrder.Sequenz:
				base.Particles.Add(mSampleParticles[lastParticle]);
				if(lastParticle == mSampleParticles.Count - 1)
				{
					lastParticle = 0;
				}
				else
				{
					lastParticle++;
				}
				break;
				
			case EmitOrder.Random:
				base.Particles.Add(mSampleParticles[ran.Next(0, mSampleParticles.Count-1)]);
				break;
			}			
			base.Update (time_ms);
		}
	}
	
	/// <summary>
	/// Emit order.
	/// </summary>
	public enum EmitOrder
	{
		None,
		Sequenz,
		Random
	}
}
