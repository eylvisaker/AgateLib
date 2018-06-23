using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Physics.TwoDimensions;

namespace AgateLib.Tests.PhysicsTests.CollisionConstraintTest
{
	class ParticleRenderer
	{
		private Color[] colors;

		public ParticleRenderer()
		{
			colors = new[]
			{
				Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Purple, Color.Violet, Color.Indigo,
				Color.Orange, Color.White, Color.Black,
			};

			for (int i = 0; i < colors.Length; i++)
				colors[i] = Color.FromArgb(196, colors[i]);
		}

		public void Draw(KinematicsSystem system)
		{
			int index = 0;

			foreach (var particle in system.Particles)
			{
				Display.Primitives.FillPolygon(colors[index % colors.Length],
					particle.TransformedPolygon);

				index++;
			}
		}
	}
}
