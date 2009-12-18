using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.AudioLib;
using AgateLib.DisplayLib;

namespace Tests.AudioTests
{
	class GenerateAudio : IAgateTest 
	{

		public string Name
		{
			get { return "Generate Audio"; }
		}

		public string Category
		{
			get { return "Audio"; }
		}

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup())
			{
				setup.AskUser = true;
				setup.Initialize(true, true, false);
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateWindowed("Generate Audio", 640, 480);

				short[] s = new short[22050];

				int frequency = 100;
				FillSoundBuffer(s, frequency);
				
				SoundBuffer buf = new SoundBuffer(s);
				
				buf.Loop = true;
				
				SoundBufferSession ses = buf.Play();

				Stopwatch w = new Stopwatch();
				w.Start();

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear();

					FontSurface.AgateSans14.Color = AgateLib.Geometry.Color.White;
					FontSurface.AgateSans14.DrawText(0, 0, string.Format("Frequency: {0}", frequency));
 
					Display.EndFrame();
					Core.KeepAlive();

					if (w.ElapsedMilliseconds > 800)
					{
						frequency += 50;
						FillSoundBuffer(s, frequency);
						buf.Write(s, 0, 0, s.Length);

						w.Reset();
						w.Start();
					}
				}
			}
		}

		private void FillSoundBuffer(short[] s, double frequency)
		{
			for (int i = 0; i < s.Length; i++)
			{
				double index = i / (double)s.Length;
				index *= 2 * Math.PI * frequency;

				s[i] = (short)(Math.Sin(index) * short.MaxValue);
			}
		}

	}
}
