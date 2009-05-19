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

using AgateLib.Geometry;

namespace AgateLib.Particles
{
	/// <summary>
	/// A gravity particle manipulator.
	/// </summary>
	public class GravityManipulator
	{
		private Vector2 mGravity = Vector2.Empty;
		
		/// <value>
		/// Gets or sets the gravity strength and direction.
		/// </value>
		public Vector2 Gravity
		{
			get { return mGravity; }
			set { mGravity = value; }
		}
		
		/// <summary>
		/// Constructs a gravitiy manipulator.
		/// </summary>
		/// <param name="gravity">Gravity strength and direction.</param>
		public GravityManipulator(Vector2 gravity)
		{
			mGravity = gravity;
		}

		void HandleOnUpdate(UpdateArgs args)
		{
			foreach(Particle pt in args.Emitter.Particles)
			{
				if(pt.IsALive == true)
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
