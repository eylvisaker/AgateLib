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

using AgateLib.Mathematics;

namespace AgateLib.DisplayLib.Particles.Manipulators
{
	/// <summary>
	/// A point gravity particle manipulator.
	/// </summary>
	public class GravityPointManipulator
	{
		private Vector2f mPosition;
		private float mForce;
		private float mRange;
		
		/// <summary>
		/// Gets or sets the position of the point gravity.
		/// </summary>
		public Vector2f Position {
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
		public GravityPointManipulator (Vector2f position, float force)
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
		public GravityPointManipulator (Vector2f position, float force, float range)
		{
			mPosition = position;
			mForce = force;
			mRange = range;			
		}
		
		void HandleOnUpdate (UpdateArgs args)
		{
			Vector2f way;
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
					if(Vector2f.DistanceBetween(particle.Position, Position) <= Range)
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