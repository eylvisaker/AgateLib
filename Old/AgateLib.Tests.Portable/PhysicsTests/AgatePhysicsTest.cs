using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Platform;

namespace AgateLib.Tests.PhysicsTests.JointConstraintTest
{
	public abstract class AgatePhysicsTest : Scene, IAgateTest
	{
		private const int boxSize = 40;
		private const double TimeStep = 0.005;

		private List<Func<KinematicsSystem, IConstraintSolver>> solvers = new List<Func<KinematicsSystem, IConstraintSolver>>
		{
			system => new ProjectedGaussSeidelConstraintSolver(system),
			system => new ImpulseConstraintSolver(system)
		};

		private List<IPhysicsExample> examples = new List<IPhysicsExample>();
		private int exampleIndex;

		private List<List<PhysicalParticle>> history = new List<List<PhysicalParticle>>();
		private int historyIndex = -1;

		private IReadOnlyList<PhysicalParticle> particles => system.Particles;
		private IReadOnlyList<IPhysicalConstraint> constraints => system.Constraints;

		private KinematicsSystem system = new KinematicsSystem();
		private KinematicsIntegrator kinematics;
		private IConstraintSolver solver;

		private int debugParticleIndex;
		private int debugInfoPage;

		private bool running;
		private int solverIndex;

		protected List<IPhysicsExample> Examples => examples;

		protected IPhysicsExample CurrentExample
		{
			get { return examples[exampleIndex]; }
			set
			{
				exampleIndex = examples.IndexOf(value);
				InitializeExample();
			}
		}

		public abstract string Name { get; }

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
			InitializeInput();
			InitializeExample();
		}

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			if (system.Particles.Any(p => double.IsNaN(p.Position.X)))
				running = false;

