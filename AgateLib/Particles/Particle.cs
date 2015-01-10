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

using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Particles
{
	/// <summary>
	/// Base class for particles.
	/// </summary>
	public class Particle
	{
		private float mLife = 10f;
		private ParticleState mCondition = ParticleState.Paused;
		
		private Vector2 mAcceleration = Vector2.Empty;
		private Vector2 mPosition = Vector2.Empty;
		private Vector2 mVelocity = Vector2.Empty;

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
		public Vector2 Acceleration
		{
			get { return mAcceleration; }
			set { mAcceleration = value; }
		}

		/// <value>
		/// Gets or sets the position.
		/// </value>
		public Vector2 Position
		{
			get { return mPosition; }
			set { mPosition = value; }
		}
		
		/// <value>
		/// Gets or sets the velocity.
		/// </value>
		public Vector2 Velocity
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
