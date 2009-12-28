using System;
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
	class GenerateAudio : IAgateTest 
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
			byte[] buffer;
			int pos;

			public LoopingStream(byte[] buffer)
			{
				this.buffer = buffer;
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
				get { return buffer.Length; }
			}

			public override long Position
			{
				get
				{
					return pos;
				}
				set
				{
					pos = (int) value;
				}
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				if (count < Length - pos)
				{
					Array.Copy(this.buffer, pos, buffer, offset, count);
					pos += count;
				}
				else
				{
					int firstcount = (int)(Length - pos);

					Array.Copy(this.buffer, pos, buffer, offset, firstcount);
					
					int secondCount = count - firstcount;

					Array.Copy(this.buffer, 0, buffer, offset + firstcount, secondCount);
					pos = secondCount;
				}

				return count;
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
					case SeekOrigin.Begin:
						pos = (int)offset;
						break;

					case SeekOrigin.Current:
						pos += (int)offset;
						pos %= (int)Length;
						break;

					case SeekOrigin.End:
						pos = (int)(Length + offset);
						pos %= (int)Length;
						break;
				}

				return pos;
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

				short[] s = new short[44100];

				int frequency = 100;
				FillSoundBuffer(s, frequency);

				byte[] buffer = new byte[s.Length * 2];
				Buffer.BlockCopy(s, 0, buffer, 0, buffer.Length);

				LoopingStream sa = new LoopingStream(buffer);
				StreamingSoundBuffer buf = new StreamingSoundBuffer(sa, SoundFormat.Pcm16(44100), 4100);
				
				buf.Play();

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
						Buffer.BlockCopy(s, 0, buffer, 0, buffer.Length);

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
