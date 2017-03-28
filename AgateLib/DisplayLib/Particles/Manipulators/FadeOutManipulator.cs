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
using AgateLib.DisplayLib.Particles.Emitters;
using AgateLib.DisplayLib.Particles.Particles;

namespace AgateLib.DisplayLib.Particles.Manipulators
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
