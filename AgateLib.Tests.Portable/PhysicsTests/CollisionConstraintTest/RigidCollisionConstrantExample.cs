using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.InputLib.GamepadModel;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using AgateLib.Physics;
using AgateLib.Physics.Forces;
using AgateLib.Platform;

namespace AgateLib.Tests.PhysicsTests.CollisionConstraintTest
{
	public class RigidCollisionConstrantExample : Scene, IAgateTest
	{
		private KinematicsSystem system;
		private KinematicsIntegrator integrator;
		private Color[] colors;

		public string Name => "Rigid Collision Constraint";

		public string Category => "Physics";

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(1024, 768)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}

		protected override void OnSceneStart()
		{
			InstallInputHandler();
			colors = new[]
			{
				Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Purple, Color.Violet, Color.Indigo,
				Color.Orange, Color.White,
			};

			for (int i = 0; i < colors.Length; i++)
				colors[i] = Color.FromArgb(196, colors[i]);

			system = new KinematicsSystem();

			system.AddParticles(
				new PhysicalParticle
				{
					Polygon = new StarBuilder().BuildStar(5, 50, 20),
					Position = new Vector2(100, 100),
					AngularVelocity = 0.1,
				}, 
				new PhysicalParticle
				{
					Polygon = new Rectangle(-20, -20, 40, 40).ToPolygon(),
					Position = new Vector2(115, 300),
				});

			system.AddForceField(new PlanetSurfaceGravity());

			integrator = new KinematicsIntegrator(system, new ImpulseConstraintSolver(system));

		}

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			base.OnUpdate(gameClockElapsed);

			integrator.Integrate(gameClockElapsed.TotalSeconds);
		}

		protected override void OnRedraw()
		{
			base.OnRedraw();

			int index = 0;

			foreach (var particle in system.Particles)
			{
				Display.Primitives.FillPolygon(colors[index % colors.Length],
					particle.TransformedPolygon);

				index++;
			}
		}

		private void InstallInputHandler()
		{
			var handler = new SimpleInputHandler();

			handler.KeyDown += (sender, e) =>
			{
				if (e.KeyCode == KeyCode.Escape)
					AgateApp.IsAlive = false;

				if (e.KeyCode == KeyCode.Space)
				{
					AddRandomParticle();
					handler.Keys.Release(KeyCode.Space);
				}
			};

			InputHandler = handler;
		}

		private void AddRandomParticle()
		{
			var random = new Random();

			var size = random.Next(20, 100);
			var innerSize = random.NextDouble() * 0.7 * size;

			var particle = new PhysicalParticle
			{
				Polygon = new StarBuilder().BuildStar(random.Next(3, 8), size, innerSize),
				Position = new Vector2(DisplayContext.Size.Width / 2, 0),
				Velocity = new Vector2(random.Next(-1000, 1000), random.Next(-1000, 1000)),
				AngularVelocity = random.NextDouble(),
			};

			system.AddParticles(particle);
		}
	}
}
