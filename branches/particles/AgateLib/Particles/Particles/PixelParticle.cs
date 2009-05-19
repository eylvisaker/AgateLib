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
	/// A particle class which represents a particle with one colored pixels.
	/// </summary>
	public class PixelParticle : Particle
	{		
		private Color mColor = Color.White;
		
		/// <value>
		/// Gets or sets the color.
		/// </value>
		public Color Color
		{
			get { return mColor; }
			set { mColor = value; }
		}		
		
		/// <summary>
		/// Constructs a PixelParticle.
		/// </summary>
		/// <param name="color">
		/// The color of the particle.
		/// </param>
		public PixelParticle(Color color)
		{
			mColor = color;
		}
	}
}
