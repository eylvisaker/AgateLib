using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics;
using AgateLib.Platform;

namespace AgateLib.Tests.PhysicsTests.JointConstraintTest
{
	public class JointConstraintTester : Scene, IAgateTest
	{
		private const int boxSize = 40;

		private List<IKinematicsExample> examples = new List<IKinematicsExample>();
		private int exampleIndex;

		private List<List<PhysicalParticle>> history = new List<List<PhysicalParticle>>();
		private int historyIndex = -1;

		private IReadOnlyList<PhysicalParticle> particles => system.Particles;
		private IReadOnlyList<IPhysicalConstraint> constraints => system.Constraints;

		private KinematicsSystem system = new KinematicsSystem();
		private KinematicsIntegrator kinematics;
		private IConstraintSolver constraintSolver;

		private int debugParticleIndex;
		private int debugInfoPage;

		private bool running;

		public string Name => "Physics Demo";
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

		private IKinematicsExample CurrentExample => examples[exampleIndex];

		private Size Area => Display.RenderTarget.Size;

		public int BoxCount { get; set; } = 8;

		protected override void OnSceneStart()
		{
			InitializeInput();

			examples.Add(new ParticleOnCircleOffCenterExample());
			examples.Add(new SmallChainNoGravityExample());
			examples.Add(new ChainOnCircleExample());
			examples.Add(new BoxChainExample());
			examples.Add(new ParticleOnCircleExample());

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


		private void Advance(double dt = 0.005)
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
			const float RadsToDegress = 180 / (float)Math.PI;

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
				constraintSolver.DebugInfo(b, debugInfoPage, box);
			}
		}

		private void InitializeExample()
		{
			history.Clear();
			historyIndex = 0;

			system = examples[exampleIndex].Initialize(Display.RenderTarget.Size);

			constraintSolver = new ImpulseConstraintSolver(system);
			kinematics = new KinematicsIntegrator(system, constraintSolver);

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
				Advance();
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
		}

		private void LoadHistory()
		{
			var historyItem = history[historyIndex];

			for (int i = 0; i < particles.Count; i++)
			{
				historyItem[i].CopyTo(particles[i]);
			}

			constraintSolver.ComputeConstraintForces(0.005f);
		}

	}
}