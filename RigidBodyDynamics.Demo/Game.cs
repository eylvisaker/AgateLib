using System;
using System.Collections.Generic;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public class Game
	{
		const int boxSize = 40;

		private Surface boxImage;

		private List<Physical> boxes = new List<Physical>();
		private Physical sphere = new Physical();

		private List<IPhysicalConstraint> constraints = new List<IPhysicalConstraint>();

		private KinematicsSolver solver = new KinematicsSolver();

		public Game()
		{
			Initialize();

			var pixels = new PixelBuffer(Display.DefaultSurfaceFormat, new Size(boxSize, boxSize / 2));
			pixels.FillRectangle(Color.FromArgb(220, Color.LightBlue), new Rectangle(Point.Empty, pixels.Size));
			pixels.FillRectangle(Color.FromArgb(220, Color.White), new Rectangle(1, 1, pixels.Width - 2, pixels.Height - 2));

			boxImage = new Surface(pixels);
		}

		private Size Area => Display.RenderTarget.Size;

		public int BoxCount { get; set; } = 8;

		public void Update(TimeSpan gameClockElapsed)
		{
			foreach (var box in boxes)
			{
				// Gravity force
				box.Force = new Vector2(0, 400 * box.Mass);
			}

			solver = new KinematicsSolver();

			solver.AddObjects(boxes.ToArray());
			solver.AddObjects(sphere);

			solver.AddConstraints(constraints);

			solver.Update(gameClockElapsed);
		}

		public void Draw()
		{
			foreach (var box in boxes)
				DrawBox(box);

			DrawSphere(sphere);
		}

		private void DrawBox(Physical box)
		{
			boxImage.RotationAngle = box.Angle;
			boxImage.Draw(box.Position);
		}

		private void DrawSphere(Physical physical)
		{
			Display.FillEllipse(new RectangleF(physical.Position.X - 40, physical.Position.Y - 40, 80, 80), Color.Blue);
		}


		private void Initialize()
		{
			boxes.Clear();

			for (int i = 0; i < BoxCount; i++)
			{
				boxes.Add(new Physical
				{
					Position = new Vector2(
						Area.Width * 0.5 + (BoxCount / 2 - i) * boxSize,
						Area.Height * 0.1
					),
					Velocity = new Vector2(0, (BoxCount / 2 - i) * 10)
				});

				if (i > 0)
				{
					var constraint = new PointTouchConstraint(boxes[i - 1], new Vector2(-boxSize * 0.5, 0),
						boxes[i], new Vector2(boxSize * 0.5, 0));

					constraints.Add(constraint);
				}
			}

			sphere = new Physical
			{
				Position = new Vector2(Area.Width * 0.5, Area.Height * 0.5)
			};
		}

	}
}