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

using AgateLib.DisplayLib;
using AgateLib.Mathematics;

namespace AgateLib.DisplayLib.Particles
{
	/// <summary>
	/// Base class for particles.
	/// </summary>
	public class Particle
	{
		private float mLife = 10f;
		private ParticleState mCondition = ParticleState.Paused;
		
		private Vector2f mAcceleration = Vector2f.Zero;
		private Vector2f mPosition = Vector2f.Zero;
		private Vector2f mVelocity = Vector2f.Zero;

		/// <value>
		/// Gets or sets the life.
		/// Life is stored in seconds.
		/// </value>
		public float Life
		{
			get { return mLife; }
			set { mLife = value; }
		}
		
		/// <value>
		/// Is particle alive.
		/// </value>
		public bool IsAlive
		{
			get {
				if(mCondition == ParticleState.Dead)
					return false;
				else
					return true;
			}
		}
		
		/// <value>
		/// Gets or sets the condition.
		/// </value>
		public ParticleState Condition
		{
			get { return mCondition; }
			set { mCondition = value; }
		}
		
		/// <value>
		/// Gets or sets the acceleration.
		/// </value>
		public Vector2f Acceleration
		{
			get { return mAcceleration; }
			set { mAcceleration = value; }
		}

		/// <value>
		/// Gets or sets the position.
		/// </value>
		public Vector2f Position
		{
			get { return mPosition; }
			set { mPosition = value; }
		}
		
		/// <value>
		/// Gets or sets the velocity.
		/// </value>
		public Vector2f Velocity
		{
			get { return mVelocity; }
			set { mVelocity = value; }
		}
		
		/// <summary>
		/// Updates the particle.
		/// </summary>
		/// <param name="time_ms">
		/// Passed time in milliseconds since last update.
		/// </param>
		public virtual void Update(double time_ms)
		{
			// If the particle is not alive don't simulate
			if(mCondition != ParticleState.Alive)
				return;
			
			// passed time in seconds
			float time = (float)time_ms/1000;
			mLife -= time;
			
			if(mLife <= 0)
			{
				mCondition = ParticleState.Dead;
			}
			
			// Euler method
			// v = v + a * dt
			// x = x + v * dt
			mVelocity = mVelocity + mAcceleration * time;
			mPosition = mPosition + mVelocity * time;
			
			// verlet integration
			// xi+1 = xi + (xi - xi-1) + a * dt * dt
			// mPosition = mPosition + (mPosition + mPosition - 1) + mAcceleration * time * time;
			
			// timer correct verlet integration
			// xi+1 = xi + (xi - xi-1) * (dti / dti-1) + a * dti * dti
			// mPosition = mPosition + (mPosition - mPosition - 1) * (time / time - 1) + mAcceleration * time * time;
		}
	}
	
	/// <summary>
	/// Describes the condition of an Particle
	/// </summary>
	public enum ParticleState
	{
		/// <summary>
		/// Particle is alive and will be simulated.
		/// </summary>
		Alive,
		/// <summary>
		/// Particle is dead and is ready to get recycled.
		/// </summary>
		Dead,
		/// <summary>
		/// Particle is paused and will not be simulated.
		/// </summary>
		Paused,
		/// <summary>
		/// Particle is frozen and will not be simulated but drawn.
		/// </summary>
		Frozen
	}
}
