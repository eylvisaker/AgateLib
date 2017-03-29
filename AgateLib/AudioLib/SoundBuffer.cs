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
using System.IO;
using System.Text;

namespace AgateLib.AudioLib
{
	using AgateLib.IO;
	using Drivers;
	using ImplementationBase;
	using Utility;

	/// <summary>
	/// A class which is used for loading and playing of sounds.
	/// Typically this is used for sound effects, whereas playing background music
	/// is done by the Music class.
	/// 
	/// The SoundBuffer class supports playing the same sound multiple times; this
	/// is done through the creation of SoundBufferSession objects for each time
	/// the SoundBuffer is played.  
	/// 
	/// SoundBufferSession objects may be recycled, to cut down on the amount of
	/// new calls.  
	/// 
	/// This class should support loading of .wav files, at the very least.
	/// </summary>
	public sealed class SoundBuffer
	{
		private string mFilename;
		private SoundBufferImpl mImpl;
		private double mVolume = 1.0;
		private double mPan = 0;
		private bool mIsDisposed = false;

		/// <summary>
		/// A list of existing SoundBufferSession objects.
		/// </summary>
		private List<SoundBufferSession> mSessions = new List<SoundBufferSession>();

		private SoundBuffer()
		{
			Audio.EventStopAllSounds += new Audio.AudioCoreEventDelegate(Stop);
		}
		/// <summary>
		/// Constructs a SoundBuffer object, loading audio data from the 
		/// specified file.
		/// </summary>
		/// <param name="filename"></param>
		public SoundBuffer(string filename, IReadFileProvider fileProvider = null)
		{
			mImpl = AgateApp.State.Factory.AudioFactory.CreateSoundBuffer(filename, fileProvider ?? AgateApp.Assets);

			mFilename = filename;
		}

		/// <summary>
		/// Constructs a SoundBuffer object, loading audio data from the passed stream.
		/// </summary>
		/// <param name="source"></param>
		public SoundBuffer(Stream source)
		{
			mImpl = AgateApp.State.Factory.AudioFactory.CreateSoundBuffer(source);
		}

		/// <summary>
		/// Disposes of the SoundBuffer object, and all SoundBufferSession objects
		/// created by this SoundBuffer.
		/// </summary>
		public void Dispose()
		{
			// trick to keep the list from changing while we iterate through it.
			List<SoundBufferSession> sessions = mSessions;
			mSessions = null;

			foreach (SoundBufferSession s in sessions)
				s.Dispose();

			if (mImpl != null)
			{
				mImpl.Dispose();
				mImpl = null;
			}

			mIsDisposed = true;
		}

		/// <summary>
		/// Returns the implemented object.
		/// </summary>
		public SoundBufferImpl Impl
		{
			get { return mImpl; }
		}
		/// <summary>
		/// Creates a SoundBufferSession object, for playing of this
		/// buffer.
		/// </summary>
		/// <returns></returns>
		public SoundBufferSession CreateSession()
		{
			return NewSoundBufferSession();
		}
		/// <summary>
		/// Creates a SoundBufferSession object and starts it playing.
		/// You can ignore the return value of this function if you just
		/// want simple playback.
		/// </summary>
		/// <returns></returns>
		public SoundBufferSession Play()
		{
			SoundBufferSession sb = NewSoundBufferSession();
			sb.Play();

			return sb;
		}

		/// <summary>
		/// Gets or sets a boolean value indicating whether or not the sound buffer
		/// should loop when it reaches the end.
		/// </summary>
		public bool Loop
		{
			get { return mImpl.Loop; }
			set { mImpl.Loop = value; }
		}

		/// <summary>
		/// Creates a new SoundBufferSession object, or finds one which
		/// can be recycled.
		/// </summary>
		/// <returns></returns>
		private SoundBufferSession NewSoundBufferSession()
		{
			if (mIsDisposed)
				throw new ObjectDisposedException("Cannot access a disposed SoundBuffer.");

			foreach (SoundBufferSession s in mSessions)
			{
				if (s.IsPlaying == false && s.Recycle)
				{
					s.Initialize();

					return s;
				}
			}

			SoundBufferSession result = new SoundBufferSession(this);

			mSessions.Add(result);

			return result;
		}

		/// <summary>
		/// Stops all SoundBufferSession objects created from this sound.
		/// </summary>
		public void Stop()
		{
			if (StopEvent != null)
				StopEvent();

			foreach (SoundBufferSession session in mSessions)
			{
				if (session.IsPlaying)
					session.Stop();
			}
		}
		/// <summary>
		/// Event which occurs when Stop is called on the SoundBuffer object.
		/// </summary>
		public event Audio.AudioCoreEventDelegate StopEvent;
		/// <summary>
		/// Filename this sound was originally loaded from.
		/// </summary>
		public string Filename
		{
			get { return mFilename; }
		}
		/// <summary>
		/// Gets or sets the default volume that will be used in new sessions. Range is:
		/// 0.0 Quiet
		/// 0.5 Sounds half volume
		/// 1.0 Full volume
		/// </summary>
		public double Volume
		{
			get
			{
				return mVolume;
			}
			set
			{
				mVolume = value;
			}
		}
		/// <summary>
		/// Gets or sets the left-right balance that will be used in new sessions. 
		/// -1 is entirely in the left speaker,
		///  0 is equally in both and,
		///  1 is entirely in the right speaker.
		/// </summary>
		public double Pan
		{
			get { return mPan; }
			set { mPan = value; }
		}

		/// <summary>
		/// Returns true if any SoundBufferSession objects are playing.
		/// </summary>
		public bool IsPlaying
		{
			get
			{
				if (mIsDisposed)
					throw new ObjectDisposedException("Cannot access a disposed SoundBuffer.");

				foreach (SoundBufferSession session in mSessions)
				{
					if (session.IsPlaying)
						return true;
				}

				return false;
			}
		}

		internal void AddSession(SoundBufferSession session)
		{
			if (mSessions == null)
				return;

			if (mSessions.Contains(session) == false)
				mSessions.Add(session);
		}
		internal void RemoveSession(SoundBufferSession session)
		{
			// this should only happen inside Dispose().
			if (mSessions == null)
				return;

			mSessions.Remove(session);
		}

	}

}
