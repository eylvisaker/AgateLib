﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.AudioLib;
using AgateLib.DisplayLib;

namespace Tests.AudioTests
{
	class StreamAudio : IAgateTest 
	{
		public string Name
		{
			get { return "Streaming Audio"; }
		}

		public string Category
		{
			get { return "Audio"; }
		}

		class LoopingStream : Stream 
		{
			public double Frequency { get; set; }

			public LoopingStream()
			{
			}

			public override bool CanRead
			{
				get { return true; }
			}

			public override bool CanSeek
			{
				get { return true; }
			}

			public override bool CanWrite
			{
				get { return false; }
			}

			public override void Flush()
			{
				throw new NotSupportedException();
			}

			public override long Length
			{
				get { return SamplingFrequency; }
			}

			public override long Position
			{
				get
				{
					return 0;
				}
				set
				{
				}
			}

			double lastValue;
			const int SamplingFrequency = 44100;

			public override int Read(byte[] buffer, int offset, int count)
			{
				double lv = lastValue;
				
				for (int i = 0; i < count / 2; i++)
				{
					double time = i / (double)SamplingFrequency;
					time *= 2 * Math.PI * Frequency;
					time += lv;
					lastValue = time;

					short val = (short)(Math.Sin(time) * short.MaxValue/2);

					buffer[offset + i * 2] = (byte)(val & 0xff);
					buffer[offset + i * 2 + 1] = (byte)(val >> 8);
				}

				return count;
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new NotImplementedException();
			}
			public override void SetLength(long value)
			{
				throw new NotImplementedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotImplementedException();
			}
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

				LoopingStream sa = new LoopingStream();
				sa.Frequency = 100;

				StreamingSoundBuffer buf = new StreamingSoundBuffer(sa, SoundFormat.Pcm16(44100), 100);
				
				buf.Play();

				Stopwatch w = new Stopwatch();
				w.Start();

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear();

					FontSurface.AgateSans14.Color = AgateLib.Geometry.Color.White;
					FontSurface.AgateSans14.DrawText(0, 0, string.Format("Frequency: {0}", sa.Frequency));
 
					Display.EndFrame();
					Core.KeepAlive();

					if (w.ElapsedMilliseconds > 500)
					{
						sa.Frequency += 50;
						w.Reset();
						w.Start();
					}
				}
			}
		}
	}
}
