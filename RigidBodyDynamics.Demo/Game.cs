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
		const int boxSize = 40;

		private Surface boxImage;

		private List<List<PhysicalParticle>> history = new List<List<PhysicalParticle>>();
		private int historyIndex = -1;

		private List<PhysicalParticle> boxes = new List<PhysicalParticle>();
		private PhysicalParticle sphere = new PhysicalParticle();

		private List<IPhysicalConstraint> constraints = new List<IPhysicalConstraint>();

		private KinematicsSystem system = new KinematicsSystem();
		private KinematicsIntegrator kinematics;
		private int debugBoxIndex;

		public Game()
		{
			Initialize();

			var pixels = new PixelBuffer(Display.DefaultSurfaceFormat, new Size(boxSize, boxSize / 2));
			pixels.FillRectangle(Color.FromArgb(220, Color.LightBlue), new Rectangle(Point.Empty, pixels.Size));
			pixels.FillRectangle(Color.FromArgb(220, Color.White), new Rectangle(1, 1, pixels.Width - 2, pixels.Height - 2));

			boxImage = new Surface(pixels);

			var handler = new SimpleInputHandler();

			handler.KeyDown += Handler_KeyDown;

			Input.Handlers.Add(handler);
		}

		private Size Area => Display.RenderTarget.Size;

		public int BoxCount { get; set; } = 8;

		public void Update(TimeSpan gameClockElapsed)
		{
		}

		private void Advance()
		{
			StoreHistory();

			ComputeExternalForces();

			kinematics.Update(0.0001f);
		}

		private void ComputeExternalForces()
		{
			foreach (var box in boxes)
			{
				// Gravity force
				box.Force = new Vector2(0, 100 * box.Mass);
			}
		}

		private void StoreHistory()
		{
			var historyItem = boxes.Select(x => x.Clone()).ToList();

			historyIndex++;

			if (historyIndex >= history.Count)
			{
				history.Add(historyItem);

				historyIndex = history.Count;
			}
			else
			{
				history[historyIndex] = historyItem;
			}
		}

		public void Draw()
		{
			foreach (var box in boxes)
				DrawBox(box);

			DrawSphere(sphere);

			Font.AgateMono.Size = 16;
			Font.AgateMono.Style = FontStyles.Bold;

			Font.AgateMono.DrawText(0, 0, DebugInfo());
		}

		private string DebugInfo()
		{
			StringBuilder b = new StringBuilder();

			b.AppendLine(historyIndex.ToString());
			b.AppendLine($"\nBox {debugBoxIndex}");

			PrintStateOfBox(b, boxes[debugBoxIndex]);

			var secondBox = (debugBoxIndex + 1) % boxes.Count;
			b.AppendLine($"\nBox {secondBox}");
			PrintStateOfBox(b, boxes[secondBox]);

			return b.ToString();
		}

		private static void PrintStateOfBox(StringBuilder b, PhysicalParticle box)
		{
			b.AppendLine($"  X: {box.Position.X}");
			b.AppendLine($"  Y: {box.Position.Y}");
			b.AppendLine($"  A: {box.Angle}");
			b.AppendLine($"\nv_x: {box.Velocity.X}");
			b.AppendLine($"v_y: {box.Velocity.Y}");
			b.AppendLine($"  w: {box.AngularVelocity}");
			b.AppendLine($"\nF_x: {box.Force.X}");
			b.AppendLine($"F_y: {box.Force.Y}");
			b.AppendLine($"  T: {box.Torque}");
		}

		private void DrawBox(PhysicalParticle box)
		{
			boxImage.RotationAngle = box.Angle;
			boxImage.Draw(box.Position);
		}

		private void DrawSphere(PhysicalParticle physicalParticle)
		{
			Display.FillEllipse(new RectangleF(physicalParticle.Position.X - 40, physicalParticle.Position.Y - 40, 80, 80), Color.Blue);
		}


		private void Initialize()
		{
			boxes.Clear();
			constraints.Clear();
			history.Clear();

			for (int i = 0; i < BoxCount; i++)
			{
				boxes.Add(new PhysicalParticle
				{
					Position = new Vector2(
						Area.Width * 0.5 + (BoxCount / 2 - i) * boxSize,
						Area.Height * 0.1
					),
					Velocity = new Vector2(0, (BoxCount / 2 - i) * 250)
				});

				if (i > 0)
				{
					var constraint = new PointTouchConstraint(boxes[i - 1], new Vector2(-boxSize * 0.5, 0),
						boxes[i], new Vector2(boxSize * 0.5, 0));

					constraints.Add(constraint);
				}
			}

			sphere = new PhysicalParticle
			{
				Position = new Vector2(Area.Width * 0.5, Area.Height * 0.5)
			};

			system = new KinematicsSystem();
			kinematics = new KinematicsIntegrator(system, new ConstraintSolver(system));

			system.AddObjects(boxes.ToArray());
			//system.AddObjects(sphere);

			system.AddConstraints(constraints);

			StoreHistory();
		}

		private void Handler_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Z)
				Initialize();
			if (e.KeyCode == KeyCode.Space)
				Advance();

			if (e.KeyCode == KeyCode.NumPadPlus || e.KeyCode == KeyCode.Plus)
			{
				debugBoxIndex++;
				if (debugBoxIndex >= boxes.Count)
					debugBoxIndex %= boxes.Count;
			}
			if (e.KeyCode == KeyCode.NumPadMinus || e.KeyCode == KeyCode.Minus)
			{
				debugBoxIndex--;
				if (debugBoxIndex < 0)
					debugBoxIndex += boxes.Count;
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
		}

		private void LoadHistory()
		{
			var historyItem = history[historyIndex];

			for(int i = 0; i < boxes.Count; i++)
			{
				historyItem[i].CopyTo(boxes[i]);
			}
		}
	}
}