// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.Drivers;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace Tests.PerformanceTester
{
	class PerformanceTester : IAgateTest
	{
		public struct TestResult
		{
			public int Frames;
			public double Time;
			public string Name;
			public string Driver;

			public double FPS
			{
				get { return Frames / Time; }
			}
		}
		struct Rects
		{
			public Rectangle rect;
			public Color color;
		}
		Random rnd = new Random();
		FontSurface font;

		delegate int TestMethod();

		const int totalFrames = 300;

		public void Main(string[] args)
		{
			Core.Initialize();

			List<AgateDriverInfo> drivers = Registrar.DisplayDrivers;

			frmPerformanceTester frm = new frmPerformanceTester();
			frm.Show();

			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

			List<TestMethod> tests = new List<TestMethod>();
			tests.Add(FilledRectTest);
			tests.Add(DrawRectTest);
			tests.Add(DrawSurfaceTestColored);
			tests.Add(DrawSurfaceTestPlain);
			tests.Add(StretchTestColored);
			tests.Add(StretchTestPlain);

			foreach (AgateDriverInfo info in drivers)
			{
				Trace.WriteLine(string.Format("Starting driver {0}...", info.FriendlyName));
				Trace.Indent();
				double fps;

				Display.Initialize((DisplayTypeID)info.DriverTypeID);
				Display.VSync = false;

				DisplayWindow wind = DisplayWindow.CreateWindowed("Performance Test", 300, 300);
				font = new FontSurface("Arial", 11);

				for (int i = 0; i < tests.Count; i++)
				{
					TestResult r = Execute(tests[i]);

					r.Driver = info.FriendlyName;

					frm.AddTestResult(r);
				}

				if (Display.CurrentWindow.IsClosed)
				{
					Display.Dispose();
					frm.Dispose();
					return;
				}

				Display.Dispose();
			}

			frm.Visible = false;
			frm.ShowDialog();
		}

		private TestResult Execute(TestMethod testMethod)
		{
			Timing.StopWatch timer = new Timing.StopWatch();
			int frames = testMethod();
			timer.Pause();

			TestResult r = new TestResult();

			r.Name = testMethod.Method.Name;
			r.Frames = frames;
			r.Time = timer.TotalSeconds;

			return r;
		}

		private int StretchTestQueue(bool applyColor)
		{
			Surface surf = new Surface("jellybean.png");
			int frames = 0;

			surf.Color = Color.White;
			double count = 1;

			for (frames = 0; frames < totalFrames; frames++)
			{
				if (Display.CurrentWindow.IsClosed)
					return frames;

				count = 1 + Math.Cos(frames / 60.0);

				Display.BeginFrame();


				for (int i = 0; i < 15; i++)
				{
					surf.SetScale(1.0, 1.0);
					surf.Color = Color.White;
					surf.Draw(10, 10);

					if (applyColor)
						surf.Color = Color.Orange;

					surf.SetScale(1.5, 1.5);
					surf.Draw(120, 10);

					if (applyColor)
						surf.Color = Color.LightGreen;

					surf.SetScale(count, 1.25);
					surf.Draw(10, 100);

					if (applyColor)
						surf.Color = Color.LightCoral;

					surf.SetScale(0.5 + count / 2, 0.5 + count / 2);
					surf.Draw((int)(150 + 40 * Math.Cos(frames / 70.0)),
							  (int)(120 + 40 * Math.Sin(frames / 70.0)));

					font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));
				}

				Display.EndFrame();
				Core.KeepAlive();
			}

			return frames;
		}

		private int StretchTestColored()
		{
			return StretchTest(true);
		}
		private int StretchTestPlain()
		{
			return StretchTest(false);
		}

		private int StretchTest(bool applyColor)
		{
			Surface surf = new Surface("jellybean.png");
			int frames = 0;

			surf.Color = Color.White;
			double count = 1;

			for (frames = 0; frames < totalFrames; frames++)
			{
				if (Display.CurrentWindow.IsClosed)
					return frames;

				count = 1.01 + Math.Cos(frames / 60.0);

				Display.BeginFrame();
				Display.Clear();
				for (int i = 0; i < 15; i++)
				{

					surf.SetScale(1.0, 1.0);
					surf.Color = Color.White;
					surf.Draw(10, 10);

					if (applyColor)
						surf.Color = Color.Orange;

					surf.SetScale(1.5, 1.5);
					surf.Draw(120, 10);

					if (applyColor)
						surf.Color = Color.LightGreen;

					surf.SetScale(count, 1.25);
					surf.Draw(10, 100);

					if (applyColor)
						surf.Color = Color.LightCoral;

					surf.SetScale(0.5 + count / 2, 0.5 + count / 2);
					surf.Draw((int)(150 + 40 * Math.Cos(frames / 70.0)),
							  (int)(120 + 40 * Math.Sin(frames / 70.0)));


				}
				font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));

				Display.EndFrame();
				Core.KeepAlive();
			}

			return frames;
		}

		private int DrawSurfaceTestColored()
		{
			return DrawSurfaceTest(true);
		}
		private int DrawSurfaceTestPlain()
		{
			return DrawSurfaceTest(false);
		}

		private int DrawSurfaceTest(bool applyColor)
		{
			Surface surf = new Surface("jellybean.png");
			List<Rects> rects = new List<Rects>();
			int frames = 0;

			surf.Color = Color.White;

			for (int i = 1; i < 10; i++)
			{
				Rects r = CreateRandomRects();

				rects.Add(r);
			}

			for (frames = 0; frames < totalFrames; frames++)
			{
				if (Display.CurrentWindow.IsClosed)
					return frames;

				rects.Add(CreateRandomRects());

				Display.BeginFrame();
				Display.Clear();

				if (applyColor)
				{
					for (int i = 0; i < rects.Count; i++)
					{
						surf.Color = rects[i].color;
						surf.Draw(rects[i].rect.Location);
					}
				}
				else

					for (int i = 0; i < rects.Count; i++)
					{
						surf.Draw(rects[i].rect.Location);
					}

				font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));

				Display.EndFrame();
				Core.KeepAlive();

			}

			return frames;
		}

		private int DrawRectTest()
		{
			List<Rects> rects = new List<Rects>();
			int frames = 0;

			for (int i = 1; i < 10; i++)
			{
				Rects r = CreateRandomRects();

				rects.Add(r);
			}

			for (frames = 0; frames < totalFrames; frames++)
			{
				if (Display.CurrentWindow.IsClosed)
					return frames;

				rects.Add(CreateRandomRects());

				Display.BeginFrame();
				Display.Clear();

				for (int i = 0; i < rects.Count; i++)
				{
					Display.DrawRect(rects[i].rect, rects[i].color);
				}

				font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));

				Display.EndFrame();
				Core.KeepAlive();

			}

			return frames;
		}

		private int FilledRectTest()
		{
			List<Rects> rects = new List<Rects>();
			int frames = 0;

			for (int i = 1; i < 10; i++)
			{
				Rects r = CreateRandomRects();

				rects.Add(r);
			}

			for (frames = 0; frames < totalFrames; frames++)
			{
				if (Display.CurrentWindow.IsClosed)
					return frames;

				rects.Add(CreateRandomRects());

				Display.BeginFrame();
				Display.Clear();

				for (int i = 0; i < rects.Count; i++)
				{
					Display.FillRect(rects[i].rect, rects[i].color);
				}

				font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));

				Display.EndFrame();
				Core.KeepAlive();

;
			}

			return frames;
		}

		private Rects CreateRandomRects()
		{
			Rects r = new Rects();

			r.rect = new Rectangle(
				rnd.Next(10, 250), rnd.Next(10, 250),
				rnd.Next(10, 40), rnd.Next(10, 40));
			r.color = Color.FromArgb(
				rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));

			return r;
		}

		#region IAgateTest Members

		public string Name { get { return "Performance Tester"; } }
		public string Category { get { return "Core"; } }

		#endregion
	}
}