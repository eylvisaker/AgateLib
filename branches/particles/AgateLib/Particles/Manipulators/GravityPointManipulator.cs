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
	/// A point gravity particle manipulator.
	/// </summary>
	public class GravityPointManipulator
	{
		private Vector2 mPosition;
		private float mForce;
		private float mRange;
		
		/// <summary>
		/// Gets or sets the position of the point gravity.
		/// </summary>
		public Vector2 Position {
			get { return mPosition; }
			set { mPosition = value; }
		}
		
		/// <summary>
		/// Gets or sets the gravity force.
		/// </summary>
		public float Force {
			get { return mForce; }
			set { mForce = value; }
		}
		
		/// <summary>
		/// Gets or sets the range of the gravity impact.
		/// A value of 0 means the gravity is disabled.
		/// A value lower than 0 means the gravity has impact on all registered particles.
		/// </summary>
		public float Range {
			get { return mRange; }
			set { mRange = value; }
		}
		
		
		/// <summary>
		/// Constructs a GravityPointManipulator with global gravity.
		/// </summary>
		/// <param name="position">The position of the point gravity.</param>
		/// <param name="force">The gravity force.</param>
		public GravityPointManipulator (Vector2 position, float force)
		{
			mPosition = position;
			mForce = force;
			mRange = -1;
		}
		
		/// <summary>
		/// Constructs a GravityPointManipulator.
		/// </summary>
		/// <param name="position">The position of the point gravity.</param>
		/// <param name="force">The gravity force.</param>
		/// <param name="range">The range of the gravity impact.</param>
		public GravityPointManipulator (Vector2 position, float force, float range)
		{
			mPosition = position;
			mForce = force;
			mRange = range;			
		}
		
		void HandleOnUpdate (UpdateArgs args)
		{
			Vector2 way;
			if(Range < 0) // global effect on all registered particles
			{
				foreach(Particle particle in args.Emitter.Particles)
				{
					way = particle.Position - Position;
					particle.Velocity += way.Normalize() * Force;
				}
			}
			else if (Range > 0) // only particles in range are effected
			{
				foreach(Particle particle in args.Emitter.Particles)
				{
					if(Vector2.DistanceBetween(particle.Position, Position) <= Range)
					{
						way = particle.Position - Position;
						particle.Velocity += way.Normalize() * Force;
					}
				}
			}
			// Range == 0 means no gravity impact
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