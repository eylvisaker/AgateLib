using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace RigidBodyDynamics
{
	public class Game
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
		private int debugParticleIndex;

		private bool running;

		public Game()
		{
			InitializeInput();

			examples.Add(new ParticleOnCircleExample());
			examples.Add(new BoxChainExample());

			InitializeExample();
		}

		private IKinematicsExample CurrentExample => examples[exampleIndex];
		
		private Size Area => Display.RenderTarget.Size;

		public int BoxCount { get; set; } = 8;

		public void Update(TimeSpan gameClockElapsed)
		{
			if (running)
				Advance((float)gameClockElapsed.TotalSeconds);
		}

		private void Advance(float dt = 0.005f)
		{
			StoreHistory();

			CurrentExample.ComputeExternalForces();

			kinematics.Integrate(dt);
		}

		private void StoreHistory()
		{
			var historyItem = particles.Select(x => x.Clone()).ToList();

			historyIndex++;

			if (historyIndex >= history.Count)
			{
				history.Add(historyItem);

				historyIndex = history.Count - 1;
			}
			else
			{
				history[historyIndex] = historyItem;
			}
		}

		public void Draw()
		{
			CurrentExample.Draw();

			Font.AgateMono.Size = 16;
			Font.AgateMono.Style = FontStyles.Bold;

			Font.AgateMono.DrawText(0, 0, DebugInfo());
		}

		private string DebugInfo()
		{
			StringBuilder b = new StringBuilder();

			b.AppendLine($"{examples[exampleIndex].Name}: {historyIndex}");
			b.AppendLine($"Particle {debugParticleIndex}");

			PrintStateOfBox(b, particles[debugParticleIndex]);

			return b.ToString();
		}

		private static void PrintStateOfBox(StringBuilder b, PhysicalParticle box)
		{
			b.AppendLine($"   X: {box.Position.X}");
			b.AppendLine($"   Y: {box.Position.Y}");
			b.AppendLine($"   A: {box.Angle}");
			b.AppendLine($"\n v_x: {box.Velocity.X}");
			b.AppendLine($" v_y: {box.Velocity.Y}");
			b.AppendLine($"   w: {box.AngularVelocity}");
			b.AppendLine($"\n F_x: {box.Force.X}");
			b.AppendLine($" F_y: {box.Force.Y}");
			b.AppendLine($"   T: {box.Torque}");
			b.AppendLine($"\nFC_x: {box.ConstraintForce.X}");
			b.AppendLine($"FC_y: {box.ConstraintForce.Y}");
			b.AppendLine($"  TC: {box.ConstraintTorque}");
		}

		private void InitializeExample()
		{
			history.Clear();
			historyIndex = 0;

			system = examples[exampleIndex].Initialize(Display.RenderTarget.Size);
			
			kinematics = new KinematicsIntegrator(system, new ConstraintSolver(system));

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
			if (e.KeyCode == KeyCode.Z)
				InitializeExample();
			if (e.KeyCode == KeyCode.Space && !running)
				Advance();
			if (e.KeyCode == KeyCode.Enter || e.KeyCode == KeyCode.Return)
				running = !running;

			if (e.KeyCode == KeyCode.NumPadPlus || e.KeyCode == KeyCode.Plus)
			{
				debugParticleIndex++;
				if (debugParticleIndex >= particles.Count)
					debugParticleIndex %= particles.Count;
			}
			if (e.KeyCode == KeyCode.NumPadMinus || e.KeyCode == KeyCode.Minus)
			{
				debugParticleIndex--;
				if (debugParticleIndex < 0)
					debugParticleIndex += particles.Count;
			}
			if (e.KeyCode == KeyCode.Left)
			{
				historyIndex--;
				if (historyIndex < 0)
					historyIndex = 0;

				LoadHistory();
			}
			if (e.KeyCode == KeyCode.Right)
			{
				historyIndex++;
				if (historyIndex >= history.Count)
					historyIndex = history.Count - 1;

				LoadHistory();
			}
			if (e.KeyCode == KeyCode.Up)
			{
				exampleIndex++;
				if (exampleIndex >= examples.Count)
					exampleIndex = examples.Count - 1;

				InitializeExample();
			}
			if (e.KeyCode == KeyCode.Down)
			{
				exampleIndex--;
				if (exampleIndex < 0)
					exampleIndex = 0;

				InitializeExample();
			}
		}

		private void LoadHistory()
		{
			var historyItem = history[historyIndex];

			for(int i = 0; i < particles.Count; i++)
			{
				historyItem[i].CopyTo(particles[i]);
			}
		}
	}
}