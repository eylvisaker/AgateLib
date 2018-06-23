//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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

using System;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using Microsoft.Xna.Framework;

namespace AgateLib.Physics.TwoDimensions
{
	public class PhysicalParticle
	{
		private Polygon untransformed;
		private Polygon transformed;

		private float mass = 1;
		private float inertialMoment = 1;

		/// <summary>
		/// Particle position.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Particle velocity.
		/// </summary>
		public Vector2 Velocity;

		/// <summary>
		/// Particle force.
		/// </summary>
		public Vector2 Force;

		/// <summary>
		/// Force exerted by constraints.
		/// </summary>
		public Vector2 ConstraintForce;

		/// <summary>
		/// Particle rotation angle.
		/// </summary>
		public float Angle;

		/// <summary>
		/// Particle angular velocity.
		/// </summary>
		public float AngularVelocity;

		/// <summary>
		/// Particle torque.
		/// </summary>
		public float Torque;

		/// <summary>
		/// Torque exerted by constraints.
		/// </summary>
		public float ConstraintTorque;

		/// <summary>
		/// The untransformed bounding polygon for this particle. This can be used by a rigid body constraint
		/// to handle collisions.
		/// </summary>
		public Polygon Polygon
		{
			get => untransformed;
			set
			{
				untransformed = value;
				transformed = null;
			}
		}

		/// <summary>
		/// The transformed polygon representing this object's physical location in space.
		/// </summary>
		public Polygon TransformedPolygon
		{
			get
			{
				if (transformed == null)
					UpdatePolygonTransformation();

				return transformed;
			}
		}

		/// <summary>
		/// Particle mass. Must not be positive.
		/// </summary>
		public float Mass
		{
			get => mass;
			set
			{
				Require.ArgumentInRange(mass > 0, nameof(mass), "Mass must be positive.");
				mass = value;
			}
		}

		/// <summary>
		/// Particle intertial moment. Must not be positive.
		/// </summary>
		public float InertialMoment
		{
			get => inertialMoment;
			set
			{
				Require.ArgumentInRange(inertialMoment > 0, nameof(inertialMoment), "Inertial moment must be positive.");
				inertialMoment = value;
			}
		}

		/// <summary>
		/// Clones this PhysicalParticle object.
		/// </summary>
		/// <returns></returns>
		public PhysicalParticle Clone()
		{
			var result = new PhysicalParticle();
			CopyTo(result);

			return result;
		}

		/// <summary>
		/// Copies data to another PhysicalParticle object.
		/// </summary>
		/// <param name="target"></param>
		public void CopyTo(PhysicalParticle target)
		{
			target.Polygon = Polygon;

			target.Position = Position;
			target.Velocity = Velocity;
			target.Force = Force;
			target.ConstraintForce = ConstraintForce;

			target.Angle = Angle;
			target.AngularVelocity = AngularVelocity;
			target.Torque = Torque;
			target.ConstraintTorque = ConstraintTorque;

			target.Mass = Mass;
			target.InertialMoment = InertialMoment;
		}

		/// <summary>
		/// Integrates the equations of motion for this particle alone. 
		/// </summary>
		/// <param name="dt"></param>
		public virtual void Integrate(float dt)
		{
			var oldVelocity = Velocity;
			var oldAngularVelocity = AngularVelocity;

			AngularVelocity += dt * Torque / InertialMoment;
			Velocity += dt * Force / Mass;

			// Cheap way to improve the integrator: use the average of the 
			// start & final velocities for the position integration.
			Angle += dt * 0.5f * (AngularVelocity + oldAngularVelocity);
			Position += dt * 0.5f * (Velocity + oldVelocity);

			UpdatePolygonTransformation();
		}

		internal void UpdatePolygonTransformation()
		{
			if (untransformed == null)
				return;

			if (transformed == null)
				transformed = new Polygon();

			untransformed.CopyTo(transformed);

			transformed.RotateSelf(Angle);
			transformed.TranslateSelf(Position);
		}

		/// <summary>
		/// Resets all values to their defaults.
		/// </summary>
		public void Clear()
		{
			mass = 1;
			inertialMoment = 1;

			Position = Vector2.Zero;
			Velocity = Vector2.Zero;
			Force = Vector2.Zero;

			Angle = 0;
			AngularVelocity = 0;
			Torque = 0;

			Polygon?.Clear();
			TransformedPolygon?.Clear();
		}
	}
}