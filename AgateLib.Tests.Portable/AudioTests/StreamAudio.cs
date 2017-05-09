using System;
using System.Diagnostics;
using System.IO;
using AgateLib.AudioLib;
using AgateLib.DisplayLib;

namespace AgateLib.Tests.Audio
{
	class StreamAudio : IAgateTest
	{
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

					short val = (short)(Math.Sin(time) * short.MaxValue / 2);

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

		public string Name
		{
			get { return "Streaming Audio"; }
		}

		public string Category
		{
			get { return "Audio"; }
		}

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				LoopingStream sa = new LoopingStream();
				sa.Frequency = 100;

				StreamingSoundBuffer buf = new StreamingSoundBuffer(sa, SoundFormat.Pcm16(44100), 100);

				buf.Play();

				Stopwatch w = new Stopwatch();
				w.Start();

				var font = Font.AgateSans;

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear();

					font.Color = Color.White;
					font.DrawText(0, 0, string.Format("Frequency: {0}", sa.Frequency));

					Display.EndFrame();
					AgateApp.KeepAlive();

					if (w.ElapsedMilliseconds > 500)
					{
						sa.Frequency += 50;
						w.Reset();
						w.Start();
					}
				}

				buf.Stop();
			}
		}
	}
}
