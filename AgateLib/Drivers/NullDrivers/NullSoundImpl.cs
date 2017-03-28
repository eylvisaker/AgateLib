//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.Diagnostics;

namespace AgateLib.Drivers.NullDrivers
{
	public class NullSoundImpl : AudioImpl
	{
		public override void Initialize()
		{
			Log.WriteLine("No audio driver found.  Audio will not be heard.");
		}

		protected internal override bool CapsBool(AgateLib.AudioLib.AudioBoolCaps audioBoolCaps)
		{
			return false;
		}
	}

	class NullSoundBufferImpl : SoundBufferImpl
	{
		public override double Volume { get; set; }
	}

	public class NullSoundBufferSessionImpl : SoundBufferSessionImpl
	{
		public override void Play()
		{
		}
		public override void Stop()
		{
		}
		public override double Volume
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}
		public override double Pan
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}
		public override bool IsPlaying
		{
			get { return false; }
		}

		public override int CurrentLocation
		{
			get { return 0; }
		}

		protected internal override void Initialize()
		{
		}

		public override bool IsPaused { get; set; }
	}

	public class NullMusicImpl : MusicImpl
	{
		protected override void OnSetLoop(bool value)
		{

		}
		public override void Dispose()
		{

		}
		public override void Play()
		{

		}
		public override void Stop()
		{

		}
		public override double Volume
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}
		public override double Pan
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}
		public override bool IsPlaying
		{
			get { return false; }
		}
	}

	public class NullStreamingSoundBuffer : StreamingSoundBufferImpl
	{
		public override void Play()
		{
		}

		public override void Stop()
		{
		}

		public override int ChunkSize { get;set;}
		public override bool IsPlaying
		{
			get { return false; }
		}

		public override void Dispose()
		{
		}

		public override double Pan { get;set;}
	}
}