			if (running)
				Advance(gameClockElapsed.TotalSeconds);
		}

		protected override void OnRedraw()
		{
			Display.Clear(Color.Gray);

			CurrentExample.Draw();

			Font.AgateMono.Size = 16;
			Font.AgateMono.Style = FontStyles.Bold;

			Font.AgateMono.DrawText(0, 0, DebugInfo());
		}

		protected void AddExamples(params IPhysicsExample[] _examples)
		{
			this.examples = _examples.ToList();
		}

		private void Advance(double dt = TimeStep)
		{
			CurrentExample.ComputeExternalForces();

			kinematics.Integrate(dt);

			historyIndex++;
			StoreHistory();
		}

		private void StoreHistory()
		{
			var historyItem = particles.Select(x => x.Clone()).ToList();

			if (historyIndex == history.Count)
			{
				history.Add(historyItem);
			}
			else
			{
				history[historyIndex] = historyItem;
			}
		}

		private string DebugInfo()
		{
			StringBuilder b = new StringBuilder();

			b.AppendLine($"{solver.GetType().Name}");
			b.AppendLine($"{examples[exampleIndex].Name}: {historyIndex}");
			b.AppendLine($"Particle {debugParticleIndex}");

			PrintStateOfBox(b, particles[debugParticleIndex]);

			PrintEnergyState(b);

			return b.ToString();
		}

		private void PrintEnergyState(StringBuilder b)
		{
			if (debugInfoPage == 0)
			{
				b.AppendLine($"\n  KE: {system.LinearKineticEnergy}");
				b.AppendLine($" RKE: {system.AngularKineticEnergy}");
				b.AppendLine($"  PE: {CurrentExample.PotentialEnergy}");
				b.AppendLine($"   E: {CurrentExample.PotentialEnergy + system.LinearKineticEnergy + system.AngularKineticEnergy}");
			}
		}

		private void PrintStateOfBox(StringBuilder b, PhysicalParticle box)
		{
			const double RadsToDegress = 180 / (double)Math.PI;

			b.AppendLine($"   X: {box.Position.X}");
			b.AppendLine($"   Y: {box.Position.Y}");
			b.AppendLine($"   A: {box.Angle}\n");

			if (debugInfoPage == 0)
			{
				b.AppendLine($" v_x: {box.Velocity.X}");
				b.AppendLine($" v_y: {box.Velocity.Y}");
				b.AppendLine($"   w: {box.AngularVelocity}");

				b.AppendLine($"\n F_x: {box.Force.X}");
				b.AppendLine($" F_y: {box.Force.Y}");
				b.AppendLine($"   T: {box.Torque}");

				b.AppendLine($"\nFC_x: {box.ConstraintForce.X}");
				b.AppendLine($"FC_y: {box.ConstraintForce.Y}");
				b.AppendLine($"  TC: {box.ConstraintTorque}");

				var angle = RadsToDegress * Vector2.DotProduct(box.ConstraintForce, box.Velocity) /
							(box.ConstraintForce.Magnitude * box.Velocity.Magnitude);

				b.AppendLine($"\n DOT: {angle}");
			}
			else if (debugInfoPage == 1)
			{
				solver.DebugInfo(b, debugInfoPage, box);
			}
		}

		private void InitializeExample()
		{
			history.Clear();
			historyIndex = 0;

			system = examples[exampleIndex].Initialize(Display.RenderTarget.Size);

			solver = solvers[solverIndex](system);
			kinematics = new KinematicsIntegrator(system, solver);
			kinematics.MinimumTimeStep = TimeStep;
			kinematics.MaximumTimeStep = TimeStep;

			StoreHistory();
		}

		private void InitializeInput()
		{
			var handler = new SimpleInputHandler();

			handler.KeyDown += Handler_KeyDown;

			Input.Handlers.Add(handler);
		}

		private void Handler_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Escape)
				AgateApp.Quit();

			if (e.KeyCode == KeyCode.Z)
				InitializeExample();
			if (e.KeyCode == KeyCode.Space && !running)
			{
				Advance();
			}
			if (e.KeyCode == KeyCode.Space && running)
			{
				running = false;
			}
			if (e.KeyCode == KeyCode.Enter || e.KeyCode == KeyCode.Return)
				running = !running;

			if (e.KeyCode == KeyCode.NumPadPlus || e.KeyCode == KeyCode.Plus)
			{
				CurrentExample.AddParticle();
				InitializeExample();
			}
			if (e.KeyCode == KeyCode.NumPadMinus || e.KeyCode == KeyCode.Minus)
			{
				CurrentExample.RemoveParticle();
				InitializeExample();
			}
			if (e.KeyCode == KeyCode.Left)
			{
				debugParticleIndex--;
				if (debugParticleIndex < 0)
					debugParticleIndex = 0;
			}
			if (e.KeyCode == KeyCode.Right)
			{
				debugParticleIndex++;
				if (debugParticleIndex >= system.Particles.Count)
					debugParticleIndex = system.Particles.Count - 1;
			}
			if (e.KeyCode == KeyCode.Down)
			{
				historyIndex--;
				if (historyIndex < 0)
					historyIndex = 0;

				LoadHistory();
			}
			if (e.KeyCode == KeyCode.Up)
			{
				historyIndex++;
				if (historyIndex >= history.Count)
					historyIndex = history.Count - 1;

				LoadHistory();
			}
			if (e.KeyCode == KeyCode.PageDown)
			{
				exampleIndex++;
				if (exampleIndex >= examples.Count)
					exampleIndex = examples.Count - 1;

				InitializeExample();
			}
			if (e.KeyCode == KeyCode.PageUp)
			{
				exampleIndex--;
				if (exampleIndex < 0)
					exampleIndex = 0;

				InitializeExample();
			}

			if (e.KeyCode >= KeyCode.D1 && e.KeyCode <= KeyCode.D2)
			{
				debugInfoPage = (int)(e.KeyCode - KeyCode.D1);
			}

			if (e.KeyCode >= KeyCode.F1 && e.KeyCode <= KeyCode.F12)
			{
				solverIndex = (int)(e.KeyCode - KeyCode.F1) % solvers.Count;

				InitializeExample();
			}
		}

		private void LoadHistory()
		{
			var historyItem = history[historyIndex];

			for (int i = 0; i < particles.Count; i++)
			{
				historyItem[i].CopyTo(particles[i]);
			}

			solver.ComputeConstraintForces(0.005f);
		}

	}
}