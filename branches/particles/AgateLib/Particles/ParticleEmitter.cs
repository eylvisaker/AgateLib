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

using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Particles
{
	/// <summary>
	/// Updates particles.
	/// Provides a list of particles and passed time in milliseconds since last update.
	/// </summary>
	public delegate void UpdateParticles(List<Particle> particles, float time_ms);
	
	/// <summary>
	/// Base class for particle emitters.
	/// </summary>
	public abstract class ParticleEmitter : Particle
	{	
		private List<Particle> mParticles = new List<Particle>();
		
		private float mEmitFrequenzy = 1f;
		
		/// <value>
		/// Gets or sets the particles.
		/// </value>
		public List<Particle> Particles
		{
			get { return mParticles; }
			set { mParticles = value; }
		}
		
		/// <value>
		/// Gets or sets the emit frequenzy in seconds.
		/// </value>
		public float EmitFrequenzy
		{
			get { return mEmitFrequenzy; }
			set { mEmitFrequenzy = value; }
		}
		
		/// <summary>
		/// Delegate to update particles.
		/// Particle manipulators should subscribe here.
		/// </summary>
		public UpdateParticles UpdateParticles;
		
		/// <summary>
		/// Draws each particle.
		/// </summary>
		public virtual void Draw ()
		{
			// Draws particles
		}

		/// <summary>
		/// Overridden update method.
		/// Updates and manipulates each particle.
		/// </summary>
		/// <param name="time_ms">
		/// Passed time in milliseconds since last update.
		/// </param>
		public override void Update (float time_ms)
		{
			UpdateParticles(mParticles, time_ms);
			base.Update (time_ms);
		}
	}
}
