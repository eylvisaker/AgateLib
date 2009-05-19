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

namespace AgateLib.Particles
{
	/// <summary>
	/// A surface particle class.
	/// </summary>
	public class SurfaceParticle : Particle
	{
		private int mSurfaceKey;
		
		private double mAlpha = 1d;
		
		/// <value>
		/// Gets or sets the surface key.
		/// </value>
		public int SurfaceKey
		{
			get { return mSurfaceKey; }
			set { mSurfaceKey = value; }
		}
		
		/// <value>
		/// Gets or sets the alpha of the surface.
		/// </value>
		public double Alpha
		{
			get { return mAlpha; }
			set { mAlpha = value; }
		}
		
		/// <summary>
		/// Constructs a SurfaceParticle with a default SurfaceKey=0.
		/// </summary>
		public SurfaceParticle() : this(0) {}
		
		/// <summary>
		/// Constructs a SurfaceParticle.
		/// </summary>
		/// <param name="surfaceKey"></param>
		public SurfaceParticle(int surfaceKey)
		{
			SurfaceKey = surfaceKey;
		}
	}
}
