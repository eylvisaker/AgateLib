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
using AgateLib.Physics.Constraints;
using AgateLib.Physics.Forces;
using AgateLib.Platform;

namespace AgateLib.Tests.PhysicsTests.CollisionConstraintTest
{
	public class RigidCollisionConstrantExample : Scene, IAgateTest
	{
		private KinematicsSystem system;
		private KinematicsIntegrator integrator;
		private Color[] colors;
		private double waitTime;

		public string Name => "Rigid Collision Constraint";

		public string Category => "Physics";

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}

		protected override void OnSceneStart()
		{
			AgateApp.GameClock.ClockSpeed = 0.1;

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
					Polygon = new StarBuilder().BuildStar(3, 50, 10),
					Position = new Vector2(100, 400),
					Velocity = new Vector2(0, 100),
					AngularVelocity = 0.1,
					Angle = 0.1,
				});

			system.AddForceField(new ConstantGravityField());

			system.AddConstraints(new RectangleBoundaryConstraint(new Rectangle(Point.Zero, DisplayContext.Size)));

			integrator = new KinematicsIntegrator(system, new ImpulseConstraintSolver(system))
			{
				MaxStepsPerFrame = 1
			};
		}

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			base.OnUpdate(gameClockElapsed);

			waitTime -= gameClockElapsed.TotalSeconds;

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

			Font.AgateSans.DrawText("Press space to add particles.");
		}

		private void InstallInputHandler()
		{
			var handler = new SimpleInputHandler();

			handler.KeyDown += (sender, e) =>
			{
				if (e.KeyCode == KeyCode.Escape)
				{
					if (system.Particles.Count > 0)
					{
						system.ClearParticles();
					}
					else
					{
						AgateApp.IsAlive = false;
					}
				}

				if (e.KeyCode == KeyCode.Space && waitTime <= 0)
				{
					AddRandomParticle();
					handler.Keys.Release(KeyCode.Space);
					waitTime = 0.4;
				}
			};

			InputHandler = handler;
		}

		private void AddRandomParticle()
		{
			var random = new Random();

			var size = random.Next(15, 80);
			var innerSize = random.NextDouble() * 0.7 * size + 1;

			var particle = new PhysicalParticle
			{
				Polygon = new StarBuilder().BuildStar(random.Next(3, 8), size, innerSize),
				Position = new Vector2(DisplayContext.Size.Width / 2, 100),
				Velocity = new Vector2(random.Next(-100, 100), random.Next(-1000, 1000)),
				AngularVelocity = random.NextDouble() * 4,
			};

			system.AddParticles(particle);
		}
	}
}
