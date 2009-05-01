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
		private float mStrength = 1f;
		private Vector2 mPosition = Vector2.Empty;		
		
		/// <value>
		/// Gets or sets the strength.
		/// </value>
		public float Strength
		{
			get { return mStrength; }
			set { mStrength = value; }
		}
		
		/// <value>
		/// Gets or sets the position.
		/// </value>
		public Vector2 Position
		{
			get { return mPosition; }
			set { mPosition = value; }
		}
		
		/// <summary>
		/// Constructs a gravitiy manipulator.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="strength"></param>
		public GravityManipulator(Vector2 position, float strength)
		{
			mPosition = position;
			mStrength = strength;
		}
		
		/// <summary>
		/// Subscribe to a particle emitter.
		/// </summary>
		/// <param name="emitter"></param>
		public void SubscribeToEmitter(ParticleEmitter emitter)
		{
			emitter.UpdateParticles += Manipulate;
		}
		
		internal void Manipulate(List<Particle> particles, float time_ms)
		{
			// TODO: add missing calculation to gravity manipulator
		}
	}
}
