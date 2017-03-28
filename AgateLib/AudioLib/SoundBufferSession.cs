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

namespace AgateLib.AudioLib
{
	using Drivers;
	using ImplementationBase;

	/// <summary>
	/// A class which represents a playback instance of a SoundBuffer object.
	/// 
	/// After a SoundBufferSession is done playing, it may be recycled if its
	/// parent SoundBuffer object's Play or CreateSession methods are called.
	/// This behavior can be suppressed by setting the Recycle property to false.
	/// If you do this, you are responsible for freeing the unmanaged resources
	/// on the SoundBufferSession by calling its Dispose method.
	/// </summary>
	public sealed class SoundBufferSession
	{
		private SoundBuffer mSource;
		private SoundBufferSessionImpl impl;
		private bool mRecycle = true;

		private SoundBufferSession()
		{ }
		internal SoundBufferSession(SoundBuffer source)
		{
			impl = AgateApp.State.Factory.AudioFactory.CreateSoundBufferSession(this, source.Impl);

			mSource = source;

			Initialize();

		}

		internal void Initialize()
		{
			impl.Initialize();

			Volume = mSource.Volume;
			Pan = mSource.Pan;
		}

		/// <summary>
		/// Destroys the unmanaged resources associated with this object.
		/// </summary>
		public void Dispose()
		{
			impl.Dispose();
			mSource.RemoveSession(this);
		}

		/// <summary>
		/// Returns the implemented object.
		/// </summary>
		public SoundBufferSessionImpl Impl
		{
			get { return impl; }
		}
		/// <summary>
		/// Returns the SoundBuffer object which created this SoundBufferSession.
		/// </summary>
		public SoundBuffer Source
		{
			get { return mSource; }
		}
		/// <summary>
		/// Begins playback of the SoundBufferSession object.
		/// </summary>
		public void Play()
		{
			impl.Play();
		}
		/// <summary>
		/// Stops playback. Allows the SoundBufferSession to release sound card resources.
		/// </summary>
		public void Stop()
		{
			impl.Stop();
		}

		/// <summary>
		/// Gets or sets the volume. Range is:
		/// 0.0 Quiet
		/// 0.5 Sounds half volume
		/// 1.0 Full volume
		/// </summary>
		public double Volume
		{
			get
			{
				return impl.Volume;
			}
			set
			{
				impl.Volume = value;
			}
		}
		/// <summary>
		/// Gets or sets the left-right balance.  
		/// -1 is entirely in the left speaker,
		///  0 is equally in both and,
		///  1 is entirely in the right speaker.
		/// </summary>
		public double Pan
		{
			get { return impl.Pan; }
			set { impl.Pan = value; }
		}

		/// <summary>
		/// Gets the current location in the sound buffer.
		/// </summary>
		public int CurrentLocation
		{
			get { return impl.CurrentLocation; }
		}

		/// <summary>
		/// Returns true if this Session is playing.
		/// </summary>
		public bool IsPlaying
		{
			get { return impl.IsPlaying; }
		}

		/// <summary>
		/// Gets or sets whether this SoundBufferSession playback is paused.
		/// If it is paused it still retains resources. Call Stop to allow
		/// the SoundBufferSession to release resources.
		/// </summary>
		public bool IsPaused
		{
			get { return impl.IsPaused; }
			set { impl.IsPaused = value; }
		}
		/// <summary>
		/// Gets or sets a bool value which indicates whether or not this
		/// SoundBufferSession object should be recycled when it is done playing.
		/// 
		/// If you set this to false, you should Dispose the SoundBufferSession
		/// object yourself when you're done with it.
		/// </summary>
		public bool Recycle
		{
			get { return mRecycle; }
			set
			{
				if (value != mRecycle)
				{
					mRecycle = value;

					if (value)
						mSource.AddSession(this);
					else
						mSource.RemoveSession(this);
				}
			}
		}
	}

}
