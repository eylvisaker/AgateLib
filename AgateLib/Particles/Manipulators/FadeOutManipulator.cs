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

using AgateLib.Geometry;

namespace AgateLib.Particles
{
	/// <summary>
	/// Fades out particles by changing the alpha channel.
	/// Works with <see cref="PixelEmitter"/>, <see cref="SurfaceEmitter"/> and <see cref="SpriteEmitter"/>.
	/// </summary>
	public class FadeOutManipulator
	{
		private float mLifeBarrier = 1f;
		private float mAlphaAmount = 0.5f;
		
		private int fadeout = 0;
		
		private bool mRecyleInvisible = true;
		
		/// <value>
		/// Gets or sets if invisible particles should be recyled.
		/// </value>
		public bool RecyleInvisible
		{
			get { return mRecyleInvisible; }
			set { mRecyleInvisible = value; }
		}
		
		/// <value>
		/// Gets or sets the life barrier at which the particle should fade out.
		/// </value>
		public float LifeBarrier
		{
			get { return mLifeBarrier; }
			set { mLifeBarrier = value; }
		}
		
		/// <value>
		/// Gets or sets the amount of alpha which will be reduced.
		/// </value>
		public float AlphaAmount
		{
			get { return mAlphaAmount; }
			set { mAlphaAmount = value; }
		}
		
		/// <summary>
		/// Constructs a FadeOutManipulator.
		/// </summary>
		/// <param name="lifeBarrier">The barrier at which the particles will be faded out</param>
		/// <param name="alphaAmount">The amount of alpha wich will be reduced.</param>
		public FadeOutManipulator(float lifeBarrier, float alphaAmount)
		{
			mLifeBarrier = lifeBarrier;
			mAlphaAmount = alphaAmount;
		}
		
		void HandleOnUpdate(UpdateArgs args)
		{
			Type emitterType = args.Emitter.GetType();
			double time = args.Time_ms/1000;
			
			if(emitterType == typeof(PixelEmitter))
			{
				foreach(PixelParticle pt in args.Emitter.Particles)
				{
					if(pt.Condition == ParticleState.Alive && pt.Life <= LifeBarrier)
					{
						fadeout = pt.Color.A - (int)(255 * mAlphaAmount * time);
						pt.Color = Color.FromArgb(fadeout, pt.Color);
						if(mRecyleInvisible == true && pt.Color.A == 0)
							pt.Condition = ParticleState.Dead;
					}
				}
			}
			else if (emitterType == typeof(SurfaceEmitter))
			{
				foreach(SurfaceParticle sp in args.Emitter.Particles)
				{
					if(sp.Condition == ParticleState.Alive && sp.Life <= LifeBarrier)
					{
						sp.Alpha -= mAlphaAmount * time;
						if(mRecyleInvisible == true && sp.Alpha == 0)
							sp.Condition = ParticleState.Dead;
					}
				}
			}
			else if (emitterType == typeof(SpriteEmitter))
			{
				foreach(SpriteParticle sp in args.Emitter.Particles)
				{
					if(sp.Condition == ParticleState.Alive && sp.Life <= LifeBarrier)
					{
						sp.Alpha -= mAlphaAmount * time;
						if(mRecyleInvisible == true && sp.Alpha == 0)
							sp.Condition = ParticleState.Dead;
					}
				}
			}
		}
		
		/// <summary>
		/// Subscribe to a particle emitter.
		/// </summary>
		/// <param name="emitter"></param>
		public void SubscribeToEmitter (ParticleEmitter emitter)
		{
			emitter.OnUpdate += HandleOnUpdate; 
		}
		
		/// <summary>
		/// Unsubscribe to a particle emitter.
		/// </summary>
		/// <param name="emitter"></param>
		public void UnSubscribeToEmitter (ParticleEmitter emitter)
		{
			emitter.OnUpdate -= HandleOnUpdate;
		}
	}
}
