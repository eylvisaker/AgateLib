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
	/// A gravity particle manipulator.
	/// </summary>
	public class GravityManipulator
	{
		private Vector2f mGravity = Vector2f.Zero;
		
		/// <value>
		/// Gets or sets the gravity strength and direction.
		/// </value>
		public Vector2f Gravity
		{
			get { return mGravity; }
			set { mGravity = value; }
		}
		
		/// <summary>
		/// Constructs a gravitiy manipulator.
		/// </summary>
		/// <param name="gravity">Gravity strength and direction.</param>
		public GravityManipulator(Vector2f gravity)
		{
			mGravity = gravity;
		}

		void HandleOnUpdate(UpdateArgs args)
		{
			foreach(Particle pt in args.Emitter.Particles)
			{
				if(pt.IsAlive == true)
				{
					pt.Acceleration = pt.Acceleration + Gravity * (float)args.Time_ms/1000;
					// pt.Acceleration = pt.Acceleration + Position * Strength * args.Time_ms;
					// pt.Velocity = pt.Velocity + Position * Strength * args.Time_ms;
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
