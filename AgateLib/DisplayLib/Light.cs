//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which represents a single light source.
	/// Only point light sources are supported at the moment.
	/// </summary>
	public class Light
	{
		private bool mEnabled = true;
		private Color mAmbient = Color.Black;
		private Color mDiffuse = Color.White;
		private Color mSpecular = Color.Black;
		private Vector3 mPosition = Vector3.Empty;
		private float mConstantAttenuation = 0.01f;
		private float mLinearAttenuation = 0.0f;
		private float mQuadraticAttenuation = 0.00001f;
		private float mRange = 100000;

		/// <summary>
		/// The constant value in the Lighting formula
		/// <seealso cref="SetAttenuationModel"/>
		/// </summary>
		public float AttenuationConstant
		{
			get { return mConstantAttenuation; }
			set { mConstantAttenuation = value; }
		}
		/// <summary>
		/// The linear value in the Lighting formula
		/// <seealso cref="SetAttenuationModel"/>
		/// </summary>
		public float AttenuationLinear
		{
			get { return mLinearAttenuation; }
			set { mLinearAttenuation = value; }
		}
		/// <summary>
		/// The quadratic value in the Lighting formula
		/// <seealso cref="SetAttenuationModel"/>
		/// </summary>
		public float AttenuationQuadratic
		{
			get { return mQuadraticAttenuation; }
			set { mQuadraticAttenuation = value; }
		}
		/// <summary>
		/// Sets the three constants in the attenuation model.  See remarks for details.
		/// </summary>
		/// <remarks>
		/// Lights decrease in intensity for objects which are farther away from the lights.
		/// This is called attenuation.
		/// Point Lights are attenuated according to the following formula:
		/// <para>
		/// K = C<sub>0</sub> + C<sub>1</sub>*d + C<sub>2</sub>*d<sup>2</sup>
		/// </para>
		/// <para>
		/// A = 1 / K
		/// </para>
		/// where d is the distance from the light the object being rendered is.  The Light's
		/// color components are multiplied by A to decrease the intensity of the light on this
		/// object.
		/// </remarks>
		/// <param name="constant"></param>
		/// <param name="linear"></param>
		/// <param name="quadratic"></param>
		public void SetAttenuationModel(float constant, float linear, float quadratic)
		{
			AttenuationConstant = constant;
			AttenuationLinear = linear;
			AttenuationQuadratic = quadratic;
		}
		/// <summary>
		/// Gets or sets the distance at which the rendering API can consider the light
		/// too far away to have any affect.
		/// </summary>
		public float Range
		{
			get { return mRange; }
			set { mRange = value; }
		}
		/// <summary>
		/// Gets or sets whether or not this Light should have any effect on anything which is rendered.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return mEnabled;
			}
			set
			{
				mEnabled = value;
			}
		}
		/// <summary>
		/// Ambient color for the light.  Ambient color is not affected by the dot product with the
		/// normal, so it appears to attenuate slower.
		/// <seealso cref="Diffuse"/>
		/// </summary>
		public Color Ambient
		{
			get { return mAmbient; }
			set { mAmbient = value; }
		}
		/// <summary>
		/// Diffuse color for the light.  The diffuse color is modulated by the dot product of the
		/// direction to the light with the surface normal (for 2D drawing in AgateLib, surface normals
		/// are always in the negative z direction).  So it attenuates faster than the Ambient color does.
		/// </summary>
		public Color Diffuse
		{
			get { return mDiffuse; }
			set { mDiffuse = value; }
		}
		/// <summary>
		/// Specular highlight color, or "shininess."  Not currently used.
		/// </summary>
		public Color Specular
		{
			get { return mSpecular; }
			set { mSpecular = value; }
		}
		/// <summary>
		/// The position of the Light source, in 3D space.  Normals in AgateLib are in the negative
		/// z direction, so it is recommended to make Position.Z a negative number.  How negative
		/// depends on the Attenuation model and what kind of effect you wish to create with the
		/// lighting.
		/// </summary>
		public Vector3 Position
		{
			get { return mPosition; }
			set { mPosition = value; }
		}

	}
}