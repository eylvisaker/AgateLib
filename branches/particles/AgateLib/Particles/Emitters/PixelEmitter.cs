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
	/// A pixel particle emitter.
	/// </summary>
	public class PixelEmitter : ParticleEmitter
	{
		private Color mEmitColor = Color.White;
		
		private Surface drawSurf = new Surface(1, 1);
		private float time = 0f;
		
		/// <value>
		/// Gets or sets the emit color.
		/// </value>
		public Color EmitColor
		{
			get { return mEmitColor; }
			set { mEmitColor = value; }
		}
		
		/// <summary>
		/// Constructs a pixel particle emitter.
		/// </summary>
		/// <param name="color">Emit color.</param>
		public PixelEmitter(Color color)
		{
			mEmitColor = color;
		}
		
		/// <summary>
		/// Overridden Draw method.
		/// Draws each living particle.
		/// </summary>
		public override void Draw ()
		{
			foreach(PixelParticle ptl in Particles)
			{
				if(ptl.Condition == Condition.ALive)
				{
					drawSurf.Color = ptl.Color;
					drawSurf.Draw(ptl.Position.X, ptl.Position.Y);
				}
			}
		}
		
		/// <summary>
		/// Overridden Update mehtod.
		/// Emits particles based on the frequenzy property.
		/// Updates the emitter position.
		/// Calls particle manipulators to manipulate particles.
		/// </summary>
		/// <param name="time_ms">
		/// A <see cref="System.Single"/>
		/// </param>
		public override void Update (float time_ms)
		{
			time += time_ms;
			float frequenzy = EmitFrequenzy*1000;
			
			while(time >= frequenzy)
			{
				// TODO: recyle dead particles
				Particles.Add(new PixelParticle(EmitColor));
				time -= frequenzy;
			}
			
			// updates own position and calls manipulators
			base.Update (time_ms);
		}
	}
}
